using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx
{
    internal static class ErrorDialog
    {
        public static void Show(string message)
            => Diagnostics.ErrorDialog.Show(message, "AtsEX");
    }
}
