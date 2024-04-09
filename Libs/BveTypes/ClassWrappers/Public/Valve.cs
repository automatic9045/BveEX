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
    /// 弁を表します。
    /// </summary>
    public class Valve : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Valve>();

            MrPressureField = members.GetSourceFieldOf(nameof(MrPressure));
            TargetPressureField = members.GetSourceFieldOf(nameof(TargetPressure));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Valve"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Valve(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Valve"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Valve FromSource(object src) => src is null ? null : new Valve(src);

        private static FastField MrPressureField;
        /// <summary>
        /// 元溜め圧力 [Pa] を取得・設定します。
        /// </summary>
        public ValueContainer MrPressure
        {
            get => ValueContainer.FromSource(MrPressureField.GetValue(Src));
            set => MrPressureField.SetValue(Src, value.Src);
        }

        private static FastField TargetPressureField;
        /// <summary>
        /// 目標圧力 [Pa] を取得・設定します。
        /// </summary>
        public ValueContainer TargetPressure
        {
            get => ValueContainer.FromSource(TargetPressureField.GetValue(Src));
            set => TargetPressureField.SetValue(Src, value.Src);
        }

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize()
            => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public void Tick(double elapsedSeconds)
            => TickMethod.Invoke(Src, new object[] { elapsedSeconds });
    }
}
