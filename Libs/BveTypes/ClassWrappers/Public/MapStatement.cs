using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers.Extensions;
using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// マップ ステートメントを表します。
    /// </summary>
    public class MapStatement : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapStatement>();

            Constructor = members.GetSourceConstructor();

            LocationGetMethod = members.GetSourcePropertyGetterOf(nameof(Location));
            ClausesGetMethod = members.GetSourcePropertyGetterOf(nameof(Clauses));
            FileNameGetMethod = members.GetSourcePropertyGetterOf(nameof(FileName));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapStatement"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapStatement(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapStatement"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapStatement FromSource(object src) => src is null ? null : new MapStatement(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="MapStatement"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">ステートメントが定義されている距離程 [m]。</param>
        /// <param name="clauses">ステートメントを構成する句の一覧。</param>
        /// <param name="fileName">ステートメントが定義されているマップファイルの名前。</param>
        public MapStatement(double location, WrappedList<MapStatementClause> clauses, string fileName)
            : this(Constructor.Invoke(new object[] { location, clauses.Src, fileName }))
        {
        }

        private static FastMethod LocationGetMethod;
        /// <summary>
        /// このステートメントが定義されている距離程 [m] を取得します。
        /// </summary>
        public double Location
            => LocationGetMethod.Invoke(Src, null);

        private static FastMethod ClausesGetMethod;
        /// <summary>
        /// このステートメントを構成する句の一覧を取得します。
        /// </summary>
        public WrappedList<MapStatementClause> Clauses
            => WrappedList<MapStatementClause>.FromSource(ClausesGetMethod.Invoke(Src, null));

        private static FastMethod FileNameGetMethod;
        /// <summary>
        /// このステートメントが定義されているマップファイルの名前を取得します。
        /// </summary>
        public string FileName
            => FileNameGetMethod.Invoke(Src, null);
    }
}
