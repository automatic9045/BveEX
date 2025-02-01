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

            ParentField = members.GetSourceFieldOf(nameof(Parent));
            VariablesField = members.GetSourceFieldOf(nameof(Variables));
            FilePathField = members.GetSourceFieldOf(nameof(FilePath));
            StatementsField = members.GetSourceFieldOf(nameof(Statements));
            LocationField = members.GetSourceFieldOf(nameof(Location));

            ParseMethod = members.GetSourceMethodOf(nameof(Parse));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapParserBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapParserBase(object src) : base(src)
        {
        }

        private static FastField ParentField;
        /// <summary>
        /// 親となる <see cref="IMapLoader"/> オブジェクトを取得・設定します。
        /// </summary>
        public IMapLoader Parent
        {
            get => CreateFromSource(ParentField.GetValue(Src)) as IMapLoader;
            set => ParentField.SetValue(Src, (value as ClassWrapperBase)?.Src);
        }

        private static FastField VariablesField;
        /// <summary>
        /// 変数の一覧を取得・設定します。
        /// </summary>
        public SortedList<string, object> Variables
        {
            get => VariablesField.GetValue(Src) as SortedList<string, object>;
            set => VariablesField.SetValue(Src, value);
        }

        private static FastField FilePathField;
        /// <summary>
        /// 対象とするマップファイルのパスを取得・設定します。
        /// </summary>
        public string FilePath
        {
            get => FilePathField.GetValue(Src) as string;
            set => FilePathField.SetValue(Src, value);
        }

        private static FastField StatementsField;
        /// <summary>
        /// 読み込んだステートメントの一覧を取得・設定します。
        /// </summary>
        public MapStatementList Statements
        {
            get => MapStatementList.FromSource(StatementsField.GetValue(Src));
            set => StatementsField.SetValue(Src, value?.Src);
        }

        private static FastField LocationField;
        /// <summary>
        /// 距離程 [m] を取得・設定します。
        /// </summary>
        public double Location
        {
            get => (double)LocationField.GetValue(Src);
            set => LocationField.SetValue(Src, value);
        }

        private static FastMethod ParseMethod;
        /// <summary>
        /// マップ構文を解析します。
        /// </summary>
        /// <param name="sourceText">マップ構文のテキスト。</param>
        /// <param name="filePath">マップファイルのパス。この値はエラーを表示する際に使用されます。</param>
        public virtual void Parse(string sourceText, string filePath) => ParseMethod.Invoke(Src, new object[] { sourceText, filePath });
    }
}
