using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins;

namespace BveEx.Plugins.Native
{
    internal class NativePluginBuilder : PluginBuilder
    {
        public string LibraryPath { get; set; }

        public NativePluginBuilder(PluginBuilder source) : base(source)
        {
        }
    }
}
