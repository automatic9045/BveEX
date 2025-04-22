using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 他軌道の位置を定義する <see cref="TrackPosition"/> オブジェクトのリストを表します。
    /// </summary>
    public class TrackPositionList : MapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TrackPositionList>();

            GetValueAtMethod = members.GetSourceMethodOf(nameof(GetValueAt));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TrackPositionList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TrackPositionList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TrackPositionList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new TrackPositionList FromSource(object src) => src is null ? null : new TrackPositionList((IList)src);

        private static FastMethod GetValueAtMethod;
        /// <summary>
        /// 指定した距離程における補間値を取得します。
        /// </summary>
        /// <param name="location">距離程 [m]。</param>
        public double GetValueAt(double location)
            => (double)GetValueAtMethod.Invoke(Src, new object[] { location });
    }
}
