using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 背景モデルを表します。
    /// </summary>
    public class Background : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Background>();

            BackgroundObjectsField = members.GetSourceFieldOf(nameof(BackgroundObjects));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Background"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Background(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Background"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Background FromSource(object src) => src is null ? null : new Background(src);


        private static FastField BackgroundObjectsField;
        /// <summary>
        /// 背景ストラクチャーのリストを取得・設定します。
        /// </summary>
        public MapFunctionList BackgroundObjects
        {
            get => MapFunctionList.FromSource(BackgroundObjectsField.GetValue(Src));
            set => BackgroundObjectsField.SetValue(Src, value?.Src);
        }

        private static FastMethod DrawMethod;
        /// <summary>
        /// 回転角度を指定して背景モデルを描画します。
        /// </summary>
        /// <param name="direct3DProvider">描画に使用する <see cref="Direct3DProvider"/>。</param>
        /// <param name="rotationMatrix">方角に合わせるための回転行列。</param>
        public void Draw(Direct3DProvider direct3DProvider, Matrix rotationMatrix) => DrawMethod.Invoke(Src, new object[] { direct3DProvider, rotationMatrix });
    }
}
