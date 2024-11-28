using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveEx.Extensions.ContextMenuHacker;
using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx.Samples.VehiclePlugins.StateViewer
{
    [Plugin(PluginType.VehiclePlugin)]
    public class StateViewer : AssemblyPluginBase
    {
        private readonly StateForm Form;
        private readonly ToolStripMenuItem MenuItem;

        public StateViewer(PluginBuilder services) : base(services)
        {
            InstanceStore.Initialize(Native, BveHacker);

            MenuItem = Extensions.GetExtension<IContextMenuHacker>().AddCheckableMenuItem("状態ウィンドウを表示", MenuItemCheckedChanged, ContextMenuItemType.Plugins);

            MenuItem.Checked = false;

            Form = new StateForm();
            Form.FormClosing += FormClosing;
            Form.WindowState = FormWindowState.Normal;

            MenuItem.Checked = true;
            BveHacker.MainFormSource.Focus();
        }

        public override void Dispose()
        {
            Form.Close();
            MenuItem.Dispose();
        }

        public override void Tick(TimeSpan elapsed)
        {
            Form?.Tick();
        }

        private void MenuItemCheckedChanged(object sender, EventArgs e)
        {
            if (MenuItem.Checked)
            {
                Form.Show(BveHacker.MainFormSource);
            }
            else
            {
                Form.Hide();
            }
        }

        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            MenuItem.Checked = false;
        }
    }
}
