using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の通過を検知可能な <see cref="MapObjectBase"/> のリストを表します。
    /// </summary>
    public class MapFunctionList : MapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapFunctionList>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapFunctionList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapFunctionList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapFunctionList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new MapFunctionList FromSource(object src) => src is null ? null : new MapFunctionList((IList)src);
    }
}
