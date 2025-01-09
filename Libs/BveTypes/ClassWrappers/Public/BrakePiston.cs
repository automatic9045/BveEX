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
    /// 基礎ブレーキ装置のピストンを表します。
    /// </summary>
    public class BrakePiston : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakePiston>();

            AreaGetMethod = members.GetSourcePropertyGetterOf(nameof(Area));
            AreaSetMethod = members.GetSourcePropertySetterOf(nameof(Area));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BrakePiston"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakePiston(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BrakePiston"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BrakePiston FromSource(object src) => src is null ? null : new BrakePiston(src);

        private static FastMethod AreaGetMethod;
        private static FastMethod AreaSetMethod;
        /// <summary>
        /// てこ比を 1、機械的損失を 0 としたときの 1 両あたりのシリンダ受圧面積 [m^2] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 必要に応じて <see cref="AirSupplement.PistonArea"/> プロパティも設定してください。
        /// </remarks>
        /// <seealso cref="AirSupplement.PistonArea"/>
        public double Area
        {
            get => (double)AreaGetMethod.Invoke(Src, null);
            set => AreaSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
