using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Diagnostics
{
    /// <summary>
    /// <see cref="ErrorDialog"/> の表示内容を指定します。
    /// </summary>
    public class ErrorDialogInfo
    {
        /// <summary>
        /// ダイアログのキャプションを取得・設定します。
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> を指定すると <see cref="Sender"/> の値で代用されます。
        /// </remarks>
        public string Caption { get; set; } = null;

        /// <summary>
        /// ダイアログのヘッダー部分に表示するテキストを取得・設定します。
        /// </summary>
        /// <remarks>
        /// エラーの内容を端的に表すメッセージを指定してください。<see langword="null"/> を指定すると「エラーが発生しました」と表示されます。
        /// </remarks>
        public string Header { get; set; } = null;

        /// <summary>
        /// エラーの発生元を表すテキストを取得・設定します。
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> を指定すると「(指定なし)」と表示されます。
        /// </remarks>
        public string Sender { get; set; } = null;

        /// <summary>
        /// エラーの原因などを説明するテキストを取得・設定します。
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> を指定すると省略されます。
        /// </remarks>
        public string Message { get; set; } = null;

        /// <summary>
        /// エラーを解決する方法を説明するテキストを取得・設定します。
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> を指定すると省略されます。
        /// </remarks>
        public string Approach { get; set; } = null;

        /// <summary>
        /// エラーの詳細が説明されているコンテンツへのリンクを表す URI を取得・設定します。
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> を指定すると省略されます。
        /// </remarks>
        public Uri HelpLink { get; set; } = null;

        /// <summary>
        /// <see cref="ErrorDialogInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="header">ダイアログのヘッダー部分に表示するテキスト。エラーの内容を端的に表すメッセージを指定してください。<see langword="null"/> を指定すると「エラーが発生しました」と表示されます。</param>
        /// <param name="sender">エラーの発生元を表すテキスト。<see langword="null"/> を指定すると「(指定なし)」と表示されます。</param>
        /// <param name="message">エラーの原因などを説明するテキスト。<see langword="null"/> を指定すると省略されます。</param>
        public ErrorDialogInfo(string header, string sender, string message)
        {
            Header = header;
            Sender = sender;
            Message = message;
        }

        internal string GetCaption() => Caption ?? Sender ?? "BveEX";
        internal string GetHeader() => Header ?? "エラーが発生しました";
        internal string GetSender() => Sender ?? "(指定なし)";
    }
}
