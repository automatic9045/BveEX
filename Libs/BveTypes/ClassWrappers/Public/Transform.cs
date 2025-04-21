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
    /// 変換行列を格納します。
    /// </summary>
    public class Transform : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Transform>();

            MatrixField = members.GetSourceFieldOf(nameof(Matrix));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Transform"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Transform(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Transform"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Transform FromSource(object src) => src is null ? null : new Transform(src);

        private static FastField MatrixField;
        /// <summary>
        /// 変換行列を取得・設定します。
        /// </summary>
        public Matrix Matrix
        {
            get => (Matrix)MatrixField.GetValue(Src);
            set => MatrixField.SetValue(Src, value);
        }
    }
}
