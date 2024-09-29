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
    /// バージョン 0.07、1.00、1.10 のマップの構文解析を行います。
    /// </summary>
    public class ClassicMapParser : MapParserBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ClassicMapParser>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ClassicMapParser"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ClassicMapParser(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ClassicMapParser"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ClassicMapParser FromSource(object src) => src is null ? null : new ClassicMapParser(src);
    }
}
