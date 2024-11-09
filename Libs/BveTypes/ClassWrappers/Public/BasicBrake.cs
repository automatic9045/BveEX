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
    /// 基礎ブレーキ装置を表します。
    /// </summary>
    public class BasicBrake : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BasicBrake>();

            PistonAreaGetMethod = members.GetSourcePropertyGetterOf(nameof(PistonArea));
            PistonAreaSetMethod = members.GetSourcePropertySetterOf(nameof(PistonArea));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BasicBrake"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BasicBrake(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BasicBrake"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BasicBrake FromSource(object src) => src is null ? null : new BasicBrake(src);

        private static FastMethod PistonAreaGetMethod;
        private static FastMethod PistonAreaSetMethod;
        /// <summary>
        /// てこ比を 1、機械的損失を 0 としたときの 1 両あたりのシリンダ受圧面積 [m^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 必要に応じて <see cref="AirSupplement.PistonArea"/> プロパティも設定してください。
        /// </remarks>
        /// <seealso cref="AirSupplement.PistonArea"/>
        public double PistonArea
        {
            get => (double)PistonAreaGetMethod.Invoke(Src, null);
            set => PistonAreaSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
