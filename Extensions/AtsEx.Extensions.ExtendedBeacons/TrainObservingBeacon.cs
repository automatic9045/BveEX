using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;
using AtsEx.Scripting;

namespace AtsEx.Extensions.ExtendedBeacons
{
    [Obsolete]
    internal sealed class TrainObservingBeacon : ExtendedBeaconBase<TrainPassedEventArgs>
    {
        private readonly List<TrainInfo> TargetTrains;

        public TrainObservingBeacon(INative native, IBveHacker bveHacker,
            IStatement definedStatement, Identifier beaconName, ObservingTargetTrack observingTargetTrack,
            IDictionary<string, Train> targetTrains, IPluginScript<ExtendedBeaconGlobalsBase<TrainPassedEventArgs>> script)
            : base(native, bveHacker, definedStatement, beaconName, observingTargetTrack, ObservingTargetTrain.Trains, script)
        {
            TargetTrains = targetTrains.Select(train => new TrainInfo(train.Key, train.Value)).ToList();
        }

        internal void Tick()
        {
            foreach (TrainInfo train in TargetTrains)
            {
                train.UpdateLocation();

                if (train.OldLocation < Location && Location <= train.Location && IsCurrentTrackTarget(train.Train))
                {
                    NotifyPassed(train, Direction.Forward);
                }
                else if (Location < train.OldLocation && train.Location <= Location && IsCurrentTrackTarget(train.Train))
                {
                    NotifyPassed(train, Direction.Backward);
                }
            }


            bool IsCurrentTrackTarget(Train train) => ObservingTargetTrack == ObservingTargetTrack.AllTracks || train.TrainInfo.TrackKey == DefinedStatement.DefinedStructure.TrackKey;

            void NotifyPassed(TrainInfo senderTrain, Direction direction)
            {
                TrainPassedEventArgs eventArgs = new TrainPassedEventArgs(senderTrain.Name, senderTrain.Train, direction);
                TrainPassedGlobals globals = new TrainPassedGlobals(Native, BveHacker, this, eventArgs);
                Script.Run(globals);
                base.NotifyPassed(globals.GetEventArgsWithScriptVariables());
            }
        }


        class TrainInfo
        {
            public string Name { get; }
            public Train Train { get; }

            public double OldLocation { get; private set; }
            public double Location { get; private set; }

            public TrainInfo(string name, Train train)
            {
                Name = name;
                Train = train;

                OldLocation = train.Location;
                Location = train.Location;
            }

            public void UpdateLocation()
            {
                OldLocation = Location;
                Location = Train.Location;
            }
        }
    }
}
