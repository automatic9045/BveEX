using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.MapStatements
{
    /// <summary>
    /// ユーザーが独自に定義したステートメントの一覧を表します。
    /// </summary>
    /// <remarks>
    /// 互換性のために残されている古い機能です。AtsEx.CoreExtensions のマップステートメント機能をご利用ください。
    /// </remarks>
    [Obsolete]
    public interface IStatementSet
    {
        /// <summary>
        /// 指定した識別子 (名前) を持つステートメントの一覧を取得します。
        /// </summary>
        /// <param name="identifier">ステートメントの識別子。</param>
        /// <returns>識別子 <paramref name="identifier"/> を持つステートメントの一覧。</returns>
        IReadOnlyList<IStatement> GetAll(Identifier identifier);
    }
}
