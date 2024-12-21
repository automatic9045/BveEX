using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launching
{
    internal class LaunchModeException : NotSupportedException
    {
        public LaunchModeException() : base("処理を継続するには通常モードへの移行が必要です。")
        {
        }
    }
}
