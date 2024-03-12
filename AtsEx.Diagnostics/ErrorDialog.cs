using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.Diagnostics
{
    /// <summary>
    /// ユーザーに対してエラーが発生したことを知らせるダイアログを表示するためのクラスです。
    /// </summary>
    public static class ErrorDialog
    {
        /// <summary>
        /// エラーダイアログを表示します。
        /// </summary>
        /// <param name="message">エラーの原因などの説明。</param>
        /// <param name="sender">エラーの発生元。<see langword="null"/> を指定すると省略されます。</param>
        /// <param name="approach">エラーを解決する方法の説明。<see langword="null"/> を指定すると省略されます。</param>
        /// <param name="helpLink">エラーの詳細が説明されているコンテンツへのリンク。<see langword="null"/> を指定すると省略されます。</param>
        /// <param name="caption">ダイアログの題名。<see langword="null"/> を指定すると <paramref name="sender"/> の値で代用されます。</param>
        public static void Show(string message, string sender = null, string approach = null, string helpLink = null, string caption = null)
        {
            string senderText = sender is null ? "" : $"\n\n場所:\n{sender}";
            string approachText = approach is null ? "" : $"\n\n対処方法:\n{approach}";

            string text = message + senderText + approachText;
            string captionText = caption ?? sender ?? "AtsEX";

            if (helpLink is null)
            {
                MessageBox.Show(text, captionText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show($"{text}\n\nこのエラーに関する情報を表示しますか?\n('{helpLink}' が開きます)", captionText, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    Process.Start(helpLink);
                }
            }
        }
    }
}
