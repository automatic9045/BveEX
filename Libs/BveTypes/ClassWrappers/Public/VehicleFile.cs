using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 車両ファイルを表します。
    /// </summary>
    public class VehicleFile : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehicleFile>();

            PathGetMethod = members.GetSourcePropertyGetterOf(nameof(Path));
            PathSetMethod = members.GetSourcePropertySetterOf(nameof(Path));

            ParametersPathGetMethod = members.GetSourcePropertyGetterOf(nameof(ParametersPath));
            ParametersPathSetMethod = members.GetSourcePropertySetterOf(nameof(ParametersPath));

            PerformanceCurvePathGetMethod = members.GetSourcePropertyGetterOf(nameof(PerformanceCurvePath));
            PerformanceCurvePathSetMethod = members.GetSourcePropertySetterOf(nameof(PerformanceCurvePath));

            AtsPluginPathGetMethod = members.GetSourcePropertyGetterOf(nameof(AtsPluginPath));
            AtsPluginPathSetMethod = members.GetSourcePropertySetterOf(nameof(AtsPluginPath));

            PanelPathGetMethod = members.GetSourcePropertyGetterOf(nameof(PanelPath));
            PanelPathSetMethod = members.GetSourcePropertySetterOf(nameof(PanelPath));

            SoundPathGetMethod = members.GetSourcePropertyGetterOf(nameof(SoundPath));
            SoundPathSetMethod = members.GetSourcePropertySetterOf(nameof(SoundPath));

            MotorNoisePathGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorNoisePath));
            MotorNoisePathSetMethod = members.GetSourcePropertySetterOf(nameof(MotorNoisePath));

            FromFileMethod = members.GetSourceMethodOf(nameof(FromFile));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehicleFile"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehicleFile(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehicleFile"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehicleFile FromSource(object src) => src is null ? null : new VehicleFile(src);

        private static FastMethod PathGetMethod;
        private static FastMethod PathSetMethod;
        /// <summary>
        /// このファイルの絶対パスを取得・設定します。
        /// </summary>
        public string Path
        {
            get => (string)PathGetMethod.Invoke(Src, null);
            set => PathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ParametersPathGetMethod;
        private static FastMethod ParametersPathSetMethod;
        /// <summary>
        /// 車両パラメーターファイルの絶対パスを取得・設定します。
        /// </summary>
        public string ParametersPath
        {
            get => (string)ParametersPathGetMethod.Invoke(Src, null);
            set => ParametersPathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PerformanceCurvePathGetMethod;
        private static FastMethod PerformanceCurvePathSetMethod;
        /// <summary>
        /// 車両性能ファイルの絶対パスを取得・設定します。
        /// </summary>
        public string PerformanceCurvePath
        {
            get => (string)PerformanceCurvePathGetMethod.Invoke(Src, null);
            set => PerformanceCurvePathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod AtsPluginPathGetMethod;
        private static FastMethod AtsPluginPathSetMethod;
        /// <summary>
        /// ATS プラグインファイルの絶対パスを取得・設定します。
        /// </summary>
        public string AtsPluginPath
        {
            get => (string)AtsPluginPathGetMethod.Invoke(Src, null);
            set => AtsPluginPathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PanelPathGetMethod;
        private static FastMethod PanelPathSetMethod;
        /// <summary>
        /// 運転台パネルファイルの絶対パスを取得・設定します。
        /// </summary>
        public string PanelPath
        {
            get => (string)PanelPathGetMethod.Invoke(Src, null);
            set => PanelPathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SoundPathGetMethod;
        private static FastMethod SoundPathSetMethod;
        /// <summary>
        /// 車両サウンドファイルの絶対パスを取得・設定します。
        /// </summary>
        public string SoundPath
        {
            get => (string)SoundPathGetMethod.Invoke(Src, null);
            set => SoundPathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MotorNoisePathGetMethod;
        private static FastMethod MotorNoisePathSetMethod;
        /// <summary>
        /// モーター音ファイルの絶対パスを取得・設定します。
        /// </summary>
        public string MotorNoisePath
        {
            get => (string)MotorNoisePathGetMethod.Invoke(Src, null);
            set => MotorNoisePathSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod FromFileMethod;
        /// <summary>
        /// 車両ファイルを読み込みます。
        /// </summary>
        /// <param name="loadingProgressForm">「シナリオを読み込んでいます...」フォーム。</param>
        /// <param name="path">車両ファイルのパス。</param>
        public static VehicleFile FromFile(LoadingProgressForm loadingProgressForm, string path)
            => FromSource(FromFileMethod.Invoke(null, new object[] { loadingProgressForm?.Src, path }));
    }
}
