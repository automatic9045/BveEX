using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.Extensions;
using BveEx.Extensions.ConductorPatch;

namespace BveEx.Samples.VehiclePlugins.ConductorPatchTest
{
    internal class TestConductor : ConductorBase
    {
        private bool HasStopPositionChecked = false;

        protected override event EventHandler FixStopPositionRequested;
        protected override event EventHandler StopPositionChecked;
        protected override event EventHandler DoorOpening;
        protected override event EventHandler DepartureSoundPlaying;
        protected override event EventHandler DoorClosing;
        protected override event EventHandler DoorClosed;

        public TestConductor(Conductor original) : base(original)
        {
        }

        protected override MethodOverrideMode OnJumped(int stationIndex, bool isDoorClosed)
        {
            HasStopPositionChecked = false;

            Original.Stations.GoToByIndex(stationIndex - 1);
            Original.Doors.SetState(DoorState.Close, DoorState.Close);

            Station currentStation = Original.Stations.Count <= stationIndex ? null : Original.Stations[stationIndex] as Station;
            int doorSide = currentStation is null || currentStation.Pass || isDoorClosed ? 0 : currentStation.DoorSide;
            if (doorSide == 0) Original.Stations.GoToByIndex(stationIndex);

            return MethodOverrideMode.SkipOriginal;
        }

        protected override MethodOverrideMode OnDoorStateChanged()
        {
            if (Original.Doors.AreAllClosed && HasStopPositionChecked)
            {
                HasStopPositionChecked = false;
                Original.Stations.GoToByIndex(Original.Stations.CurrentIndex + 1);
                DoorClosed(this, EventArgs.Empty);
            }

            return MethodOverrideMode.SkipOriginal;
        }

        protected override MethodOverrideMode OnTick()
        {
            Station nextStation = GetNextStation();
            if (!(nextStation is null))
			{
				if (nextStation.Pass || nextStation.DoorSide == 0)
				{
                    double location = Original.LocationManager.Location;
					if ((Math.Abs(Original.LocationManager.SpeedMeterPerSecond) < 0.01f && location >= nextStation.MinStopPosition) || location >= nextStation.MaxStopPosition)
					{
                        Original.Stations.GoToByIndex(Original.Stations.CurrentIndex + 1);
					}
				}
			}

			return MethodOverrideMode.SkipOriginal;
        }

        public void RequestFixStopPosition()
        {
            FixStopPositionRequested(this, EventArgs.Empty);
        }

        public void OpenDoors(DoorSide doorSide)
        {
            Station nextStation = GetNextStation();
            if (!(nextStation is null) && nextStation.DoorSide == ToDoorSideNumber(doorSide))
            {
                if (!HasStopPositionChecked)
                {
                    HasStopPositionChecked = true;
                    StopPositionChecked(this, EventArgs.Empty);
                }

                Original.Doors.GetSide(doorSide).OpenDoors();
                DoorOpening(this, EventArgs.Empty);
            }
            else
            {
                Original.Doors.GetSide(doorSide).OpenDoors();
            }
        }

        public void PlayDepartureSound()
        {
            DepartureSoundPlaying(this, EventArgs.Empty);
        }

        public void CloseDoors(DoorSide doorSide)
        {
            Station nextStation = GetNextStation();
            if (!(nextStation is null) && nextStation.DoorSide == ToDoorSideNumber(doorSide))
            {
                if (!HasStopPositionChecked)
                {
                    HasStopPositionChecked = true;
                    StopPositionChecked(this, EventArgs.Empty);
                }

                Original.Doors.GetSide(doorSide).CloseDoors(nextStation.StuckInDoorMilliseconds);
                DoorClosing(this, EventArgs.Empty);
            }
            else
            {
                Original.Doors.GetSide(doorSide).CloseDoors(nextStation is null ? 0 : nextStation.StuckInDoorMilliseconds);
            }
        }

        private Station GetNextStation()
            => Original.Stations.Count <= Original.Stations.CurrentIndex + 1 ? null : Original.Stations[Original.Stations.CurrentIndex + 1] as Station;

        private int ToDoorSideNumber(DoorSide doorSide) => (int)doorSide * 2 - 1;
    }
}
