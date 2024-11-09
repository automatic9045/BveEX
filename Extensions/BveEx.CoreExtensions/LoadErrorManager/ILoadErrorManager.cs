using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Plugins.Extensions;
using BveTypes.ClassWrappers;

namespace BveEx.Extensions.LoadErrorManager
{
    /// <summary>
    /// シナリオ読込時のエラーを編集するための機能を提供します。
    /// </summary>
    public interface ILoadErrorManager : IExtension
    {
        /// <summary>
        /// エラーの一覧を取得します。
        /// </summary>
        IList<LoadError> Errors { get; }

        /// <summary>
        /// エラーをスローします。
        /// </summary>
        /// <param name="text">エラーの内容を表すテキスト。</param>
        /// <param name="senderFileName">エラーの発生元となるファイルのファイル名。</param>
        /// <param name="lineIndex">エラーの発生元となる行番号。</param>
        /// <param name="charIndex">エラーの発生元となる列番号。</param>
        void Throw(string text, string senderFileName, int lineIndex, int charIndex);

        /// <summary>
        /// エラーをスローします。
        /// </summary>
        /// <param name="text">エラーの内容を表すテキスト。</param>
        /// <param name="senderFileName">エラーの発生元となるファイルのファイル名。</param>
        /// <param name="lineIndex">エラーの発生元となる行番号。</param>
        void Throw(string text, string senderFileName, int lineIndex);

        /// <summary>
        /// エラーをスローします。
        /// </summary>
        /// <param name="text">エラーの内容を表すテキスト。</param>
        /// <param name="senderFileName">エラーの発生元となるファイルのファイル名。</param>
        void Throw(string text, string senderFileName);

        /// <summary>
        /// エラーをスローします。
        /// </summary>
        /// <param name="text">エラーの内容を表すテキスト。</param>
        void Throw(string text);

        /// <summary>
        /// エラーをスローします。
        /// </summary>
        /// <param name="error">スローするエラー。</param>
        void Throw(LoadError error);
    }
}
