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
    /// 自軌道のカントを表します。
    /// </summary>
    public class Cant : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Cant>();

            RotationZGetMethod = members.GetSourcePropertyGetterOf(nameof(RotationZ));

            CenterXGetMethod = members.GetSourcePropertyGetterOf(nameof(CenterX));

            FunctionIdGetMethod = members.GetSourcePropertyGetterOf(nameof(FunctionId));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Cant"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Cant(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Cant"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Cant FromSource(object src) => src is null ? null : new Cant(src);

        private static FastMethod RotationZGetMethod;
        /// <summary>
        /// カントの角度 [rad] を取得します。
        /// </summary>
        public double RotationZ => (double)RotationZGetMethod.Invoke(Src, null);

        private static FastMethod CenterXGetMethod;
        /// <summary>
        /// このカントの回転中心の X 座標 [m] を取得します。
        /// </summary>
        public double CenterX => (double)CenterXGetMethod.Invoke(Src, null);

        private static FastMethod FunctionIdGetMethod;
        /// <summary>
        /// このカントが設置されているカーブの緩和曲線関数を取得します。
        /// </summary>
        public int FunctionId => (int)FunctionIdGetMethod.Invoke(Src, null);
    }
}
