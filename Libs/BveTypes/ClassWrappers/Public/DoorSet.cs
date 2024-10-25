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
    /// 自列車のドアのセットを表します。
    /// </summary>
    public class DoorSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<DoorSet>();

            StandardCloseTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(StandardCloseTime));
            StandardCloseTimeSetMethod = members.GetSourcePropertySetterOf(nameof(StandardCloseTime));

            AreAllClosedGetMethod = members.GetSourcePropertyGetterOf(nameof(AreAllClosed));

            GetSideMethod = members.GetSourceMethodOf(nameof(GetSide));
            SetCarLengthMethod = members.GetSourceMethodOf(nameof(SetCarLength));
            SetStateMethod = members.GetSourceMethodOf(nameof(SetState));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="DoorSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected DoorSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="DoorSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static DoorSet FromSource(object src) => src is null ? null : new DoorSet(src);

        private static FastMethod StandardCloseTimeGetMethod;
        private static FastMethod StandardCloseTimeSetMethod;
        /// <summary>
        /// この側のドアが閉まるのに要する時間の基準 [ms] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>標準では、これをランダムに 0.95 ～ 1.05 倍した値が各ドア (<see cref="SideDoorSet.CarDoors"/> プロパティの各要素) の <see cref="CarDoor.CloseTime"/> プロパティに設定されます。</item>
        /// <item>
        /// 取得の場合、厳密には左側のドアの <see cref="SideDoorSet.StandardCloseTime"/> プロパティの値が返されます。
        /// 標準ではドアの側によって値が異なることはありませんが、BveEX プラグインから手動で変更している場合は注意してください。
        /// </item>
        /// </list>
        /// </remarks>
        public int StandardCloseTime
        {
            get => StandardCloseTimeGetMethod.Invoke(Src, null);
            set => StandardCloseTimeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod AreAllClosedGetMethod;
        /// <summary>
        /// 全てのドアが閉まり切っているかどうかを取得します。
        /// </summary>
        public bool AreAllClosed => AreAllClosedGetMethod.Invoke(Src, null);

        /// <summary>
        /// 互換性のために残されている旧名のプロパティです。<see cref="AreAllClosed"/> を使用してください。
        /// </summary>
        [Obsolete]
        public bool AreAllClosingOrClosed => AreAllClosed;

        private static FastMethod GetSideMethod;
        private SideDoorSet GetSide(int doorSide) => SideDoorSet.FromSource(GetSideMethod.Invoke(Src, new object[] { doorSide }));

        /// <summary>
        /// 指定した側のドアの一覧を取得します。
        /// </summary>
        /// <returns><paramref name="doorSide"/> 側のドアの一覧を表す <see cref="SideDoorSet"/>。</returns>
        /// <param name="doorSide">指定するドアの側。</param>
        public SideDoorSet GetSide(DoorSide doorSide) => GetSide((int)doorSide);

        private static FastMethod SetCarLengthMethod;
        /// <summary>
        /// 両数を指定して両側の <see cref="SideDoorSet.CarDoors"/> プロパティを初期化します。
        /// </summary>
        /// <param name="length">両数。</param>
        public void SetCarLength(int length) => SetCarLengthMethod.Invoke(Src, new object[] { length });

        private static FastMethod SetStateMethod;
        /// <summary>
        /// 全てのドアの開閉状態を変更します。
        /// </summary>
        /// <remarks>
        /// このメソッドは標準では駅ジャンプ時などに使用されており、実行すると開閉時の演出なしで状態が変更されます。
        /// 演出を伴った開閉状態の変更を行いたい場合は <see cref="SideDoorSet.OpenDoors"/> メソッド、<see cref="SideDoorSet.CloseDoors(int)"/> メソッドを使用してください。
        /// </remarks>
        /// <param name="left">指定する左側のドアの開閉状態。</param>
        /// <param name="right">指定する右側のドアの開閉状態。</param>
        /// <seealso cref="SideDoorSet.OpenDoors"/>
        /// <seealso cref="SideDoorSet.CloseDoors(int)"/>
        public void SetState(DoorState left, DoorState right) => SetStateMethod.Invoke(Src, new object[] { left, right });
    }
}
