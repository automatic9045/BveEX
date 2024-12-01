using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using SlimDX;
using System.Drawing;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// カメラの位置に関する情報を提供します。
    /// </summary>
    public class CameraLocation : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CameraLocation>();

            TransformFromBlockGetMethod = members.GetSourcePropertyGetterOf(nameof(TransformFromBlock));
            TransformFromBlockSetMethod = members.GetSourcePropertySetterOf(nameof(TransformFromBlock));

            TransformFromCameraHomePositionGetMethod = members.GetSourcePropertyGetterOf(nameof(TransformFromCameraHomePosition));
            TransformFromCameraHomePositionSetMethod = members.GetSourcePropertySetterOf(nameof(TransformFromCameraHomePosition));

            PlaneGetMethod = members.GetSourcePropertyGetterOf(nameof(Plane));
            PlaneSetMethod = members.GetSourcePropertySetterOf(nameof(Plane));

            SpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(Speed));
            SpeedSetMethod = members.GetSourcePropertySetterOf(nameof(Speed));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CameraLocation"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CameraLocation(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CameraLocation"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static CameraLocation FromSource(object src) => src is null ? null : new CameraLocation(src);

        private static FastMethod TransformFromBlockGetMethod;
        private static FastMethod TransformFromBlockSetMethod;
        /// <summary>
        /// 現在位置のストラクチャーブロックの原点から見たときの、このカメラの相対位置を表すワールド変換行列を取得・設定します。
        /// </summary>
        public Matrix TransformFromBlock
        {
            get => (Matrix)TransformFromBlockGetMethod.Invoke(Src, null);
            set => TransformFromBlockSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TransformFromCameraHomePositionGetMethod;
        private static FastMethod TransformFromCameraHomePositionSetMethod;
        /// <summary>
        /// このカメラのホーム位置から見たときの、現在のカメラの相対位置を表す変換行列を取得・設定します。
        /// </summary>
        public Matrix TransformFromCameraHomePosition
        {
            get => (Matrix)TransformFromCameraHomePositionGetMethod.Invoke(Src, null);
            set => TransformFromCameraHomePositionSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PlaneGetMethod;
        private static FastMethod PlaneSetMethod;
        /// <summary>
        /// このカメラのプレーンを取得・設定します。
        /// </summary>
        public RectangleF Plane
        {
            get => (RectangleF)PlaneGetMethod.Invoke(Src, null);
            set => PlaneSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SpeedGetMethod;
        private static FastMethod SpeedSetMethod;
        /// <summary>
        /// このカメラが移動する速度 [m/s] を取得・設定します。
        /// </summary>
        public Vector3 Speed
        {
            get => (Vector3)SpeedGetMethod.Invoke(Src, null);
            set => SpeedSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
