using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using BveTypes.ClassWrappers;
using TypeWrapping;

using AtsEx.Native;
using AtsEx.PluginHost;

namespace AtsEx
{
    internal abstract partial class AtsEx
    {
        internal sealed partial class AsInputDevice : AtsEx
        {
            private readonly PatchSet Patches;

            public event EventHandler<ValueEventArgs<ScenarioInfo>> ScenarioOpened;
            public event EventHandler<ValueEventArgs<Scenario>> ScenarioClosed;

            public event EventHandler<ValueEventArgs<VehicleSpec>> OnSetVehicleSpec;
            public event EventHandler<ValueEventArgs<DefaultBrakePosition>> OnInitialize;
            public event EventHandler<OnElapseEventArgs> PostElapse;
            public event EventHandler<ValueEventArgs<int>> OnSetPower;
            public event EventHandler<ValueEventArgs<int>> OnSetBrake;
            public event EventHandler<ValueEventArgs<int>> OnSetReverser;
            public event EventHandler<ValueEventArgs<ATSKeys>> OnKeyDown;
            public event EventHandler<ValueEventArgs<ATSKeys>> OnKeyUp;
            public event EventHandler<ValueEventArgs<HornType>> OnHornBlow;
            public event EventHandler OnDoorOpen;
            public event EventHandler OnDoorClose;
            public event EventHandler<ValueEventArgs<int>> OnSetSignal;
            public event EventHandler<ValueEventArgs<BeaconData>> OnSetBeaconData;

            public AsInputDevice(BveTypeSet bveTypes) : base(bveTypes)
            {
                ClassMemberSet mainFormMembers = BveHacker.BveTypes.GetClassInfoOf<MainForm>();
                ClassMemberSet scenarioMembers = BveHacker.BveTypes.GetClassInfoOf<Scenario>();
                ClassMemberSet atsPluginMembers = BveHacker.BveTypes.GetClassInfoOf<AtsPlugin>();

                Patches = new PatchSet(mainFormMembers, scenarioMembers, atsPluginMembers);

                PatchEventInitializer patchEventInitializer = new PatchEventInitializer(this);
                patchEventInitializer.InitializeEvents();
            }

            public override void Dispose()
            {
                Patches.Dispose();
                base.Dispose();
            }


            internal class ValueEventArgs<T> : EventArgs
            {
                public T Value { get; }

                public ValueEventArgs(T value)
                {
                    Value = value;
                }
            }

            internal class OnElapseEventArgs : EventArgs
            {
                public VehicleState VehicleState { get; }
                public int[] Panel { get; }
                public int[] Sound { get; }

                public OnElapseEventArgs(VehicleState vehicleState, int[] panel, int[] sound)
                {
                    VehicleState = vehicleState;
                    Panel = panel;
                    Sound = sound;
                }
            }
        }
    }
}
