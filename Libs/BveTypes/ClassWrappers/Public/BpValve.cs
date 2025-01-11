using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ブレーキ管の圧力調整弁を表します。
    /// </summary>
    public class BpValve : Valve
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BpValve>();

            RapidReleaseSpeedField = members.GetSourceFieldOf(nameof(RapidReleaseSpeed));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BpValve"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BpValve(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BpValve"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new BpValve FromSource(object src) => src is null ? null : new BpValve(src);

        private static FastField RapidReleaseSpeedField;
        /// <summary>
        /// 非常ブレーキ時の排気速度 [Pa^(1/2)/s] を取得・設定します。
        /// </summary>
        public double RapidReleaseSpeed
        {
            get => (double)RapidReleaseSpeedField.GetValue(Src);
            set => RapidReleaseSpeedField.SetValue(Src, value);
        }
    }
}
