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
    /// 運転台のハンドルを表します。
    /// </summary>
    public class CabBase : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CabBase>();

            HandlesGetMethod = members.GetSourcePropertyGetterOf(nameof(Handles));

            ReverserTextsGetMethod = members.GetSourcePropertyGetterOf  (nameof(ReverserTexts));
            ReverserTextsSetMethod = members.GetSourcePropertySetterOf(nameof(ReverserTexts));

            PowerTextsGetMethod = members.GetSourcePropertyGetterOf(nameof(PowerTexts));
            PowerTextsSetMethod = members.GetSourcePropertySetterOf(nameof(PowerTexts));

            BrakeTextsGetMethod = members.GetSourcePropertyGetterOf(nameof(BrakeTexts));
            BrakeTextsSetMethod = members.GetSourcePropertySetterOf(nameof(BrakeTexts));

            HoldingSpeedTextsGetMethod = members.GetSourcePropertyGetterOf(nameof(HoldingSpeedTexts));
            HoldingSpeedTextsSetMethod = members.GetSourcePropertySetterOf(nameof(HoldingSpeedTexts));

            GetDescriptionTextMethod = members.GetSourceMethodOf(nameof(GetDescriptionText));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CabBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CabBase(object src) : base(src)
        {
        }

        private static FastMethod HandlesGetMethod;
        /// <summary>
        /// 操作可能なハンドルのセットを取得します。
        /// </summary>
        public HandleSet Handles => HandleSet.FromSource(HandlesGetMethod.Invoke(Src, null));

        private static FastMethod ReverserTextsGetMethod;
        private static FastMethod ReverserTextsSetMethod;
        /// <summary>
        /// 逆転器の表示文字列の配列を取得・設定します。
        /// </summary>
        public string[] ReverserTexts
        {
            get => ReverserTextsGetMethod.Invoke(Src, null) as string[];
            set => ReverserTextsSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PowerTextsGetMethod;
        private static FastMethod PowerTextsSetMethod;
        /// <summary>
        /// 力行ノッチの表示文字列の配列を取得・設定します。
        /// </summary>
        public string[] PowerTexts
        {
            get => PowerTextsGetMethod.Invoke(Src, null) as string[];
            set => PowerTextsSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BrakeTextsGetMethod;
        private static FastMethod BrakeTextsSetMethod;
        /// <summary>
        /// ブレーキノッチの表示文字列の配列を取得・設定します。
        /// </summary>
        public string[] BrakeTexts
        {
            get => BrakeTextsGetMethod.Invoke(Src, null) as string[];
            set => BrakeTextsSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod HoldingSpeedTextsGetMethod;
        private static FastMethod HoldingSpeedTextsSetMethod;
        /// <summary>
        /// 抑速ノッチの表示文字列の配列を取得・設定します。
        /// </summary>
        public string[] HoldingSpeedTexts
        {
            get => HoldingSpeedTextsGetMethod.Invoke(Src, null) as string[];
            set => HoldingSpeedTextsSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod GetDescriptionTextMethod;
        /// <summary>
        /// 現在の状態を説明するテキストを取得します。
        /// </summary>
        public string GetDescriptionText() => GetDescriptionTextMethod.Invoke(Src, null) as string;

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
