using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Scripting.Hosting;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;
using AtsEx.Scripting;
using AtsEx.Scripting.IronPython2;

using BeaconBase = AtsEx.Extensions.ExtendedBeacons.ExtendedBeaconBase<AtsEx.Extensions.ExtendedBeacons.PassedEventArgs>;
using TrainObservingBeaconBase = AtsEx.Extensions.ExtendedBeacons.ExtendedBeaconBase<AtsEx.Extensions.ExtendedBeacons.TrainPassedEventArgs>;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Obsolete]
    internal partial class ExtendedBeaconSet : IExtendedBeaconSet
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ExtendedBeaconSet>(@"ExtendedBeacons");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> BeaconRepeaterNotEnded { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ArgumentMissing { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ItemName { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ScriptLanguageNotRecognized { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static ExtendedBeaconSet()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public ReadOnlyDictionary<Identifier, BeaconBase> Beacons { get; }
        public ReadOnlyDictionary<Identifier, TrainObservingBeaconBase> TrainObservingBeacons { get; }
        public ReadOnlyDictionary<Identifier, BeaconBase> PreTrainObservingBeacons { get; }

        protected ExtendedBeaconSet(
            IDictionary<Identifier, BeaconBase> beacons, IDictionary<Identifier, TrainObservingBeaconBase> trainObservingBeacons, IDictionary<Identifier, BeaconBase> preTrainObservingBeacons)
        {
            Beacons = new ReadOnlyDictionary<Identifier, BeaconBase>(beacons);
            TrainObservingBeacons = new ReadOnlyDictionary<Identifier, TrainObservingBeaconBase>(trainObservingBeacons);
            PreTrainObservingBeacons = new ReadOnlyDictionary<Identifier, BeaconBase>(preTrainObservingBeacons);
        }

        public static ExtendedBeaconSet Load(INative native, IBveHacker bveHacker, Scenario scenario)
        {
            Dictionary<Identifier, BeaconBase> beacons = new Dictionary<Identifier, BeaconBase>();
            Dictionary<Identifier, TrainObservingBeaconBase> trainObservingBeacons = new Dictionary<Identifier, TrainObservingBeaconBase>();
            Dictionary<Identifier, BeaconBase> preTrainObservingBeacons = new Dictionary<Identifier, BeaconBase>();

            List<ICompilationErrorCheckable> errorCheckList = new List<ICompilationErrorCheckable>();
            Dictionary<Type, Dictionary<ScriptLanguage, Dictionary<string, dynamic>>> cachedScripts = new Dictionary<Type, Dictionary<ScriptLanguage, Dictionary<string, dynamic>>>();

            foreach (IStatement statement in bveHacker.MapStatements.GetAll(Identifiers.Put))
            {
                if (statement.From != statement.To)
                {
                    bveHacker.LoadErrorManager.Throw(Resources.Value.BeaconRepeaterNotEnded.Value);
                }

                if (statement.AdditionalDeclaration.Length <= 3) throw new BveFileLoadException(Resources.Value.ArgumentMissing.Value, Resources.Value.ItemName.Value);

                Identifier beaconNameIdentifier = statement.AdditionalDeclaration[0];
                Identifier scriptLanguageIdentifier = statement.AdditionalDeclaration[1];
                Identifier observingTargetTrackIdentifier = statement.AdditionalDeclaration[2];

                if (beaconNameIdentifier == Identifier.Null) beaconNameIdentifier = new Identifier((Namespace)null, Guid.NewGuid().ToString());

                try
                {
                    ScriptLanguage scriptLanguage = ScriptLanguageIdentifierConverter.Convert(scriptLanguageIdentifier);
                    ObservingTargetTrack observingTargetTrack = Identifiers.ConvertObservingTargetTrack(observingTargetTrackIdentifier);

                    for (int i = 3; i < statement.AdditionalDeclaration.Length; i++)
                    {
                        Identifier observingTargetTrainIdentifier = statement.AdditionalDeclaration[i];
                        ObservingTargetTrain observingTargetTrain = Identifiers.ConvertObservingTargetTrain(observingTargetTrainIdentifier);

                        string code = Regex.Unescape(statement.Argument).Replace('`', '\"');

                        switch (observingTargetTrain)
                        {
                            case ObservingTargetTrain.Myself:
                            {
                                IPluginScript<ExtendedBeaconGlobalsBase<PassedEventArgs>> script = CreateScript<PassedEventArgs>(code, scriptLanguage);
                                Beacon beacon = new Beacon(native, bveHacker, statement, beaconNameIdentifier, observingTargetTrack, observingTargetTrain, script);

                                beacons[beaconNameIdentifier] = beacon;
                                errorCheckList.Add(beacon);
                                break;
                            }

                            case ObservingTargetTrain.Trains:
                            {
                                IPluginScript<ExtendedBeaconGlobalsBase<TrainPassedEventArgs>> script = CreateScript<TrainPassedEventArgs>(code, scriptLanguage);
                                TrainObservingBeacon beacon = new TrainObservingBeacon(native, bveHacker, statement, beaconNameIdentifier, observingTargetTrack, scenario.Trains, script);

                                trainObservingBeacons[beaconNameIdentifier] = beacon;
                                errorCheckList.Add(beacon);
                                break;
                            }

                            case ObservingTargetTrain.PreTrain:
                            {
                                IPluginScript<ExtendedBeaconGlobalsBase<PassedEventArgs>> script = CreateScript<PassedEventArgs>(code, scriptLanguage);
                                Beacon beacon = new Beacon(native, bveHacker, statement, beaconNameIdentifier, observingTargetTrack, observingTargetTrain, script);

                                preTrainObservingBeacons[beaconNameIdentifier] = beacon;
                                errorCheckList.Add(beacon);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new BveFileLoadException(ex, Resources.Value.ItemName.Value);
                }
            }

            errorCheckList.ForEach(beacon => beacon.CheckCompilationErrors());

            return new ExtendedBeaconSet(OrderBeacons(beacons), OrderBeacons(trainObservingBeacons), OrderBeacons(preTrainObservingBeacons));


            IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>> CreateScript<TPassedEventArgs>(string code, ScriptLanguage language) where TPassedEventArgs : PassedEventArgs
            {
                string mainMapDirectory = Path.GetDirectoryName(bveHacker.ScenarioInfo.RouteFiles.SelectedFile.Path);

                if (!cachedScripts.ContainsKey(typeof(TPassedEventArgs))) cachedScripts[typeof(TPassedEventArgs)] = new Dictionary<ScriptLanguage, Dictionary<string, dynamic>>();
                if (!cachedScripts[typeof(TPassedEventArgs)].ContainsKey(language)) cachedScripts[typeof(TPassedEventArgs)][language] = new Dictionary<string, dynamic>();

                if (cachedScripts[typeof(TPassedEventArgs)][language].ContainsKey(code))
                {
                    IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>> cachedScript = cachedScripts[typeof(TPassedEventArgs)][language][code] as IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>>;
                    return cachedScript.Clone() as IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>>;
                }

                IPluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>> script;
                switch (language)
                {
                    case ScriptLanguage.CSharpScript:
                        script = new AtsEx.Scripting.CSharp.PluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>>(code, mainMapDirectory, Resources.Value.ItemName.Value);
                        break;

                    case ScriptLanguage.IronPython2:
                        ScriptEngine scriptEngine = ScriptEngineProvider.CreateEngine(mainMapDirectory);
                        ScriptScope scriptScope = ScriptEngineProvider.CreateScope(scriptEngine);

                        script = new AtsEx.Scripting.IronPython2.PluginScript<ExtendedBeaconGlobalsBase<TPassedEventArgs>>(code, scriptEngine, scriptScope, Resources.Value.ItemName.Value);
                        break;

                    default:
                        throw new DevelopException(string.Format(Resources.Value.ScriptLanguageNotRecognized.Value, language));
                }

                cachedScripts[typeof(TPassedEventArgs)][language][code] = script;

                return script;
            }

            Dictionary<Identifier, TBeacon> OrderBeacons<TBeacon>(Dictionary<Identifier, TBeacon> source) where TBeacon : IExtendedBeacon
                => source.OrderBy(item => item.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public void Tick(double location, double preTrainLocation)
        {
            foreach (Beacon beacon in Beacons.Values) beacon.Tick(location);
            foreach (TrainObservingBeacon beacon in TrainObservingBeacons.Values) beacon.Tick();
            foreach (Beacon beacon in PreTrainObservingBeacons.Values) beacon.Tick(preTrainLocation);
        }
    }
}
