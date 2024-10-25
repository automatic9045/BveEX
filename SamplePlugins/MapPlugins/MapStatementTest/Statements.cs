using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

using BveEx.Extensions.MapStatements;
using BveEx.PluginHost;

namespace BveEx.Samples.MapPlugins.MapStatementTest
{
    internal static class Statements
    {
        public static void Load(IStatementSet mapStatements)
        {
            ClauseFilter[] clauseFilters = new ClauseFilter[]
            {
                new ClauseFilter(nameof(MapStatementTest), ClauseType.Element),
                new ClauseFilter("Greetings", ClauseType.Element),
                new ClauseFilter("Put", ClauseType.Function),
            };
            IEnumerable<Statement> statements = mapStatements.FindUserStatements("Automatic9045", clauseFilters);

            foreach (Statement statement in statements)
            {
                string fileName = statement.Source.FileName;
                double location = statement.Source.Location;

                IList<MapStatementClause> clauses = statement.Source.Clauses;

                MapStatementClause element = clauses[clauses.Count - 2];
                if (2 < element.Keys.Count) throw new BveFileLoadException($"キーの長さが不正です。", fileName, element.LineIndex, element.CharIndex);

                MapStatementClause function = statement.Source.Clauses[statement.Source.Clauses.Count - 1];
                if (2 < function.Args.Count) throw new BveFileLoadException($"引数の長さが不正です。", fileName, function.LineIndex, function.CharIndex);

                MessageBox.Show($"定義位置: '{Path.GetFileName(fileName)}', 行 {function.LineIndex}\n距離程: {location} m\n\n" +
                    $"キー 1: {(0 < element.Keys.Count ? ToString(element.Keys[0]) : "(なし)")}\n" +
                    $"キー 2: {(1 < element.Keys.Count ? ToString(element.Keys[1]) : "(なし)")}\n" +
                    $"引数 1: {(0 < function.Args.Count ? ToString(function.Args[0]) : "(なし)")}\n" +
                    $"引数 2: {(1 < function.Args.Count ? ToString(function.Args[1]) : "(なし)")}");


                string ToString(object value) => value is null ? "(null)" : value is string ? $"\"{value}\"" : value.ToString();
            }
        }
    }
}
