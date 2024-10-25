using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX.DirectSound;
using SlimDX.Multimedia;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.SoundFactory
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(ISoundFactory))]
    internal class SoundFactory : AssemblyPluginBase, ISoundFactory
    {
        private Scenario Scenario;

        public override string Title { get; } = nameof(SoundFactory);
        public override string Description { get; } = "プラグインから音声を簡単に読み込めるようにします。";

        public SoundFactory(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
            BveHacker.ScenarioClosed += OnScenarioClosed;
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
            BveHacker.ScenarioClosed -= OnScenarioClosed;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            Scenario = e.Scenario;
        }

        private void OnScenarioClosed(EventArgs e)
        {
            Scenario = null;
        }

        public Sound LoadFrom(string path, double minRadius, Sound.SoundPosition position, int bufferCount)
        {
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

            Sound sound = new Sound(Scenario.TimeManager, Scenario.Vehicle.CameraLocation, buffers, minRadius, position)
            {
                MaxFrequency = device.Capabilities.MaxSecondarySampleRate,
                MinFrequency = device.Capabilities.MinSecondarySampleRate,
            };
            return sound;
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            return new ExtensionTickResult();
        }
    }
}
