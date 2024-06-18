using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UnembeddedResources;

using AtsEx.PluginHost.Plugins;

namespace AtsEx
{
    internal partial class VersionForm : Form
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<VersionForm>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Caption { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Description { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> License { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Website { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Repository { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginListHeader { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> OK { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static VersionForm()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }


        public VersionForm()
        {
            InitializeComponent();
        }

        public void SetPluginDetails(PluginType pluginType, IEnumerable<PluginBase> plugins)
        {
            PluginListPages[(int)pluginType].SetPluginDetails(plugins);
        }
    }
}
