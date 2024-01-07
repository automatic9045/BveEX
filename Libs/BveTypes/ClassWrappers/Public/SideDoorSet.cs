using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の特定側のドアのセットを表します。
    /// </summary>
    public class SideDoorSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SideDoorSet>();

            StandardCloseTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(StandardCloseTime));
            StandardCloseTimeSetMethod = members.GetSourcePropertySetterOf(nameof(StandardCloseTime));

            CarDoorsGetMethod = members.GetSourcePropertyGetterOf(nameof(CarDoors));

            IsOpenGetMethod = members.GetSourcePropertyGetterOf(nameof(IsOpen));

            SetCarLengthMethod = members.GetSourceMethodOf(nameof(SetCarLength));
            SetStateMethod = members.GetSourceMethodOf(nameof(SetState));
            OpenDoorsMethod = members.GetSourceMethodOf(nameof(OpenDoors));
            CloseDoorsMethod = members.GetSourceMethodOf(nameof(CloseDoors));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SideDoorSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SideDoorSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SideDoorSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static SideDoorSet FromSource(object src) => src is null ? null : new SideDoorSet(src);

        private static FastMethod StandardCloseTimeGetMethod;
        private static FastMethod StandardCloseTimeSetMethod;
        /// <summary>
        /// この側のドアが閉まるのに要する時間の基準 [ms] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 標準では、これをランダムに 0.95 ～ 1.05 倍した値が各ドア (<see cref="CarDoors"/> プロパティの各要素) の <see cref="CarDoor.CloseTime"/> プロパティに設定されます。
        /// </remarks>
        public int StandardCloseTime
        {
            get => StandardCloseTimeGetMethod.Invoke(Src, null);
            set => StandardCloseTimeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CarDoorsGetMethod;
        /// <summary>
        /// 車両単位のドアの一覧を取得します。
        /// </summary>
        public WrappedList<CarDoor> CarDoors => WrappedList<CarDoor>.FromSource(CarDoorsGetMethod.Invoke(Src, null));

        private static FastMethod IsOpenGetMethod;
        /// <summary>
        /// この側のドアのうち、ひとつでも開いているものがあるかどうかを取得します。
        /// </summary>
        /// <seealso cref="DoorSet.AreAllClosed"/>
        public bool IsOpen => IsOpenGetMethod.Invoke(Src, null);

        private static FastMethod SetCarLengthMethod;
        /// <summary>
        /// 両数を指定して <see cref="CarDoors"/> プロパティを初期化します。
        /// </summary>
        /// <param name="length">両数。</param>
        /// <seealso cref="DoorSet.SetCarLength(int)"/>
        public void SetCarLength(int length) => SetCarLengthMethod.Invoke(Src, new object[] { length });

        private static FastMethod SetStateMethod;
        /// <summary>
        /// この側の全てのドアの開閉状態を変更します。
        /// </summary>
        /// <param name="state">指定する開閉状態。</param>
        /// <remarks>
        /// このメソッドを直接実行すると、本来開閉状態を変更した時に発生する一部のイベントが発生しません。通常は <see cref="DoorSet.SetState(DoorState, DoorState)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="DoorSet.SetState(DoorState, DoorState)"/>
        /// <seealso cref="OpenDoors"/>
        /// <seealso cref="CloseDoors(int)"/>
        public void SetState(DoorState state) => SetStateMethod.Invoke(Src, new object[] { state });

        private static FastMethod OpenDoorsMethod;
        /// <summary>
        /// この側の全てのドアを開けます。
        /// </summary>
        /// <remarks>
        /// 開閉時の演出を伴わずに状態を変更したい場合は <see cref="DoorSet.SetState(DoorState, DoorState)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="DoorSet.SetState(DoorState, DoorState)"/>
        /// <seealso cref="CloseDoors(int)"/>
        public void OpenDoors() => OpenDoorsMethod.Invoke(Src, new object[] { });

        private static FastMethod CloseDoorsMethod;
        /// <summary>
        /// この側の全てのドアを閉めます。
        /// </summary>
        /// <remarks>
        /// 開閉時の演出を伴わずに状態を変更したい場合は <see cref="DoorSet.SetState(DoorState, DoorState)"/> メソッドを使用してください。
        /// </remarks>
        /// <param name="maxStuckTime">
        /// 旅客がこの側のドアに挟まる時間の最大値 [ms]。<br/>
        /// 実際に使用される値は、この値に 0 以上 1 未満の同じ乱数を 2 回掛けたものです。乱数は各ドア (<see cref="CarDoors"/> プロパティの各要素) ごとに計算されます。
        /// </param>
        /// <seealso cref="DoorSet.SetState(DoorState, DoorState)"/>
        /// <seealso cref="OpenDoors"/>
        public void CloseDoors(int maxStuckTime) => CloseDoorsMethod.Invoke(Src, new object[] { maxStuckTime });
    }
}
