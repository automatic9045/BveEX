using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Launcher
{
    internal static class ErrorDialog
    {
        public static void Show(int id, string message, string approach)
        {
            string code = $"L-{id}";

            string header = $"{Resources.Name} でエラーが発生しました ({code})";
            string messageWithCode = $"エラーコード {code}\n{message}";
            Diagnostics.ErrorDialogInfo errorDialogInfo = new Diagnostics.ErrorDialogInfo(header, Resources.Name, messageWithCode)
            {
                Approach = approach,
                HelpLink = new Uri($"https://www.okaoka-depot.com/AtsEX.Docs/support/errors/#{code}"),
            };

            Diagnostics.ErrorDialog.Show(errorDialogInfo);
        }
    }
}
