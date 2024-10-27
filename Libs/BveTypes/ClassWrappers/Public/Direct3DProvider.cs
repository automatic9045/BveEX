using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SlimDX.Direct3D9;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// Direct3D の機能を利用するためのメンバーを提供します。
    /// </summary>
    public class Direct3DProvider : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Direct3DProvider>();

            InstanceGetMethod = members.GetSourcePropertyGetterOf(nameof(Instance));

            Direct3DGetMethod = members.GetSourcePropertyGetterOf(nameof(Direct3D));

            DeviceField = members.GetSourceFieldOf(nameof(Device));
            PresentParametersField = members.GetSourceFieldOf(nameof(PresentParameters));
            HasDeviceLostField = members.GetSourceFieldOf(nameof(HasDeviceLost));

            CreateDeviceMethod = members.GetSourceMethodOf(nameof(CreateDevice));
            InitializeStatesMethod = members.GetSourceMethodOf(nameof(InitializeStates));
            RenderMethod = members.GetSourceMethodOf(nameof(Render));
        }

        /// <summary>
        /// <see cref="Direct3DProvider"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Direct3DProvider(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Direct3DProvider"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Direct3DProvider FromSource(object src) => src is null ? null : new Direct3DProvider(src);

        private static FastMethod InstanceGetMethod;
        /// <summary>
        /// <see cref="Direct3DProvider"/> クラスのインスタンスを取得します。
        /// </summary>
        public static Direct3DProvider Instance => FromSource(InstanceGetMethod.Invoke(null, null));

        private static FastMethod Direct3DGetMethod;
        /// <summary>
        /// <see cref="SlimDX.Direct3D9.Direct3D"/> を取得します。
        /// </summary>
        public Direct3D Direct3D => (Direct3D)Direct3DGetMethod.Invoke(Src, null);

        private static FastField DeviceField;
        /// <summary>
        /// <see cref="SlimDX.Direct3D9.Device"/> を取得・設定します。
        /// </summary>
        public Device Device
        {
            get => DeviceField.GetValue(Src);
            set => DeviceField.SetValue(Src, value);
        }

        private static FastField PresentParametersField;
        /// <summary>
        /// <see cref="Device"/> の生成に使用した <see cref="SlimDX.Direct3D9.PresentParameters"/> を取得します。
        /// </summary>
        public PresentParameters PresentParameters
        {
            get => PresentParametersField.GetValue(Src);
            set => PresentParametersField.SetValue(Src, value);
        }

        private static FastField HasDeviceLostField;
        /// <summary>
        /// デバイス ロスト状態であるかどうかを取得・設定します。
        /// </summary>
        public bool HasDeviceLost
        {
            get => HasDeviceLostField.GetValue(Src);
            set => HasDeviceLostField.SetValue(Src, value);
        }

        private static FastMethod CreateDeviceMethod;
        /// <summary>
        /// 描画対象を指定して Direct3D9 デバイスを作成します。
        /// </summary>
        /// <param name="deviceWindow">描画対象の Windows Forms コントロール。</param>
        /// <param name="isWindowed">ウィンドウモードかどうか。</param>
        /// <param name="windowSize">描画対象のウィンドウのサイズ。</param>
        public void CreateDevice(Control deviceWindow, bool isWindowed, Size windowSize)
            => CreateDeviceMethod.Invoke(Src, new object[] { deviceWindow, isWindowed, windowSize });

        private static FastMethod InitializeStatesMethod;
        /// <summary>
        /// デバイスの各種状態を初期化します。
        /// </summary>
        public void InitializeStates() => InitializeStatesMethod.Invoke(Src, null);

        private static FastMethod RenderMethod;
        /// <summary>
        /// 指定したオブジェクトへ描画します。
        /// </summary>
        /// <param name="target">描画のターゲット。</param>
        public void Render(IDrawable target) => RenderMethod.Invoke(Src, new object[] { ((ClassWrapperBase)target)?.Src });
    }
}
