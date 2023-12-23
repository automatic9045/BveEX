using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.Native;
using AtsEx.Plugins.Scripting;
using AtsEx.Plugins.Scripting.CSharp;
using AtsEx.Plugins.Scripting.IronPython2;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Plugins
{
    internal partial class PluginLoader
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginLoader>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginClassNotFound { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginVersionNotSupported { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ConstructorNotFound { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotSetIdentifier { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> WrongPluginType { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> MustUseExtensions { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> MaybeBecauseBuiltForDifferentVersion { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();
        private static readonly Version SupportedMinPluginHostVersion = new Version(0, 17);

        static PluginLoader()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        protected readonly NativeImpl Native;
        protected readonly BveHacker BveHacker;
        protected readonly IExtensionSet Extensions;
        protected readonly IPluginSet Plugins;

        public PluginLoader(NativeImpl native, BveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins)
        {
            Native = native;
            BveHacker = bveHacker;
            Extensions = extensions;
            Plugins = plugins;
        }

        public Dictionary<string, PluginBase> Load(PluginSourceSet pluginSources)
        {
            Dictionary<string, PluginBase> plugins = new Dictionary<string, PluginBase>();
            PluginLoadErrorQueue loadErrorQueue = new PluginLoadErrorQueue(BveHacker.LoadErrorManager);

            foreach (IPluginPackage pluginPackage in pluginSources)
            {
                switch (pluginPackage)
                {
                    case AssemblyPluginPackage assemblyPluginPackage:
                    {
                        try
                        {
                            List<PluginBase> loadedPlugins = LoadFromAssembly(assemblyPluginPackage.Identifier, assemblyPluginPackage.Assembly, pluginSources.PluginType, pluginSources.AllowNonPluginAssembly);
                            loadedPlugins.ForEach(plugin => plugins[plugin.Identifier] = plugin);
                        }
                        catch (Exception ex)
                        {
                            loadErrorQueue.OnFailedToLoadAssembly(assemblyPluginPackage.Assembly, ex);
                        }

                        break;
                    }

                    case ScriptPluginPackage scriptPluginPackage:
                    {
                        try
                        {
                            PluginBuilder pluginBuilder = new PluginBuilder(Native, BveHacker, Extensions, Plugins, scriptPluginPackage.Identifier.Text);

                            ScriptPluginBase plugin;
                            switch (scriptPluginPackage.ScriptLanguage)
                            {
                                case ScriptLanguage.CSharpScript:
                                    plugin = CSharpScriptPlugin.FromPackage(pluginBuilder, pluginSources.PluginType, scriptPluginPackage);
                                    break;

                                case ScriptLanguage.IronPython2:
                                    plugin = IronPython2Plugin.FromPackage(pluginBuilder, pluginSources.PluginType, scriptPluginPackage);
                                    break;

                                default:
                                    throw new NotImplementedException();
                            }

                            plugins[scriptPluginPackage.Identifier.Text] = plugin;
                        }
                        catch (Exception ex)
                        {
                            loadErrorQueue.OnFailedToLoadScriptPlugin(scriptPluginPackage, ex);
                        }

                        break;
                    }

                    // TODO: ここで他の種類のプラグイン（ネイティブなど）を読み込む

                    default:
                        throw new NotImplementedException();
                }
            }

            loadErrorQueue.Resolve();
            return plugins;


            List<PluginBase> LoadFromAssembly(Identifier identifier, Assembly assembly, PluginType pluginType, bool allowNonPluginAssembly)
            {
                string fileName = Path.GetFileName(assembly.Location);

                Version pluginHostVersion = App.Instance.AtsExPluginHostAssembly.GetName().Version;
                AssemblyName referencedPluginHost = assembly.GetReferencedPluginHost();
                if (referencedPluginHost is null)
                {
                    return allowNonPluginAssembly
                        ? new List<PluginBase>()
                        : throw new BveFileLoadException(string.Format(Resources.Value.PluginClassNotFound.Value, nameof(PluginBase), App.Instance.ProductShortName), fileName);
                }
                else if (referencedPluginHost.Version < SupportedMinPluginHostVersion)
                {
                    throw new BveFileLoadException(string.Format(
                        Resources.Value.PluginVersionNotSupported.Value,
                        referencedPluginHost.Version, App.Instance.ProductShortName, pluginHostVersion, SupportedMinPluginHostVersion.ToString(2)), fileName);
                }

                Type[] allTypes = assembly.GetTypes();
                IEnumerable<Type> pluginTypes = allTypes.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(PluginBase)));
                if (!pluginTypes.Any())
                {
                    throw new BveFileLoadException(string.Format(Resources.Value.PluginClassNotFound.Value, nameof(PluginBase), App.Instance.ProductShortName), fileName);
                }

                List<(Type, ConstructorInfo)> constructors = new List<(Type, ConstructorInfo)>();
                foreach (Type type in pluginTypes)
                {
                    ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(PluginBuilder) });
                    if (constructor is null) continue;

                    constructors.Add((type, constructor));
                }

                switch (constructors.Count)
                {
                    case 0:
                        throw new BveFileLoadException(string.Format(Resources.Value.ConstructorNotFound.Value, nameof(PluginBase), typeof(PluginBuilder)), fileName);

                    case 1:
                        break;

                    default:
                        if (!(identifier is RandomIdentifier))
                        {
                            throw new BveFileLoadException(string.Format(Resources.Value.CannotSetIdentifier.Value, fileName), pluginSources.Name);
                        }
                        break;
                }

                List<PluginBase> constructedPlugins = constructors.ConvertAll(constructor =>
                {
                    (Type type, ConstructorInfo constructorInfo) = constructor;

                    PluginBase pluginInstance = constructorInfo.Invoke(new object[] { new PluginBuilder(Native, BveHacker, Extensions, Plugins, GenerateIdentifier()) }) as PluginBase;
                    if (pluginInstance.PluginType != pluginType)
                    {
                        throw new InvalidOperationException(string.Format(Resources.Value.WrongPluginType.Value, pluginType.GetTypeString(), pluginInstance.PluginType.GetTypeString()));
                    }
                    else if (pluginInstance.PluginType == PluginType.MapPlugin && !pluginInstance.UseBveHacker)
                    {
                        throw new NotSupportedException(string.Format(Resources.Value.MustUseExtensions.Value, pluginInstance.PluginType.GetTypeString(), App.Instance.ProductShortName));
                    }

                    return pluginInstance;
                });

                return constructedPlugins;


                string GenerateIdentifier() => constructors.Count == 1 ? identifier.Text : Guid.NewGuid().ToString();
            }
        }
    }
}
