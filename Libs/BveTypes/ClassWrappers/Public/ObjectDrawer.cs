using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// BVE によって読み込まれた全ての 3D モデルを描画するための機能を提供します。
    /// </summary>
    public class ObjectDrawer : ClassWrapperBase, IDrawable, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ObjectDrawer>();

            VehicleLocationField = members.GetSourceFieldOf(nameof(VehicleLocation));
            StructureDrawerField = members.GetSourceFieldOf(nameof(StructureDrawer));
            BackgroundField = members.GetSourceFieldOf(nameof(Background));
            CameraLocationField = members.GetSourceFieldOf(nameof(CameraLocation));
            TrainsField = members.GetSourceFieldOf(nameof(Trains));
            MapField = members.GetSourceFieldOf(nameof(Map));
            DrawDistanceManagerField = members.GetSourceFieldOf(nameof(DrawDistanceManager));

            BuildMethod = members.GetSourceMethodOf(nameof(Build));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            OnDeviceLostMethod = members.GetSourceMethodOf(nameof(OnDeviceLost));
            OnDeviceResetMethod = members.GetSourceMethodOf(nameof(OnDeviceReset));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ObjectDrawer"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ObjectDrawer(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ObjectDrawer"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ObjectDrawer FromSource(object src) => src is null ? null : new ObjectDrawer(src);

        private static FastField VehicleLocationField;
        /// <summary>
        /// 自列車の位置情報を取得・設定します。
        /// </summary>
        public VehicleLocation VehicleLocation
        {
            get => VehicleLocation.FromSource(VehicleLocationField.GetValue(Src));
            set => VehicleLocationField.SetValue(Src, value?.Src);
        }

        private static FastField StructureDrawerField;
        /// <summary>
        /// ストラクチャーを描画するための機能を提供する <see cref="ClassWrappers.StructureDrawer"/> を取得・設定します。
        /// </summary>
        public StructureDrawer StructureDrawer
        {
            get => StructureDrawer.FromSource(StructureDrawerField.GetValue(Src));
            set => StructureDrawerField.SetValue(Src, value?.Src);
        }

        private static FastField BackgroundField;
        /// <summary>
        /// 背景モデルを取得・設定します。
        /// </summary>
        public Background Background
        {
            get => Background.FromSource(BackgroundField.GetValue(Src));
            set => BackgroundField.SetValue(Src, value?.Src);
        }

        private static FastField CameraLocationField;
        /// <summary>
        /// カメラの位置情報を取得・設定します。
        /// </summary>
        public CameraLocation CameraLocation
        {
            get => CameraLocation.FromSource(CameraLocationField.GetValue(Src));
            set => CameraLocationField.SetValue(Src, value?.Src);
        }

        private static FastField TrainsField;
        /// <summary>
        /// 他列車の一覧を取得・設定します。
        /// </summary>
        /// <remarks>キーはマップファイル内で定義した他列車名、値は他列車を表す <see cref="Train"/> です。</remarks>
        public WrappedSortedList<string, Train> Trains
        {
            get
            {
                object src = TrainsField.GetValue(Src);
                return src is IDictionary dictionarySrc ? new WrappedSortedList<string, Train>(dictionarySrc) : null;
            }
            set => TrainsField.SetValue(Src, value?.Src);
        }

        private static FastField MapField;
        /// <summary>
        /// 路線データを取得・設定します。
        /// </summary>
        public Map Map
        {
            get => Map.FromSource(MapField.GetValue(Src));
            set => MapField.SetValue(Src, value?.Src);
        }

        private static FastField DrawDistanceManagerField;
        /// <summary>
        /// ストラクチャーの描画範囲を算出するための機能を提供する <see cref="ClassWrappers.DrawDistanceManager"/> を取得・設定します。
        /// </summary>
        public DrawDistanceManager DrawDistanceManager
        {
            get => DrawDistanceManager.FromSource(DrawDistanceManagerField.GetValue(Src));
            set => DrawDistanceManagerField.SetValue(Src, value?.Src);
        }

        private static FastMethod BuildMethod;
        /// <summary>
        /// 風景を構築します。
        /// </summary>
        /// <param name="map">路線データ。</param>
        public void Build(Map map) => BuildMethod.Invoke(Src, new object[] { map?.Src });

        /// <summary>
        /// 互換性のために残されている古いメソッドです。<see cref="Build(Map)"/> メソッドを使用してください。
        /// </summary>
        /// <param name="map">処理対象となるマップ。</param>
        /// <seealso cref="Build(Map)"/>
        [Obsolete]
        public void SetMap(Map map) => Build(map);

        private static FastMethod DrawMethod;
        /// <summary>
        /// 3D モデル群を描画します。
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
