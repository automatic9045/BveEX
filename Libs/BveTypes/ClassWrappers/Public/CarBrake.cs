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
    /// 車両のブレーキを表します。
    /// </summary>
    public class CarBrake : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CarBrake>();

            BasicBrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(BasicBrake));
            BrakeReAdhesionGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeReAdhesion));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CarBrake"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CarBrake(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CarBrake"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static CarBrake FromSource(object src) => src is null ? null : new CarBrake(src);

        private static FastMethod BasicBrakeGetMethod;
        /// <summary>
        /// 基礎ブレーキ装置を表す <see cref="ClassWrappers.BasicBrake"/> を取得します。
        /// </summary>
        public BasicBrake BasicBrake => ClassWrappers.BasicBrake.FromSource(BasicBrakeGetMethod.Invoke(Src, null));

        private static FastMethod BrakeReAdhesionGetMethod;
        /// <summary>
        /// 基礎ブレーキ装置の滑走再粘着制御機構を取得します。
        /// </summary>
        public ReAdhesionControl BrakeReAdhesion => ReAdhesionControl.FromSource(BrakeReAdhesionGetMethod.Invoke(Src, null));
    }
}
