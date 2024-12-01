using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using BveTypes.ClassWrappers.Extensions;

using BveEx.Extensions.MapStatements;

namespace BveEx.Plugins
{
    internal class MapListener : IDisposable
    {
        private static readonly ClauseFilter RootFilter = ClauseFilter.Element("MapPlugin", 0);
        private static readonly ClauseFilter[] LoadFilters = new ClauseFilter[] { RootFilter, ClauseFilter.Function("Load", 1) };
        private static readonly ClauseFilter[] LoadAssemblyFilters = new ClauseFilter[] { RootFilter, ClauseFilter.Function("LoadAssembly", 1, 2) };


        private readonly IStatementSet Statements;

        public event EventHandler<LoadRequestedEventArgs> LoadRequested;
        public event EventHandler<LoadAssemblyRequestedEventArgs> LoadAssemblyRequested;

        private MapListener(IStatementSet statements)
        {
            Statements = statements;
            Statements.StatementLoaded += OnStatementLoaded;
        }

        public static MapListener Listen(IStatementSet statements)
        {
            return new MapListener(statements);
        }

        public void Dispose()
        {
            Statements.StatementLoaded -= OnStatementLoaded;
        }

        private void OnStatementLoaded(object sender, StatementLoadedEventArgs e)
        {
            if (e.Statement.IsOfficialStatement(LoadFilters))
            {
                string mapDirectory = Path.GetDirectoryName(e.Statement.Source.FileName);
                WrappedList<MapStatementClause> clauses = e.Statement.Source.Clauses;

                string pluginUsingPath = Path.Combine(mapDirectory, clauses[clauses.Count - 1].Args[0].ToString());

                LoadRequested?.Invoke(this, new LoadRequestedEventArgs(pluginUsingPath));
            }
            else if (e.Statement.IsOfficialStatement(LoadAssemblyFilters))
            {
                string mapDirectory = Path.GetDirectoryName(e.Statement.Source.FileName);
                WrappedList<MapStatementClause> clauses = e.Statement.Source.Clauses;
                MapStatementClause functionClause = clauses[clauses.Count - 1];

                string assemblyPath = Path.Combine(mapDirectory, functionClause.Args[0].ToString());
                Identifier identifier = 2 <= functionClause.Args.Count ? new Identifier(functionClause.Args[1].ToString()) : new RandomIdentifier();

                LoadAssemblyRequested?.Invoke(this, new LoadAssemblyRequestedEventArgs(assemblyPath, identifier));
            }
        }


        internal class LoadRequestedEventArgs : EventArgs
        {
            public string PluginUsingPath { get; }

            public LoadRequestedEventArgs(string pluginUsingPath)
            {
                PluginUsingPath = pluginUsingPath;
            }
        }

        internal class LoadAssemblyRequestedEventArgs : EventArgs
        {
            public string AssemblyPath { get; }
            public Identifier Identifier { get; }

            public LoadAssemblyRequestedEventArgs(string assemblyPath, Identifier identifier)
            {
                AssemblyPath = assemblyPath;
                Identifier = identifier;
            }
        }
    }
}
