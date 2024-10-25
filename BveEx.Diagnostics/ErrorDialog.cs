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


        private ErrorDialog(ErrorDialogInfo info)
        {
            InitializeComponent(info);
        }
    }
}
