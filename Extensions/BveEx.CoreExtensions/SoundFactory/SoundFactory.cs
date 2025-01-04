using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using FastMember;
using ObjectiveHarmonyPatch;
using SlimDX.DirectSound;
using SlimDX.Multimedia;
using TypeWrapping;
using UnembeddedResources;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.SoundFactory
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(ISoundFactory))]
    internal class SoundFactory : AssemblyPluginBase, ISoundFactory
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<SoundFactory>("CoreExtensions");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> NotAvailable { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static SoundFactory()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


        private readonly HarmonyPatch Patch;

        private TimeManager TimeManager = null;
        private CameraLocation CameraLocation = null;

        public override string Title { get; } = nameof(SoundFactory);
        public override string Description { get; } = "プラグインから音声を簡単に読み込めるようにします。";

        public bool IsAvailable { get; private set; } = false;

        public SoundFactory(PluginBuilder builder) : base(builder)
        {
            ClassMemberSet vehicleMembers = BveHacker.BveTypes.GetClassInfoOf<Vehicle>();
            FastConstructor constructor = vehicleMembers.GetSourceConstructor();
            Patch = HarmonyPatch.Patch(nameof(SoundFactory), constructor.Source, PatchType.Postfix);
            Patch.Invoked += (sender, e) =>
            {
                TimeManager = TimeManager.FromSource(e.Args[3]);
                CameraLocation = Vehicle.FromSource(e.Instance).CameraLocation;
                IsAvailable = true;

                return PatchInvokationResult.DoNothing(e);
            };

            BveHacker.ScenarioClosed += OnScenarioClosed;
        }

        public override void Dispose()
        {
            Patch.Dispose();

            BveHacker.ScenarioClosed -= OnScenarioClosed;
        }

        private void OnScenarioClosed(EventArgs e)
        {
            TimeManager = null;
            CameraLocation = null;
            IsAvailable = false;
        }

        public Sound LoadFrom(string path, double minRadius, Sound.SoundPosition position, int bufferCount)
        {
            if (!IsAvailable) throw new InvalidOperationException(string.Format(Resources.Value.NotAvailable.Value, Title));

            DirectSound device = BveHacker.MainForm.DirectSound;

            SecondarySoundBuffer[] buffers = new SecondarySoundBuffer[bufferCount];
            using (WaveStream ws = new WaveStream(path))
            {
                SoundBufferDescription description = new SoundBufferDescription()
                {
                    Flags = BufferFlags.ControlFrequency | BufferFlags.ControlVolume | BufferFlags.ControlPan,
                    Format = ws.Format,
                    SizeInBytes = (int)ws.Length,
                };

                byte[] source = new byte[description.SizeInBytes];
                ws.Read(source, 0, description.SizeInBytes);

                for (int i = 0; i < bufferCount; i++)
                {
                    SecondarySoundBuffer buffer = new SecondarySoundBuffer(device, description);
                    buffer.Write(source, 0, LockFlags.None);

                    buffers[i] = buffer;
                }
            }

            Sound sound = new Sound(TimeManager, CameraLocation, buffers, minRadius, position)
            {
                MaxFrequency = device.Capabilities.MaxSecondarySampleRate,
                MinFrequency = device.Capabilities.MinSecondarySampleRate,
            };
            return sound;
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
