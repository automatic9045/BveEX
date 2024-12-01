using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zbx1425.DXDynamicTexture;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.DXDynamicTexture
{
    [Plugin(PluginType.Extension)]
    [HideExtensionMain]
    public class ExtensionMain : AssemblyPluginBase, IExtension
    {
        public ExtensionMain(PluginBuilder builder) : base(builder)
        {
            TextureManager.Initialize();
        }

        public override void Dispose()
        {
            TextureManager.Clear();
        }

        public override void Tick(TimeSpan elapsed)
        {
        }
    }
}
