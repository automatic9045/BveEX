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
    /// 自列車の 1 車両のドアを表します。
    /// </summary>
    /// <remarks>
    /// ドアを車両単位で定義することで、車両ごとのドアが閉まるタイミングの誤差を表現しています。
    /// </remarks>
    public class CarDoor : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CarDoor>();

            CloseTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(CloseTime));
            CloseTimeSetMethod = members.GetSourcePropertySetterOf(nameof(CloseTime));

            IsOpenGetMethod = members.GetSourcePropertyGetterOf(nameof(IsOpen));

            TimeLeftToCompleteClosingField = members.GetSourceFieldOf(nameof(TimeLeftToCompleteClosing));
            StateField = members.GetSourceFieldOf(nameof(State));

            SetStateMethod = members.GetSourceMethodOf(nameof(SetState));
            OpenMethod = members.GetSourceMethodOf(nameof(Open));
            CloseMethod = members.GetSourceMethodOf(nameof(Close));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CarDoor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CarDoor(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CarDoor"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static CarDoor FromSource(object src) => src is null ? null : new CarDoor(src);

        private static FastMethod CloseTimeGetMethod;
        private static FastMethod CloseTimeSetMethod;
        /// <summary>
        /// このドアが閉まるのに要する時間 [ms] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 標準では、<see cref="SideDoorSet.StandardCloseTime"/> をランダムに 0.95 ～ 1.05 倍した値が設定されています。
        /// </remarks>
        /// <seealso cref="SideDoorSet.StandardCloseTime"/>
        /// <seealso cref="DoorSet.StandardCloseTime"/>
        public int CloseTime
        {
            get => (int)CloseTimeGetMethod.Invoke(Src, null);
            set => CloseTimeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod IsOpenGetMethod;
        /// <summary>
        /// このドアが開いているかどうかを取得します。
        /// </summary>
        public bool IsOpen => (bool)IsOpenGetMethod.Invoke(Src, null);

        private static FastField TimeLeftToCompleteClosingField;
        /// <summary>
        /// このドアが [ms] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <seealso cref="CloseTime"/>
        public int TimeLeftToCompleteClosing
        {
            get => (int)TimeLeftToCompleteClosingField.GetValue(Src);
            set => TimeLeftToCompleteClosingField.SetValue(Src, value);
        }

        private static FastField StateField;
        /// <summary>
        /// このドアの開閉状態を取得します。
        /// </summary>
        /// <remarks>
        /// 開閉状態の変更には <see cref="SetState(DoorState)"/> メソッド、<see cref="Open"/> メソッド、<see cref="Close(int)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="CloseTime"/>
        /// <seealso cref="SetState(DoorState)"/>
        /// <seealso cref="Open"/>
        /// <seealso cref="Close(int)"/>
        public DoorState State => (DoorState)StateField.GetValue(Src);

        private static FastMethod SetStateMethod;
        /// <summary>
        /// このドアの開閉状態を変更します。
        /// </summary>
        /// <param name="state">指定する開閉状態。</param>
        /// <remarks>
        /// 開閉状態の取得には <see cref="State"/> プロパティを使用してください。<br/>
        /// このメソッドを直接実行すると、本来開閉状態を変更した時に発生する一部のイベントが発生しません。通常は <see cref="DoorSet.SetState(DoorState, DoorState)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="State"/>
        /// <seealso cref="DoorSet.SetState(DoorState, DoorState)"/>
        /// <seealso cref="Open"/>
        /// <seealso cref="Close(int)"/>
        public void SetState(DoorState state) => SetStateMethod.Invoke(Src, new object[] { state });

        private static FastMethod OpenMethod;
        /// <summary>
        /// このドアを開けます。
        /// </summary>
        /// <remarks>
        /// 開閉時の演出を伴わずに状態を変更したい場合は <see cref="DoorSet.SetState(DoorState, DoorState)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="DoorSet.SetState(DoorState, DoorState)"/>
        /// <seealso cref="Close(int)"/>
        public void Open() => OpenMethod.Invoke(Src, new object[] { });

        private static FastMethod CloseMethod;
        /// <summary>
        /// このドアを閉めます。
        /// </summary>
        /// <remarks>
        /// 開閉時の演出を伴わずに状態を変更したい場合は <see cref="DoorSet.SetState(DoorState, DoorState)"/> メソッドを使用してください。
        /// </remarks>
        /// <param name="stuckTime">旅客がこのドアに挟まる時間 [ms]。</param>
        /// <seealso cref="DoorSet.SetState(DoorState, DoorState)"/>
        /// <seealso cref="Open"/>
        public void Close(int stuckTime) => CloseMethod.Invoke(Src, new object[] { stuckTime });
    }
}
