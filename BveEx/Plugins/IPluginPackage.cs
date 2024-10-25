using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Plugins
{
    internal interface IPluginPackage
    {
        Identifier Identifier { get; }
    }
}
