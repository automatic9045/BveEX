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
    /// マップファイルの構文を定義します。
    /// </summary>
    public class MapGrammar : MapParserBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapGrammar>();

            Constructor = members.GetSourceConstructor();
        }

        /// <summary>
        /// ラップされているオリジナルの <see cref="Grammar"/> 型オブジェクトを取得します。
        /// </summary>
        public new Grammar Src => (Grammar)base.Src;

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapGrammar"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapGrammar(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapGrammar"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapGrammar FromSource(object src) => src is null ? null : new MapGrammar(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="MapGrammar"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MapGrammar() : base(Constructor.Invoke(null))
        {
        }
    }
}
