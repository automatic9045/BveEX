using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Plugins;

namespace AtsEx.Plugins.Native
{
    internal class NativePluginBuilder : PluginBuilder
    {
        public string LibraryPath { get; set; }

        public NativePluginBuilder(PluginBuilder source) : base(source)
        {
        }
    }
}
