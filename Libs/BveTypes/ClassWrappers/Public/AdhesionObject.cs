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
    /// 車輪 - レール間の粘着特性の設定を表します。
    /// </summary>
    public class AdhesionObject : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<AdhesionObject>();

            Constructor1 = members.GetSourceConstructor(new Type[] { typeof(double) });
            Constructor2 = members.GetSourceConstructor(new Type[] { typeof(double), typeof(double), typeof(double), typeof(double) });

            AGetMethod = members.GetSourcePropertyGetterOf(nameof(A));
            ASetMethod = members.GetSourcePropertySetterOf(nameof(A));

            BGetMethod = members.GetSourcePropertyGetterOf(nameof(B));
            BSetMethod = members.GetSourcePropertySetterOf(nameof(B));

            CGetMethod = members.GetSourcePropertyGetterOf(nameof(C));
            CSetMethod = members.GetSourcePropertySetterOf(nameof(C));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AdhesionObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected AdhesionObject(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="AdhesionObject"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static AdhesionObject FromSource(object src) => src is null ? null : new AdhesionObject(src);

        private static FastConstructor Constructor1;
        /// <summary>
        /// 各係数を既定値に設定して、<see cref="AdhesionObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        public AdhesionObject(double location)
            : this(Constructor1.Invoke(new object[] { location }))
        {
        }

        private static FastConstructor Constructor2;
        /// <summary>
        /// <see cref="AdhesionObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="a">静止時における粘着係数。</param>
        /// <param name="b">粘着係数の走行速度に対する変化を表す分子側の係数 [s/m]。</param>
        /// <param name="c">粘着係数の走行速度に対する変化を表す分母側の係数 [s/m]。</param>
        public AdhesionObject(double location, double a, double b, double c)
            : this(Constructor2.Invoke(new object[] { location, a, b, c }))
        {
        }

        private static FastMethod AGetMethod;
        private static FastMethod ASetMethod;
        /// <summary>
        /// 静止時における粘着係数を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定値は 0.32 です。
        /// </remarks>
        public double A
        {
            get => (double)AGetMethod.Invoke(Src, null);
            set => ASetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BGetMethod;
        private static FastMethod BSetMethod;
        /// <summary>
        /// 粘着係数の走行速度に対する変化を表す分子側の係数 [s/m] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定値は 0 です。
        /// </remarks>
        public double B
        {
            get => (double)BGetMethod.Invoke(Src, null);
            set => BSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CGetMethod;
        private static FastMethod CSetMethod;
        /// <summary>
        /// 粘着係数の走行速度に対する変化を表す分母側の係数 [s/m] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定値は 3.6 / 85 です。
        /// </remarks>
        public double C
        {
            get => (double)CGetMethod.Invoke(Src, null);
            set => CSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
