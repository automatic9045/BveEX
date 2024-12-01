using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UnembeddedResources;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx
{
    internal partial class PluginListTabPage
    {
        private class PluginListItem : ListViewItem
        {
            private readonly PluginBase Plugin;
            private readonly bool ShowStateColumn;

            private readonly ITogglableExtension TogglableExtension;
            private readonly ListViewSubItem IsEnabledItem;

            public bool CanToggle { get; }

            public bool IsPluginEnabled
            {
                get => !CanToggle || TogglableExtension.IsEnabled;
                set
                {
                    if (!CanToggle) return;

                    TogglableExtension.IsEnabled = value;
                    UpdateStyle();
                }
            }

            public PluginListItem(PluginBase plugin, bool showStateColumn)
            {
                if (plugin is null) throw new ArgumentNullException(nameof(plugin));

                Plugin = plugin;
                ShowStateColumn = showStateColumn;
                TogglableExtension = Plugin as ITogglableExtension;
                CanToggle = !(TogglableExtension is null);

                Text = plugin.Name;
                UseItemStyleForSubItems = false;
                ToolTipText = plugin.Location;

                SubItems.Add(plugin.Title);
                SubItems.Add(plugin.Version);
                if (ShowStateColumn) IsEnabledItem = SubItems.Add(string.Empty);
                SubItems.Add(plugin.Description);

                if (ShowStateColumn) UpdateStyle();
            }

            private void UpdateStyle()
            {
                bool isEnabled = !CanToggle || TogglableExtension.IsEnabled;

                Color foreColor = isEnabled ? SystemColors.WindowText : Color.Gray;
                ForeColor = foreColor;
                foreach (ListViewSubItem item in SubItems)
                {
                    item.ForeColor = foreColor;
                }

                if (CanToggle)
                {
                    Resource<string> stateResource = isEnabled ? Resources.Value.Enabled : Resources.Value.Disabled;
                    IsEnabledItem.Text = stateResource.Value;
                    IsEnabledItem.ForeColor = isEnabled ? ForeColor : Color.Red;
                }
                else
                {
                    IsEnabledItem.Text = "-";
                }
            }
        }
    }
}
