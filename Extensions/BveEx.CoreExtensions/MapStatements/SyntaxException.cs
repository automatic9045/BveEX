using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;

namespace AtsEx.Extensions.MapStatements
{
    /// <summary>
    /// AtsEX マップステートメントの文法エラーによる例外を表します。
    /// </summary>
    public class SyntaxException : BveFileLoadException
    {
        /// <summary>
        /// 標準のエラーメッセージを取得します。
        /// </summary>
        public static readonly string DefaultMessage = $"Syntax error. ({App.Instance.ProductShortName})";

        /// <summary>
        /// 例外の原因となっているステートメントを取得します。
        /// </summary>
        public Statement Statement { get; }

        /// <summary>
        /// <see cref="SyntaxException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="statement">例外の原因となっているステートメント。</param>
        /// <param name="message">例外の概要を表すメッセージ。</param>
        public SyntaxException(Statement statement, string message)
            : base(message, statement?.Source.FileName, GetPosition(statement).LineIndex, GetPosition(statement).CharIndex)
        {
            Statement = statement;
        }

        /// <summary>
        /// 内部例外が存在する <see cref="SyntaxException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="statement">例外の原因となっているステートメント。</param>
        /// <param name="innerException">内部例外。</param>
        public SyntaxException(Statement statement, Exception innerException)
            : base(innerException, statement.Source.FileName, GetPosition(statement).LineIndex, GetPosition(statement).CharIndex)
        {
            Statement = statement;
        }

        /// <summary>
        /// 標準のエラーメッセージを使用して、<see cref="SyntaxException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="statement">例外の原因となっているステートメント。</param>
        public SyntaxException(Statement statement) : this(statement, DefaultMessage)
        {
        }

        /// <summary>
        /// 原因となるステートメントを指定せずに、<see cref="SyntaxException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SyntaxException() : this(null)
        {
        }

        private static (int LineIndex, int CharIndex) GetPosition(Statement statement)
        {
            if (statement is null) return (0, 0);

            IList<MapStatementClause> clauses = statement.Source.Clauses;
            return 0 < clauses.Count ? (clauses[0].LineIndex, clauses[0].CharIndex) : (0, 0);
        }
    }
}
