using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 車輪の回転数から速度・加速度を測定する速度発電機を表します。
    /// </summary>
    public class Tachogenerator : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Tachogenerator>();

            SpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(Speed));

            SetSpeedMethod = members.GetSourceMethodOf(nameof(SetSpeed));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Tachogenerator"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Tachogenerator(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Tachogenerator"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Tachogenerator FromSource(object src) => src is null ? null : new Tachogenerator(src);

        private static FastMethod SpeedGetMethod;
        /// <summary>
        /// 速度 [m/s] を取得します。
        /// </summary>
        /// <remarks>
        /// 速度の変更には <see cref="SetSpeed(double)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="SetSpeed(double)"/>
        public double Speed => (double)SpeedGetMethod.Invoke(Src, null);

        private static FastMethod SetSpeedMethod;
        /// <summary>
        /// 速度を設定します。
        /// </summary>
        /// <param name="speed">速度 [m/s]。</param>
        /// <remarks>
        /// 速度の取得には <see cref="Speed"/> プロパティを使用してください。
        /// </remarks>
        /// <seealso cref="Speed"/>
        public void SetSpeed(double speed) => SetSpeedMethod.Invoke(Src, new object[] { speed });
    }
}
