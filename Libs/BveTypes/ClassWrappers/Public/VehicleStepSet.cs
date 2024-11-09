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
    /// 自列車の制御段 (特性曲線) のセットを表します。
    /// </summary>
    public class VehicleStepSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleStepSet>();

            CountGetMethod = members.GetSourcePropertyGetterOf(nameof(StepCount));

            CurrentIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentStepIndex));
            CurrentIndexSetMethod = members.GetSourcePropertySetterOf(nameof(CurrentStepIndex));

            CurrentCurveGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentStep));

            GetStepMethod = members.GetSourceMethodOf(nameof(GetStep));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleStepSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleStepSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleStepSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleStepSet FromSource(object src) => src is null ? null : new VehicleStepSet(src);

        private static FastMethod CountGetMethod;
        /// <summary>
        /// 総制御段数を取得します。
        /// </summary>
        public int StepCount => (int)CountGetMethod.Invoke(Src, null);

        private static FastMethod CurrentIndexGetMethod;
        private static FastMethod CurrentIndexSetMethod;
        /// <summary>
        /// 現在の制御段のインデックスを取得・設定します。
        /// </summary>
        public int CurrentStepIndex
        {
            get => (int)CurrentIndexGetMethod.Invoke(Src, null);
            set => CurrentIndexSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CurrentCurveGetMethod;
        /// <summary>
        /// 現在の制御段 (特性曲線) を取得します。
        /// </summary>
        public VehicleStep CurrentStep => VehicleStep.FromSource(CurrentCurveGetMethod.Invoke(Src, null));

        private static FastMethod GetStepMethod;
        /// <summary>
        /// 指定したインデックスの制御段 (特性曲線) を取得します。
        /// </summary>
        /// <param name="index">取得する制御段のインデックス。</param>
        /// <returns>インデックスが <paramref name="index"/> の制御段。</returns>
        public VehicleStep GetStep(int index) => VehicleStep.FromSource(GetStepMethod.Invoke(Src, new object[] { index }));
    }
}
