using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UnembeddedResources;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx
{
    internal partial class PluginListTabPage : TabPage
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginListTabPage>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ColumnFileName { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ColumnName { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ColumnState { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ColumnVersion { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ColumnDescription { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Enabled { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Disabled { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Enable { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Disable { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static PluginListTabPage()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


        private readonly string TabName;
        private readonly bool ShowStateColumn;

        private readonly ListView PluginList;

        public PluginListTabPage(string tabName, bool showStateColumn) : base()
        {
            TabName = tabName;
            ShowStateColumn = showStateColumn;

            Text = TabName + " (0)";

            PluginList = new ListView()
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                GridLines = true,
                ShowItemToolTips = true,
            };
            PluginList.Columns.Add(Resources.Value.ColumnFileName.Value, 128);
            PluginList.Columns.Add(Resources.Value.ColumnName.Value, 192);
            PluginList.Columns.Add(Resources.Value.ColumnVersion.Value, 96);
            if (ShowStateColumn) PluginList.Columns.Add(Resources.Value.ColumnState.Value, 64);
            PluginList.Columns.Add(Resources.Value.ColumnDescription.Value, 256 + (showStateColumn ? 0 : 64));
            Controls.Add(PluginList);

            if (ShowStateColumn)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripItem enable = contextMenu.Items.Add(Resources.Value.Enable.Value);
                ToolStripItem disable = contextMenu.Items.Add(Resources.Value.Disable.Value);

                enable.Click += (sender, e) => ToggleSelectedPlugins(true);
                disable.Click += (sender, e) => ToggleSelectedPlugins(false);

                contextMenu.Opening += (sender, e) =>
                {
                    e.Cancel = true;

                    if (PluginList.SelectedItems.Count == 0) return;

                    bool haveEnabledPlugins = false;
                    bool haveDisabledPlugins = false;

                    foreach (PluginListItem item in PluginList.SelectedItems)
                    {
                        if (!item.CanToggle) continue;

                        e.Cancel = false;

                        haveEnabledPlugins |= item.IsPluginEnabled;
                        haveDisabledPlugins |= !item.IsPluginEnabled;
                    }

                    enable.Enabled = haveDisabledPlugins;
                    disable.Enabled = haveEnabledPlugins;
                };

                PluginList.ContextMenuStrip = contextMenu;


                void ToggleSelectedPlugins(bool isEnabled)
                {
                    foreach (PluginListItem item in PluginList.SelectedItems)
                    {
                        item.IsPluginEnabled = isEnabled;
                    }
                }
            }
        }

        public void SetPluginDetails(IEnumerable<PluginBase> plugins)
        {
            ListViewItem[] listViewItems = plugins.Select(plugin => new PluginListItem(plugin, ShowStateColumn)).ToArray();

            PluginList.Items.Clear();
            PluginList.Items.AddRange(listViewItems);

            Text = $"{TabName} ({listViewItems.Length})";
        }
    }
}
