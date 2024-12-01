using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveEx.PluginHost.Plugins;

using VehiclePlugin = BveEx.Samples.VehiclePlugins.CooperatingWithMapPlugin.PluginMain;

namespace BveEx.Samples.MapPlugins.CooperatingWithVehiclePlugin
{
    [Plugin(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        public PluginMain(PluginBuilder builder) : base(builder)
        {
            Plugins.AllPluginsLoaded += OnAllPluginsLoaded;
        }

        private void OnAllPluginsLoaded(object sender, EventArgs e)
        {
            VehiclePlugin vehiclePlugin = Plugins.VehiclePlugins["TestPlugin"] as VehiclePlugin;
            MessageBox.Show($"車両プラグインから値を取得しました: {vehiclePlugin.SharedValue}", "BveEX マッププラグイン：車両プラグイン連携サンプル");
        }

        public override void Dispose()
        {
            Plugins.AllPluginsLoaded -= OnAllPluginsLoaded;
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
