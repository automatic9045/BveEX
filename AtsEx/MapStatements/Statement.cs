using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;

namespace AtsEx.MapStatements
{
#pragma warning disable CS0612 // 型またはメンバーが旧型式です
    internal sealed class Statement : IStatement
    {
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
        private double OldVehicleLocation = double.NegativeInfinity;
        private double OldPreTrainLocation = double.NegativeInfinity;

        public Identifier Name { get; }
        public Identifier[] AdditionalDeclaration { get; }
        public string Argument { get; }

        public double From => DefinedStructure.Location;
        public double To { get; }
        public RepeatedStructure DefinedStructure { get; }

        private readonly IReadOnlyDictionary<Train, ObservableTrain> TrainsSource;

        private readonly Dictionary<Train, ObservableTrain> TrainsObserving = new Dictionary<Train, ObservableTrain>();
        IReadOnlyCollection<Train> IStatement.TrainsObserving => TrainsObserving.Keys;

        public Statement(Identifier name, Identifier[] additionalDeclaration, string argument, RepeatedStructure definedStructure, double to,
            IReadOnlyDictionary<Train, ObservableTrain> trainsSource)
        {
            Name = name;
            AdditionalDeclaration = additionalDeclaration;
            Argument = argument;

            DefinedStructure = definedStructure;
            To = to;

            TrainsSource = trainsSource;
        }

        public void Tick(double vehicleLocation, double preTrainLocation)
        {
            {
                if (OldVehicleLocation < From && From <= vehicleLocation)
                {
                    Entered?.Invoke(this, new PassedEventArgs(Direction.Forward));
                }
                else if (To < OldVehicleLocation && vehicleLocation <= To)
                {
                    Entered?.Invoke(this, new PassedEventArgs(Direction.Backward));
                }

                if (OldVehicleLocation < To && To <= vehicleLocation)
                {
                    Exited?.Invoke(this, new PassedEventArgs(Direction.Forward));
                }
                else if (From < OldVehicleLocation && vehicleLocation <= From)
                {
                    Exited?.Invoke(this, new PassedEventArgs(Direction.Backward));
                }


                OldVehicleLocation = vehicleLocation;
            }

            {
                if (OldPreTrainLocation < From && From <= preTrainLocation)
                {
                    PreTrainEntered?.Invoke(this, new PassedEventArgs(Direction.Forward));
                }
                else if (To < OldPreTrainLocation && preTrainLocation <= To)
                {
                    PreTrainEntered?.Invoke(this, new PassedEventArgs(Direction.Backward));
                }

                if (OldPreTrainLocation < To && To <= preTrainLocation)
                {
                    PreTrainExited?.Invoke(this, new PassedEventArgs(Direction.Forward));
                }
                else if (From < OldPreTrainLocation && preTrainLocation <= From)
                {
                    PreTrainExited?.Invoke(this, new PassedEventArgs(Direction.Backward));
                }

                OldPreTrainLocation = preTrainLocation;
            }

            foreach (ObservableTrain train in TrainsObserving.Values)
            {
                if (train.OldLocation < From && From <= train.Location)
                {
                    TrainEntered?.Invoke(this, new TrainPassedEventArgs(train.Name, train.Train, Direction.Forward));
                }
                else if (From < train.OldLocation && train.Location <= From)
                {
                    TrainEntered?.Invoke(this, new TrainPassedEventArgs(train.Name, train.Train, Direction.Backward));
                }

                if (train.OldLocation < To && To <= train.Location)
                {
                    TrainExited?.Invoke(this, new TrainPassedEventArgs(train.Name, train.Train, Direction.Forward));
                }
                else if (From < train.OldLocation && train.Location <= From)
                {
                    TrainExited?.Invoke(this, new TrainPassedEventArgs(train.Name, train.Train, Direction.Backward));
                }
            }
        }

        public void ObserveTrain(Train train)
            => TrainsObserving[train] = TrainsSource[train];

        public void ObserveTrains(IEnumerable<Train> trains)
        {
            foreach (Train train in trains)
            {
                ObserveTrain(train);
            }
        }

        public event EventHandler<PassedEventArgs> Entered;
        public event EventHandler<PassedEventArgs> PreTrainEntered;
        public event EventHandler<TrainPassedEventArgs> TrainEntered;

        public event EventHandler<PassedEventArgs> Exited;
        public event EventHandler<PassedEventArgs> PreTrainExited;
        public event EventHandler<TrainPassedEventArgs> TrainExited;
    }
}
