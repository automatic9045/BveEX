using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Scripting;

namespace AtsEx.Plugins.Scripting
{
    public class TickGlobals : Globals
    {
        public TimeSpan Elapsed { get; }

        public TickGlobals(Globals source, TimeSpan elapsed) : base(source)
        {
            Elapsed = elapsed;
        }
    }
}
