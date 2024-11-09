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
        /// ノッチの情報を表す <see cref="ClassWrappers.NotchInfo"/> を取得します。
        /// </summary>
        public NotchInfo NotchInfo
        {
            get => ClassWrappers.NotchInfo.FromSource(NotchInfoGetMethod.Invoke(Src, null));
            internal set => NotchInfoSetMethod.Invoke(Src, new object[] { value?.Src });
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
    }
}
