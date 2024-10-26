using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zbx1425.DXDynamicTexture;

using BveTypes.ClassWrappers;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.MapPlugins.DXDynamicTextureTest
{
    [Plugin(PluginType.MapPlugin)]
    public class DXDynamicTextureTest : AssemblyPluginBase
    {
        private static readonly Random Random = new Random();

        private TextureHandle TextureHandle;
        private GDIHelper GDIHelper;

        public DXDynamicTextureTest(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            Model targetModel = e.Scenario.Route.StructureModels["dxdt-test"];
            TextureHandle = targetModel.Register("Stop6.png");

            GDIHelper = new GDIHelper(TextureHandle.Width, TextureHandle.Height);
        }

        public override IPluginTickResult Tick(TimeSpan elapsed)
        {
            if (TextureHandle.HasEnoughTimePassed(10))
            {
                GDIHelper.BeginGDI();
                {
                    Color randomColor = Color.FromArgb(Random.Next(255), Random.Next(255), Random.Next(255));
                    GDIHelper.FillRectWH(randomColor, 0, 0, TextureHandle.Width, TextureHandle.Height);
                }
                GDIHelper.EndGDI();
                TextureHandle.Update(GDIHelper);
            }

            return new MapPluginTickResult();
        }
    }
}
