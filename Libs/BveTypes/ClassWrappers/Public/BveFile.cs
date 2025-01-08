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
    /// シナリオを構成するファイルを表します。
    /// </summary>
    public class BveFile : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BveFile>();

            PathGetMethod = members.GetSourcePropertyGetterOf(nameof(Path));
            PathSetMethod = members.GetSourcePropertySetterOf(nameof(Path));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="BveFile"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BveFile(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BveFile"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BveFile FromSource(object src) => src is null ? null : new BveFile(src);

        private static FastMethod PathGetMethod;
        private static FastMethod PathSetMethod;
        /// <summary>
        /// ファイルのパスを取得・設定します。
        /// </summary>
        public string Path
        {
            get => PathGetMethod.Invoke(Src, null) as string;
            set => PathSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
