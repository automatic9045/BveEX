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
    /// マップの構文解析を行います。
    /// </summary>
    public class MapParserBase : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapParserBase>();

            FilePathField = members.GetSourceFieldOf(nameof(FilePath));

            ParseMethod = members.GetSourceMethodOf(nameof(Parse));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapParserBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapParserBase(object src) : base(src)
        {
        }

        private static FastField FilePathField;
        /// <summary>
        /// 対象とするマップファイルのパスを取得・設定します。
        /// </summary>
        public string FilePath
        {
            get => FilePathField.GetValue(Src);
            set => FilePathField.SetValue(Src, value);
        }

        private static FastMethod ParseMethod;
        /// <summary>
        /// マップ構文を解析します。
        /// </summary>
        /// <param name="sourceText">マップ構文のテキスト。</param>
        /// <param name="filePath">マップファイルのパス。この値はエラーを表示する際に使用されます。</param>
        public string Parse(string sourceText, string filePath) => ParseMethod.Invoke(Src, new object[] { sourceText, filePath });
    }
}
