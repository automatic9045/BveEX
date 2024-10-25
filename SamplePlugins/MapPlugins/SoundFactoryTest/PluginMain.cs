using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

using BveEx.Extensions.SoundFactory;

namespace BveEx.Samples.MapPlugins.SoundFactoryTest
{
    [Plugin(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private Sound SampleSound;

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
            SampleSound.Dispose();
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            ISoundFactory soundFactory = Extensions.GetExtension<ISoundFactory>();

            string path = Path.Combine(Path.GetDirectoryName(Location), "Sample.wav");
            SampleSound = soundFactory.LoadFrom(path, 1, Sound.SoundPosition.Cab);

            SampleSound.Play(1, 1, 0);
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            return new MapPluginTickResult();
        }
    }
}
