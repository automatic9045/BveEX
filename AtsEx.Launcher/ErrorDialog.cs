using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    internal static class ErrorDialog
    {
        public static void Show(int id, string message, string approach)
        {
            Diagnostics.ErrorDialog.Show($"エラーコード L-{id}\n{message}", Resources.Name, approach, $"https://www.okaoka-depot.com/AtsEX.Docs/support/errors/#L-{id}");
        }
    }
}
