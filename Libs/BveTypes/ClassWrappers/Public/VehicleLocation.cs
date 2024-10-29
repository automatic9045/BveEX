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
    /// 自列車の位置情報を表します。
    /// </summary>
    public class VehicleLocation : Tachogenerator
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleLocation>();

            LocationGetMethod = members.GetSourcePropertyGetterOf(nameof(Location));

            BlockIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(BlockIndex));

            SetLocationMethod = members.GetSourceMethodOf(nameof(SetLocation));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleLocation"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleLocation(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleLocation"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new VehicleLocation FromSource(object src) => src is null ? null : new VehicleLocation(src);

        private static FastMethod LocationGetMethod;
        /// <summary>
        /// 自車両の位置を取得します。
        /// </summary>
        /// <remarks>
        /// 自車両の位置を設定するには <see cref="SetLocation(double, bool)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="SetLocation(double, bool)"/>
        public double Location => LocationGetMethod.Invoke(Src, null);

        private static FastMethod BlockIndexGetMethod;
        /// <summary>
        /// 現在の自車両の位置が含まれるストラクチャー描画ブロックのインデックスを取得します。
        /// </summary>
        /// <remarks>
        /// 25m 毎に 1 つの描画ブロックが定義されており、全てのストラクチャーは設置された距離程が含まれるブロックに登録されます。<br/>
        /// 各ストラクチャーが描画距離内に入っているかどうかを描画ブロック単位で判定することで、ストラクチャーの描画処理を高速化しています。
        /// </remarks>
        public int BlockIndex => BlockIndexGetMethod.Invoke(Src, null);

        private static FastMethod SetLocationMethod;
        /// <summary>
        /// 自車両の位置を設定します。
        /// </summary>
        /// <param name="location">設定する自車両の位置 [m]。</param>
        /// <param name="skipIfNoChange">指定された位置が現在と変わらない場合、処理をスキップするか。</param>
        /// <remarks>
        /// 自車両の位置を取得するには <see cref="Location"/> プロパティを使用してください。
        /// </remarks>
        /// <seealso cref="Location"/>
        public void SetLocation(double location, bool skipIfNoChange) => SetLocationMethod.Invoke(Src, new object[] { location, skipIfNoChange });
    }
}
