using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using Irony.Parsing;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// バージョン 2.0 以降のマップの構文解析を行います。
    /// </summary>
    public class MapParser : MapParserBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapParser>();

            IncludeMethod = members.GetSourceMethodOf(nameof(Include));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapParser"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapParser(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapParser"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapParser FromSource(object src) => src is null ? null : new MapParser(src);

        private static FastMethod IncludeMethod;
        /// <summary>
        /// 他のマップファイルをインクルードします。
        /// </summary>
        /// <param name="node">インクルード文。</param>
        public void Include(ParseTreeNode node) => IncludeMethod.Invoke(Src, new object[] { node });
    }
}
