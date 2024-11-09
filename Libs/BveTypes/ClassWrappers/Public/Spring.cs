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
    /// ばねのモデルを表します。
    /// </summary>
    public class Spring : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Spring>();

            UpperPositionGetMethod = members.GetSourcePropertyGetterOf(nameof(UpperPosition));
            UpperPositionSetMethod = members.GetSourcePropertySetterOf(nameof(UpperPosition));

            UpperSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(UpperSpeed));
            UpperSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(UpperSpeed));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Spring"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Spring(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Spring"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Spring FromSource(object src) => src is null ? null : new Spring(src);

        private static FastMethod UpperPositionGetMethod;
        private static FastMethod UpperPositionSetMethod;
        /// <summary>
        /// ばね上端の位置 [m] を取得・設定します。
        /// </summary>
        public double UpperPosition
        {
            get => (double)UpperPositionGetMethod.Invoke(Src, null);
            set => UpperPositionSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod UpperSpeedGetMethod;
        private static FastMethod UpperSpeedSetMethod;
        /// <summary>
        /// ばね上端の速度 [m/s] を取得・設定します。
        /// </summary>
        public double UpperSpeed
        {
            get => (double)UpperSpeedGetMethod.Invoke(Src, null);
            set => UpperSpeedSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
