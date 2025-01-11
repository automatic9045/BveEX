using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;
using static System.Net.Mime.MediaTypeNames;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// INI 形式の設定ファイルを読み込むための機能を提供します。
    /// </summary>
    public class IniReader : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<IniReader>();

            Constructor = members.GetSourceConstructor();

            CommentMarksGetMethod = members.GetSourcePropertyGetterOf(nameof(CommentMarks));
            CommentMarksSetMethod = members.GetSourcePropertySetterOf(nameof(CommentMarks));

            SectionNameGetMethod = members.GetSourcePropertyGetterOf(nameof(SectionName));

            KeyGetMethod = members.GetSourcePropertyGetterOf(nameof(Key));

            LineIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(LineIndex));

            DirectoryNameGetMethod = members.GetSourcePropertyGetterOf(nameof(DirectoryName));

            FileNameGetMethod = members.GetSourcePropertyGetterOf(nameof(FileName));

            ReadNextMethod = members.GetSourceMethodOf(nameof(ReadNext));
            GetStringMethod = members.GetSourceMethodOf(nameof(GetString));
            GetSingleMethod = members.GetSourceMethodOf(nameof(GetSingle));
            GetDoubleMethod = members.GetSourceMethodOf(nameof(GetDouble));
            GetInt32Method = members.GetSourceMethodOf(nameof(GetInt32));
            GetBooleanMethod = members.GetSourceMethodOf(nameof(GetBoolean));
            GetStringArrayMethod = members.GetSourceMethodOf(nameof(GetStringArray));
            GetDoubleArrayMethod = members.GetSourceMethodOf(nameof(GetDoubleArray));
            GetInt32ArrayMethod = members.GetSourceMethodOf(nameof(GetInt32Array));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="IniReader"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected IniReader(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="IniReader"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static IniReader FromSource(object src) => src is null ? null : new IniReader(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// 指定したファイルを読み込むための <see cref="IniReader"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス。</param>
        /// <param name="validHeaders">有効なヘッダー文字列の一覧。</param>
        public IniReader(string filePath, string[] validHeaders) : base(Constructor.Invoke(new object[] { filePath, validHeaders }))
        {
        }

        private static FastMethod CommentMarksGetMethod;
        private static FastMethod CommentMarksSetMethod;
        /// <summary>
        /// コメントを表す記号の一覧を取得・設定します。
        /// </summary>
        public char[] CommentMarks
        {
            get => CommentMarksGetMethod.Invoke(Src, null) as char[];
            set => CommentMarksSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SectionNameGetMethod;
        /// <summary>
        /// 現在のセクションの名前を取得します。
        /// </summary>
        public string SectionName => SectionNameGetMethod.Invoke(Src, null) as string;

        private static FastMethod KeyGetMethod;
        /// <summary>
        /// 現在の要素のキーを取得します。
        /// </summary>
        public string Key => KeyGetMethod.Invoke(Src, null) as string;

        private static FastMethod LineIndexGetMethod;
        /// <summary>
        /// 現在の要素の行番号を取得します。
        /// </summary>
        public int LineIndex => (int)LineIndexGetMethod.Invoke(Src, null);

        private static FastMethod DirectoryNameGetMethod;
        /// <summary>
        /// 読み込んでいるファイルのディレクトリ名を取得します。
        /// </summary>
        public string DirectoryName => DirectoryNameGetMethod.Invoke(Src, null) as string;

        private static FastMethod FileNameGetMethod;
        /// <summary>
        /// 読み込んでいるファイルの名前を取得します。
        /// </summary>
        public string FileName => FileNameGetMethod.Invoke(Src, null) as string;

        private static FastMethod ReadNextMethod;
        /// <summary>
        /// 次の要素を読み出します。
        /// </summary>
        /// <returns>
        /// 次の要素が存在し、正常に読み出された場合は <see langword="true"/>。次の要素が存在しない場合は <see langword="false"/>。
        /// </returns>
        public bool ReadNext() => (bool)ReadNextMethod.Invoke(Src, null);

        private static FastMethod GetStringMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="string"/> 型として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="string"/> 型表現。
        /// </returns>
        public string GetString() => GetStringMethod.Invoke(Src, null) as string;

        private static FastMethod GetSingleMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="float"/> 型として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="float"/> 型表現。
        /// </returns>
        public float GetSingle() => (float)GetSingleMethod.Invoke(Src, null);

        private static FastMethod GetDoubleMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="double"/> 型として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="double"/> 型表現。
        /// </returns>
        public double GetDouble() => (double)GetDoubleMethod.Invoke(Src, null);

        private static FastMethod GetInt32Method;
        /// <summary>
        /// 現在の要素の値を <see cref="int"/> 型として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="int"/> 型表現。
        /// </returns>
        public int GetInt32() => (int)GetInt32Method.Invoke(Src, null);

        private static FastMethod GetBooleanMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="bool"/> 型として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="bool"/> 型表現。
        /// </returns>
        public bool GetBoolean() => (bool)GetBooleanMethod.Invoke(Src, null);

        private static FastMethod GetStringArrayMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="string"/> 型の配列として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="string"/> 型配列表現。
        /// </returns>
        public string[] GetStringArray() => GetStringArrayMethod.Invoke(Src, null) as string[];

        private static FastMethod GetDoubleArrayMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="double"/> 型の配列として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="double"/> 型配列表現。
        /// </returns>
        public double[] GetDoubleArray() => GetDoubleArrayMethod.Invoke(Src, null) as double[];

        private static FastMethod GetInt32ArrayMethod;
        /// <summary>
        /// 現在の要素の値を <see cref="int"/> 型の配列として取得します。
        /// </summary>
        /// <returns>
        /// 現在の要素の値の <see cref="int"/> 型配列表現。
        /// </returns>
        public int[] GetInt32Array() => GetInt32ArrayMethod.Invoke(Src, null) as int[];

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
