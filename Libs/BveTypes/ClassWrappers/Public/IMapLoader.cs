using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// マップを読み込むための機能を表します。
    /// </summary>
    public interface IMapLoader
    {
        /// <summary>
        /// 他のマップファイルの内容を挿入します。
        /// </summary>
        /// <param name="filePath">挿入するマップファイルのパス。</param>
        void Include(string filePath);

        /// <summary>
        /// マップ読込中に発生したエラーをエラー一覧に追加します。
        /// </summary>
        /// <param name="error">追加するエラー。</param>
        void ThrowError(LoadError error);
    }
}
