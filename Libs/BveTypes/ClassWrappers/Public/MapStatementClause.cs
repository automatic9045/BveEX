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
    /// マップ ステートメントの 1 つの句 (マップ要素、関数など) を表します。
    /// </summary>
    public class MapStatementClause : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapStatementClause>();

            Constructor = members.GetSourceConstructor();

            NameGetMethod = members.GetSourcePropertyGetterOf(nameof(Name));
            NameSetMethod = members.GetSourcePropertySetterOf(nameof(Name));

            ArgsGetMethod = members.GetSourcePropertyGetterOf(nameof(Args));
            ArgsSetMethod = members.GetSourcePropertySetterOf(nameof(Args));

            KeysGetMethod = members.GetSourcePropertyGetterOf(nameof(Keys));
            KeysSetMethod = members.GetSourcePropertySetterOf(nameof(Keys));

            LineIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(LineIndex));
            LineIndexSetMethod = members.GetSourcePropertySetterOf(nameof(LineIndex));

            CharIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(CharIndex));
            CharIndexSetMethod = members.GetSourcePropertySetterOf(nameof(CharIndex));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapStatementClause"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapStatementClause(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapStatementClause"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapStatementClause FromSource(object src) => src is null ? null : new MapStatementClause(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="MapStatementClause"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">句の名前。</param>
        /// <param name="lineIndex">句が存在する行番号。</param>
        /// <param name="charIndex">句が存在する列番号。</param>
        public MapStatementClause(string name, int lineIndex, int charIndex) : this(Constructor.Invoke(new object[] { name, lineIndex, charIndex }))
        {
        }

        private static FastMethod NameGetMethod;
        private static FastMethod NameSetMethod;
        /// <summary>
        /// この句の名前を取得・設定します。
        /// </summary>
        public string Name
        {
            get => NameGetMethod.Invoke(Src, null);
            set => NameSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ArgsGetMethod;
        private static FastMethod ArgsSetMethod;
        /// <summary>
        /// この句が関数であるとき、引数の一覧を取得・設定します。
        /// </summary>
        public List<object> Args
        {
            get => ArgsGetMethod.Invoke(Src, null);
            set => ArgsSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod KeysGetMethod;
        private static FastMethod KeysSetMethod;
        /// <summary>
        /// この句がマップ要素であるとき、キーの一覧を取得・設定します。
        /// </summary>
        public List<object> Keys
        {
            get => KeysGetMethod.Invoke(Src, null);
            set => KeysSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod LineIndexGetMethod;
        private static FastMethod LineIndexSetMethod;
        /// <summary>
        /// この句が存在する行番号を取得・設定します。
        /// </summary>
        public int LineIndex
        {
            get => LineIndexGetMethod.Invoke(Src, null);
            set => LineIndexSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CharIndexGetMethod;
        private static FastMethod CharIndexSetMethod;
        /// <summary>
        /// この句が存在する列番号を取得・設定します。
        /// </summary>
        public int CharIndex
        {
            get => CharIndexGetMethod.Invoke(Src, null);
            set => CharIndexSetMethod.Invoke(Src, new object[] { value });
        }


        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(Name);

            foreach (object key in Keys)
            {
                stringBuilder.Append($"[{ParseToString(key)}]");
            }

            if (0 < Args.Count)
            {
                IEnumerable<string> argTexts = Args.Select(ParseToString);
                stringBuilder.Append($"({string.Join(", ", argTexts)})");
            }

            return stringBuilder.ToString();


            string ParseToString(object obj) => obj is null ? "null" : obj is string ? $"'{obj}'" : obj.ToString();
        }
    }
}
