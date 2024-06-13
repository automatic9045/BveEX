using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.Diagnostics
{
    /// <summary>
    /// ユーザーに対してエラーが発生したことを知らせるダイアログを表します。
    /// </summary>
    public partial class ErrorDialog : Form
    {
        /// <summary>
        /// エラーダイアログを表示します。
        /// </summary>
        /// <param name="info">ダイアログの表示内容。</param>
        public static void Show(ErrorDialogInfo info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));

            ErrorDialog dialog = new ErrorDialog(info);
            dialog.ShowDialog();
        }

        /// <summary>
        /// エラーダイアログを表示します。
        /// </summary>
        /// <remarks>
        /// 互換性のために残されている古い形式のメソッドです。<see cref="Show(ErrorDialogInfo)"/> メソッドを使用してください。
        /// </remarks>
        /// <param name="message">エラーの原因などの説明。</param>
        /// <param name="sender">エラーの発生元。<see langword="null"/> を指定すると省略されます。</param>
        /// <param name="approach">エラーを解決する方法の説明。<see langword="null"/> を指定すると省略されます。</param>
        /// <param name="helpLink">エラーの詳細が説明されているコンテンツへのリンクを表す URI 文字列。<see langword="null"/> を指定すると省略されます。</param>
        /// <param name="caption">ダイアログのキャプション。<see langword="null"/> を指定すると <paramref name="sender"/> の値で代用されます。</param>
        [Obsolete]
        public static void Show(string message, string sender = null, string approach = null, string helpLink = null, string caption = null)
        {

        }


        private ErrorDialog(ErrorDialogInfo info)
        {
            InitializeComponent(info);
        }
    }
}
