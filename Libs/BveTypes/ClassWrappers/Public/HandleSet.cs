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
    /// 操作可能なハンドルのセットを表します。
    /// </summary>
    public class HandleSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<HandleSet>();

            NotchInfoGetMethod = members.GetSourcePropertyGetterOf(nameof(NotchInfo));
            NotchInfoSetMethod = members.GetSourcePropertySetterOf(nameof(NotchInfo));

            BrakeNotchGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeNotch));
            BrakeNotchSetMethod = members.GetSourcePropertySetterOf(nameof(BrakeNotch));

            PowerNotchGetMethod = members.GetSourcePropertyGetterOf(nameof(PowerNotch));
            PowerNotchSetMethod = members.GetSourcePropertySetterOf(nameof(PowerNotch));

            ReverserPositionGetMethod = members.GetSourcePropertyGetterOf(nameof(_ReverserPosition));
            ReverserPositionSetMethod = members.GetSourcePropertySetterOf(nameof(_ReverserPosition));

            ConstantSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(_ConstantSpeedMode));
            ConstantSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(_ConstantSpeedMode));

            FastEvent brakeChangedEvent = members.GetSourceEventOf(nameof(BrakeChanged));
            FastEvent powerChangedEvent = members.GetSourceEventOf(nameof(PowerChanged));
            FastEvent reverserChangedEvent = members.GetSourceEventOf(nameof(ReverserChanged));

            BrakeChangedEvent = new WrapperEvent<EventHandler<ValueEventArgs<int>>>(brakeChangedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), ValueEventArgs<int>.FromSource(e)));
            PowerChangedEvent = new WrapperEvent<EventHandler<ValueEventArgs<int>>>(powerChangedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), ValueEventArgs<int>.FromSource(e)));
            ReverserChangedEvent = new WrapperEvent<EventHandler<ValueEventArgs<int>>>(reverserChangedEvent, x => (sender, e) => x?.Invoke(FromSource(sender), ValueEventArgs<int>.FromSource(e)));
            ConstantSpeedChangedEvent = members.GetSourceEventOf(nameof(ConstantSpeedChanged));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="HandleSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected HandleSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="HandleSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static HandleSet FromSource(object src) => src is null ? null : new HandleSet(src);

        private static FastMethod NotchInfoGetMethod;
        private static FastMethod NotchInfoSetMethod;
        /// <summary>
        /// ノッチの情報を取得・設定します。
        /// </summary>
        public NotchInfo NotchInfo
        {
            get => NotchInfo.FromSource(NotchInfoGetMethod.Invoke(Src, null));
            set => NotchInfoSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod BrakeNotchGetMethod;
        private static FastMethod BrakeNotchSetMethod;
        /// <summary>
        /// ブレーキノッチを取得・設定します。
        /// </summary>
        public int BrakeNotch
        {
            get => (int)BrakeNotchGetMethod.Invoke(Src, null);
            set => BrakeNotchSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PowerNotchGetMethod;
        private static FastMethod PowerNotchSetMethod;
        /// <summary>
        /// 力行ノッチを取得・設定します。
        /// </summary>
        public int PowerNotch
        {
            get => (int)PowerNotchGetMethod.Invoke(Src, null);
            set => PowerNotchSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ReverserPositionGetMethod;
        private static FastMethod ReverserPositionSetMethod;
#pragma warning disable IDE1006 // 命名スタイル
        private int _ReverserPosition
#pragma warning restore IDE1006 // 命名スタイル
        {
            get => (int)ReverserPositionGetMethod.Invoke(Src, null);
            set => ReverserPositionSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 逆転器の位置を取得・設定します。
        /// </summary>
        public ReverserPosition ReverserPosition
        {
            get => (ReverserPosition)_ReverserPosition;
            set => _ReverserPosition = (int)value;
        }

        private static FastMethod ConstantSpeedGetMethod;
        private static FastMethod ConstantSpeedSetMethod;
#pragma warning disable IDE1006 // 命名スタイル
        private int _ConstantSpeedMode
#pragma warning restore IDE1006 // 命名スタイル
        {
            get => (int)ConstantSpeedGetMethod.Invoke(Src, null);
            set => ConstantSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 定速制御の動作モードを取得・設定します。
        /// </summary>
        public ConstantSpeedMode ConstantSpeedMode
        {
            get => (ConstantSpeedMode)_ConstantSpeedMode;
            set => _ConstantSpeedMode = (int)value;
        }

        private static WrapperEvent<EventHandler<ValueEventArgs<int>>> BrakeChangedEvent;
        /// <summary>
        /// ブレーキハンドルが扱われたときに発生します。
        /// </summary>
        public event EventHandler<ValueEventArgs<int>> BrakeChanged
        {
            add => BrakeChangedEvent.Add(Src, value);
            remove => BrakeChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="BrakeChanged"/> イベントを実行します。
        /// </summary>
        public void BrakeChanged_Invoke(ValueEventArgs<int> args) => BrakeChangedEvent.Invoke(Src, args);

        private static WrapperEvent<EventHandler<ValueEventArgs<int>>> PowerChangedEvent;
        /// <summary>
        /// 主ハンドルが扱われたときに発生します。
        /// </summary>
        public event EventHandler<ValueEventArgs<int>> PowerChanged
        {
            add => PowerChangedEvent.Add(Src, value);
            remove => PowerChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="PowerChanged"/> イベントを実行します。
        /// </summary>
        public void PowerChanged_Invoke(ValueEventArgs<int> args) => PowerChangedEvent.Invoke(Src, args);

        private static WrapperEvent<EventHandler<ValueEventArgs<int>>> ReverserChangedEvent;
        /// <summary>
        /// レバーサーが扱われたときに発生します。
        /// </summary>
        public event EventHandler<ValueEventArgs<int>> ReverserChanged
        {
            add => ReverserChangedEvent.Add(Src, value);
            remove => ReverserChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="ReverserChanged"/> イベントを実行します。
        /// </summary>
        public void ReverserChanged_Invoke(ValueEventArgs<int> args) => ReverserChangedEvent.Invoke(Src, args);

        private static FastEvent ConstantSpeedChangedEvent;
        /// <summary>
        /// 定速制御モードが変更されたときに発生します。
        /// </summary>
        public event EventHandler ConstantSpeedChanged
        {
            add => ConstantSpeedChangedEvent.Add(Src, value);
            remove => ConstantSpeedChangedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="ConstantSpeedChanged"/> イベントを実行します。
        /// </summary>
        public void ConstantSpeedChanged_Invoke() => ConstantSpeedChangedEvent.Invoke(Src, new object[] { Src, EventArgs.Empty });
    }
}
