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
    /// 先行列車の通過時刻のリストを表します。
    /// </summary>
    public class PreTrainObjectList : MapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<PreTrainObjectList>();

            GetPreTrainLocationMethod = members.GetSourceMethodOf(nameof(GetPreTrainLocation));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="PreTrainObjectList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected PreTrainObjectList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="PreTrainObjectList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new PreTrainObjectList FromSource(object src) => src is null ? null : new PreTrainObjectList((IList)src);

        private static FastMethod GetPreTrainLocationMethod;
        /// <summary>
        /// 指定した時刻における先行列車の位置 [m] を取得します。
        /// </summary>
        /// <param name="timeMilliseconds">0 時丁度から指定する時刻までに経過したミリ秒数 [ms]。</param>
        /// <returns><paramref name="timeMilliseconds"/> における先行列車の位置 [m]。</returns>
        public double GetPreTrainLocation(int timeMilliseconds) => (double)GetPreTrainLocationMethod.Invoke(Src, new object[] { timeMilliseconds });

        /// <summary>
        /// 指定した時刻における先行列車の位置 [m] を取得します。
        /// </summary>
        /// <param name="time">指定する時刻。</param>
        /// <returns><paramref name="time"/> における先行列車の位置 [m]。</returns>
        public double GetPreTrainLocation(TimeSpan time) => GetPreTrainLocation((int)time.TotalMilliseconds);
    }
}
