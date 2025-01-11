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
    /// 自列車の車両性能の基本クラスを表します。
    /// </summary>
    public class VehiclePerformanceBase : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehiclePerformanceBase>();

            PowerGetMethod = members.GetSourcePropertyGetterOf(nameof(Power));
            BrakeGetMethod = members.GetSourcePropertyGetterOf(nameof(Brake));

            LoadFromFileMethod = members.GetSourceMethodOf(nameof(LoadFromFile));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehiclePerformanceBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehiclePerformanceBase(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehiclePerformanceBase"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehiclePerformanceBase FromSource(object src) => src is null ? null : new VehiclePerformanceBase(src);

        private static FastMethod PowerGetMethod;
        /// <summary>
        /// 力行性能を取得します。
        /// </summary>
        public VehicleStepSet Power => VehicleStepSet.FromSource(PowerGetMethod.Invoke(Src, null));

        private static FastMethod BrakeGetMethod;
        /// <summary>
        /// 電気ブレーキ性能を取得します。
        /// </summary>
        public VehicleStepSet Brake => VehicleStepSet.FromSource(BrakeGetMethod.Invoke(Src, null));

        private static FastMethod LoadFromFileMethod;
        /// <summary>
        /// 車両性能ファイルを読み込みます。
        /// </summary>
        /// <param name="loadingProgressForm">エラーの出力先となる「シナリオを読み込んでいます...」フォーム。</param>
        /// <param name="filePath">車両性能ファイルのパス。</param>
        public void LoadFromFile(LoadingProgressForm loadingProgressForm, string filePath) => LoadFromFileMethod.Invoke(Src, new object[] { loadingProgressForm?.Src, filePath });
    }
}
