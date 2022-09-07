﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zbx1425.DXDynamicTexture;

using Automatic9045.AtsEx.PluginHost;
using Automatic9045.AtsEx.PluginHost.ClassWrappers;
using Automatic9045.AtsEx.PluginHost.Plugins;

namespace Automatic9045.MapPlugins.DXDynamicTextureTest
{
    public class DXDynamicTextureTest : AssemblyPluginBase, IDisposable
    {
        private static readonly Random Random = new Random();

        private TextureHandle TextureHandle;
        private GDIHelper GDIHelper;

        public DXDynamicTextureTest(PluginBuilder builder) : base(builder, PluginType.MapPlugin)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public void Dispose()
        {
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            Model targetModel = e.Scenario.Route.StructureModels["dxdt-test"];
            TextureHandle = targetModel.Register("Stop6.png");

            GDIHelper = new GDIHelper(TextureHandle.Width, TextureHandle.Height);
        }

        public override TickResult Tick(TimeSpan elapsed)
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