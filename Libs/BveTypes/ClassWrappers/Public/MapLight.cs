using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using SlimDX;
using SlimDX.Direct3D9;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// マップ上のストラクチャーの描画に対して作用する光を表します。
    /// </summary>
    public class MapLight : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapLight>();

            Constructor = members.GetSourceConstructor();

            AmbientGetMethod = members.GetSourcePropertyGetterOf(nameof(Ambient));
            AmbientSetMethod = members.GetSourcePropertySetterOf(nameof(Ambient));

            DiffuseGetMethod = members.GetSourcePropertyGetterOf(nameof(Diffuse));
            DiffuseSetMethod = members.GetSourcePropertySetterOf(nameof(Diffuse));

            IsFixedToGroundGetMethod = members.GetSourcePropertyGetterOf(nameof(IsFixedToGround));
            IsFixedToGroundSetMethod = members.GetSourcePropertySetterOf(nameof(IsFixedToGround));

            TransformField = members.GetSourceFieldOf(nameof(Transform));

            SetDirectionMethod = members.GetSourceMethodOf(nameof(SetDirection));
            UpdateTransformMethod = members.GetSourceMethodOf(nameof(UpdateTransform));
            ApplyMethod = members.GetSourceMethodOf(nameof(Apply));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapLight"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapLight(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapLight"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapLight FromSource(object src) => src is null ? null : new MapLight(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="MapLight"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MapLight() : this(Constructor.Invoke(null))
        {
        }

        private static FastMethod AmbientGetMethod;
        private static FastMethod AmbientSetMethod;
        /// <summary>
        /// 環境光の色を取得・設定します。
        /// </summary>
        public Color Ambient
        {
            get => (Color)AmbientGetMethod.Invoke(Src, null);
            set => AmbientSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DiffuseGetMethod;
        private static FastMethod DiffuseSetMethod;
        /// <summary>
        /// 平行光の色を取得・設定します。
        /// </summary>
        public Color Diffuse
        {
            get => (Color)DiffuseGetMethod.Invoke(Src, null);
            set => DiffuseSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod IsFixedToGroundGetMethod;
        private static FastMethod IsFixedToGroundSetMethod;
        /// <summary>
        /// 線形に合わせて方向を動かすかどうかを取得・設定します。
        /// </summary>
        public bool IsFixedToGround
        {
            get => (bool)IsFixedToGroundGetMethod.Invoke(Src, null);
            set => IsFixedToGroundSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField TransformField;
        /// <summary>
        /// 距離程 0 において、単位方向ベクトル (0,0,1) を平行光が指す方向へ回転する変換行列を取得・設定します。
        /// </summary>
        public Matrix Transform
        {
            get => (Matrix)TransformField.GetValue(Src);
            set => TransformField.SetValue(Src, value);
        }

        private static FastMethod SetDirectionMethod;
        /// <summary>
        /// 距離程 0 において平行光が指す方向を設定します。
        /// </summary>
        /// <param name="pitch">ピッチ (X) 成分。</param>
        /// <param name="yaw">ヨー (Y) 成分。</param>
        public void SetDirection(double pitch, double yaw) => SetDirectionMethod.Invoke(Src, new object[] { pitch, yaw });

        private static FastMethod UpdateTransformMethod;
        /// <summary>
        /// 平行光が指す方向の設定値を基に、<see cref="Transform"/> プロパティの値を更新します。
        /// </summary>
        public void UpdateTransform() => UpdateTransformMethod.Invoke(Src, null);

        private static FastMethod ApplyMethod;
        /// <summary>
        /// 指定した Direct3D9 デバイスに光の設定を適用します。
        /// </summary>
        /// <param name="device">光の設定を適用するデバイス。</param>
        /// <param name="world">ワールド変換行列。</param>
        public void Apply(Device device, Matrix world) => ApplyMethod.Invoke(Src, new object[] { device, world });
    }
}
