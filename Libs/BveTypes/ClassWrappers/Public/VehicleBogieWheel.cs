using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 台車の車輪 (軌道接触部) を表します。
    /// </summary>
    public class VehicleBogieWheel : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleBogieWheel>();

            PositionInBlockField = members.GetSourceFieldOf(nameof(PositionInBlock));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleBogieWheel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleBogieWheel(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleBogieWheel"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleBogieWheel FromSource(object src) => src is null ? null : new VehicleBogieWheel(src);

        private static FastField PositionInBlockField;
        /// <summary>
        /// 現在自列車が走行しているマップ ブロックの原点に対する、この車輪の位置ベクトル [m] を取得・設定します。
        /// </summary>
        public Vector3 PositionInBlock
        {
            get => PositionInBlockField.GetValue(Src);
            set => PositionInBlockField.SetValue(Src, value);
        }
    }
}
