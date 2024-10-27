using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// <see cref="Cant"/> のリストを表します。
    /// </summary>
    public class CantList : InterpolatableMapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CantList>();

            RotationZGetMethod = members.GetSourcePropertyGetterOf(nameof(RotationZ));
            RotationZSetMethod = members.GetSourcePropertySetterOf(nameof(RotationZ));

            YGetMethod = members.GetSourcePropertyGetterOf(nameof(Y));
            YSetMethod = members.GetSourcePropertySetterOf(nameof(Y));

            XGetMethod = members.GetSourcePropertyGetterOf(nameof(X));
            XSetMethod = members.GetSourcePropertySetterOf(nameof(X));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CantList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CantList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="CantList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new CantList FromSource(object src) => src is null ? null : new CantList((IList)src);

        private static FastMethod RotationZGetMethod;
        private static FastMethod RotationZSetMethod;
        /// <summary>
        /// 最後に <see cref="MapObjectList.GoTo(double)"/> メソッドで設定された距離程における、カントの角度 [rad] を取得・設定します。
        /// </summary>
        public Physical RotationZ
        {
            get => Physical.FromSource(RotationZGetMethod.Invoke(Src, null));
            set => RotationZSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod YGetMethod;
        private static FastMethod YSetMethod;
        /// <summary>
        /// 最後に <see cref="MapObjectList.GoTo(double)"/> メソッドで設定された距離程における、カントの回転中心の Y 座標 [m] を取得・設定します。
        /// </summary>
        public Physical Y
        {
            get => Physical.FromSource(YGetMethod.Invoke(Src, null));
            set => YSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod XGetMethod;
        private static FastMethod XSetMethod;
        /// <summary>
        /// 最後に <see cref="MapObjectList.GoTo(double)"/> メソッドで設定された距離程における、カントの回転中心の X 座標 [m] を取得・設定します。
        /// </summary>
        public Physical X
        {
            get => Physical.FromSource(XGetMethod.Invoke(Src, null));
            set => XSetMethod.Invoke(Src, new object[] { value?.Src });
        }
    }
}
