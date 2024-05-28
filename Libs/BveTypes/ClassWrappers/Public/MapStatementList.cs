using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers.Extensions;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// <see cref="MapStatement"/> のリストを表します。
    /// </summary>
    public class MapStatementList : WrappedList<MapStatement>
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapStatementList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapStatementList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapStatementList(object src) : base((IList)src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapStatementList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapStatementList FromSource(object src) => src is null ? null : new MapStatementList(src);
    }
}
