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
    /// 自列車の軌道形状を計算します。
    /// </summary>
    public class MyTrackAlignment : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MyTrackAlignment>();

            CurvatureField = members.GetSourceFieldOf(nameof(Curvature));
            GradientField = members.GetSourceFieldOf(nameof(Gradient));

            GetCarCenterGradientMethod = members.GetSourceMethodOf(nameof(GetCarCenterGradient));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MyTrackAlignment"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MyTrackAlignment(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MyTrackAlignment"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MyTrackAlignment FromSource(object src) => src is null ? null : new MyTrackAlignment(src);

        private static FastField CurvatureField;
        /// <summary>
        /// 曲率 [/m] を取得・設定します。
        /// </summary>
        public double Curvature
        {
            get => (double)CurvatureField.GetValue(Src);
            set => CurvatureField.SetValue(Src, value);
        }

        private static FastField GradientField;
        /// <summary>
        /// 勾配を取得・設定します。
        /// </summary>
        public double Gradient
        {
            get => (double)GradientField.GetValue(Src);
            set => GradientField.SetValue(Src, value);
        }

        private static FastMethod GetCarCenterGradientMethod;
        /// <summary>
        /// 自列車の車体中心における勾配を取得します。
        /// </summary>
        public double GetCarCenterGradient() => (double)GetCarCenterGradientMethod.Invoke(Src, null);
    }
}
