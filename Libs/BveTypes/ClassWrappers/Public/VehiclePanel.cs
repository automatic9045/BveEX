using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車の運転台パネルを表します。
    /// </summary>
    public class VehiclePanel : ClassWrapperBase, IDrawable, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<VehiclePanel>();

            PerspectiveGetMethod = members.GetSourcePropertyGetterOf(nameof(Perspective));
            PerspectiveSetMethod = members.GetSourcePropertySetterOf(nameof(Perspective));

            DistanceFromDriverGetMethod = members.GetSourcePropertyGetterOf(nameof(DistanceFromDriver));
            DistanceFromDriverSetMethod = members.GetSourcePropertySetterOf(nameof(DistanceFromDriver));

            StateStoreGetMethod = members.GetSourcePropertyGetterOf(nameof(StateStore));

            BorderGetMethod = members.GetSourcePropertyGetterOf(nameof(Border));
            BorderSetMethod = members.GetSourcePropertySetterOf(nameof(Border));

            CenterGetMethod = members.GetSourcePropertyGetterOf(nameof(Center));
            CenterSetMethod = members.GetSourcePropertySetterOf(nameof(Center));

            OriginGetMethod = members.GetSourcePropertyGetterOf(nameof(Origin));
            OriginSetMethod = members.GetSourcePropertySetterOf(nameof(Origin));

            BrightnessSetMethod = members.GetSourcePropertySetterOf(nameof(Brightness));

            ResolutionGetMethod = members.GetSourcePropertyGetterOf(nameof(Resolution));
            ResolutionSetMethod = members.GetSourcePropertySetterOf(nameof(Resolution));

            ElementsGetMethod = members.GetSourcePropertyGetterOf(nameof(Elements));
            ElementsSetMethod = members.GetSourcePropertySetterOf(nameof(Elements));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            OnDeviceLostMethod = members.GetSourceMethodOf(nameof(OnDeviceLost));
            OnDeviceResetMethod = members.GetSourceMethodOf(nameof(OnDeviceReset));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="VehiclePanel"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected VehiclePanel(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="VehiclePanel"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static VehiclePanel FromSource(object src) => src is null ? null : new VehiclePanel(src);

        private static FastMethod PerspectiveGetMethod;
        private static FastMethod PerspectiveSetMethod;
        /// <summary>
        /// Tilt キーを設定した計器のパース (遠近感) の強さを取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定値は 1 です。値が大きいほどパースは弱くなります。
        /// </remarks>
        public double Perspective
        {
            get => PerspectiveGetMethod.Invoke(Src, null);
            set => PerspectiveSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DistanceFromDriverGetMethod;
        private static FastMethod DistanceFromDriverSetMethod;
        /// <summary>
        /// 運転士から運転台までの距離 [m] を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定では 0.8 で固定となっています。
        /// </remarks>
        public double DistanceFromDriver
        {
            get => DistanceFromDriverGetMethod.Invoke(Src, null);
            set => DistanceFromDriverSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod StateStoreGetMethod;
        /// <summary>
        /// 自列車の状態に関する情報を取得します。
        /// </summary>
        public VehicleStateStore StateStore => VehicleStateStore.FromSource(StateStoreGetMethod.Invoke(Src, null));

        private static FastMethod BorderGetMethod;
        private static FastMethod BorderSetMethod;
        /// <summary>
        /// 視点移動で表示可能な限界を取得・設定します。
        /// </summary>
        public Rectangle Border
        {
            get => BorderGetMethod.Invoke(Src, null);
            set => BorderSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CenterGetMethod;
        private static FastMethod CenterSetMethod;
        /// <summary>
        /// 既定の視点において画面の中心になる座標を取得・設定します。
        /// </summary>
        public Point Center
        {
            get => CenterGetMethod.Invoke(Src, null);
            set => CenterSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod OriginGetMethod;
        private static FastMethod OriginSetMethod;
        /// <summary>
        /// 消失点の座標を取得・設定します。
        /// </summary>
        public Point Origin
        {
            get => OriginGetMethod.Invoke(Src, null);
            set => OriginSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod BrightnessSetMethod;
        /// <summary>
        /// 運転台の明るさを設定します。
        /// </summary>
        public double Brightness
        {
            set => BrightnessSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ResolutionGetMethod;
        private static FastMethod ResolutionSetMethod;
        /// <summary>
        /// ズーム 1 倍の時の X 解像度を取得・設定します。
        /// </summary>
        public double Resolution
        {
            get => ResolutionGetMethod.Invoke(Src, null);
            set => ResolutionSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ElementsGetMethod;
        private static FastMethod ElementsSetMethod;
        /// <summary>
        /// 計器要素の一覧を取得・設定します。
        /// </summary>
        public WrappedList<VehiclePanelElement> Elements
        {
            get => WrappedList<VehiclePanelElement>.FromSource(ElementsGetMethod.Invoke(Src, null));
            set => ElementsSetMethod.Invoke(Src, new object[] { value.Src });
        }

        private static FastMethod DrawMethod;
        /// <summary>
        /// 運転台パネルを描画します。
        /// </summary>
        public void Draw() => DrawMethod.Invoke(Src, null);

        private static FastMethod OnDeviceLostMethod;
        /// <inheritdoc/>
        public void OnDeviceLost() => OnDeviceLostMethod.Invoke(Src, null);

        private static FastMethod OnDeviceResetMethod;
        /// <inheritdoc/>
        public void OnDeviceReset() => OnDeviceResetMethod.Invoke(Src, null);

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
