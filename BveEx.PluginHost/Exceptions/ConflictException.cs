using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// 他のプラグインとの競合を検出した場合にスローされます。
    /// </summary>
    public class ConflictException : Exception
    {
        /// <summary>
        /// <see cref="ConflictException"/> の原因となった箇所を表すテキストを取得します。
        /// </summary>
        public string SenderName { get; } = null;

        /// <summary>
        /// 例外を説明するメッセージを指定して、<see cref="ConflictException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外がスローされたときに表示するメッセージ。</param>
        public ConflictException(string message) : base(message)
        {
        }

        /// <summary>
        /// 例外を説明するメッセージとこの例外の原因となった箇所を表すテキストを指定して、<see cref="ConflictException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外がスローされたときに表示するメッセージ。<see langword="null"/> を指定すると既定のメッセージが使用されます。</param>
        /// <param name="senderName"><see cref="ConflictException"/> の原因となった箇所を表すテキスト。</param>
        public ConflictException(string message, string senderName) : base(message)
        {
            SenderName = senderName;
        }

        /// <summary>
        /// 例外を説明するメッセージとこの例外の原因となった例外を指定して、<see cref="ConflictException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外がスローされたときに表示するメッセージ。</param>
        /// <param name="innerException"><see cref="ConflictException"/> の原因となった例外。</param>
        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
