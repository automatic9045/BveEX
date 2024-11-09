using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using BveTypes.ClassWrappers.Extensions;

using BveEx.Extensions.MapStatements;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

using BveEx.Plugins;

namespace BveEx
{
    internal partial class ScenarioService
    {
        private class PluginLoader
        {
            private readonly NativeImpl Native;
            private readonly BveHacker BveHacker;
            private readonly IExtensionSet Extensions;

            public PluginLoader(NativeImpl native, BveHacker bveHacker, IExtensionSet extensions)
            {
                Native = native;
                BveHacker = bveHacker;
                Extensions = extensions;
            }

            public PluginSet Load(PluginSourceSet vehiclePluginUsing)
            {
                PluginLoadErrorResolver loadErrorResolver = new PluginLoadErrorResolver(BveHacker.LoadingProgressForm);

                PluginSet loadedPlugins = new PluginSet();
                Plugins.PluginLoader pluginLoader = new Plugins.PluginLoader(Native, BveHacker, Extensions, loadedPlugins);

                Dictionary<string, PluginBase> vehiclePlugins;
                try
                {
                    vehiclePlugins = pluginLoader.Load(vehiclePluginUsing);
                }
                catch (Exception ex)
                {
                    vehiclePlugins = new Dictionary<string, PluginBase>();
                    loadErrorResolver.Resolve(null, ex);
                }

                Dictionary<string, PluginBase> mapPlugins = new Dictionary<string, PluginBase>();
                try
                {
                    string mapDirectory = Path.GetDirectoryName(BveHacker.ScenarioInfo.RouteFiles.SelectedFile.Path);

                    {
                        IStatementSet statements = Extensions.GetExtension<IStatementSet>();
                        ClauseFilter elementFilter = ClauseFilter.Element("MapPlugin", 0);

                        IEnumerable<Statement> loadPluginUsingStatements = statements.FindOfficialStatements(elementFilter, ClauseFilter.Function("Load", 1));
                        foreach (Statement statement in loadPluginUsingStatements)
                        {
                            WrappedList<MapStatementClause> clauses = statement.Source.Clauses;
                            string pluginUsingPath = Path.Combine(mapDirectory, clauses[clauses.Count - 1].Args[0].ToString());
                            LoadMapPluginUsing(pluginUsingPath);
                        }

                        IEnumerable<Statement> loadAssemblyStatements = statements.FindOfficialStatements(elementFilter, ClauseFilter.Function("LoadAssembly", 1, 2));
                        foreach (Statement statement in loadAssemblyStatements)
                        {
                            WrappedList<MapStatementClause> clauses = statement.Source.Clauses;
                            MapStatementClause functionClause = clauses[clauses.Count - 1];
                            string assemblyPath = Path.Combine(mapDirectory, functionClause.Args[0].ToString());
                            Identifier identifier = 2 <= functionClause.Args.Count ? new Identifier(functionClause.Args[1].ToString()) : new RandomIdentifier();

                            PluginSourceSet pluginSources = new PluginSourceSet(Path.GetFileName(assemblyPath), PluginType.MapPlugin, false, new IPluginPackage[]
                            {
                                new AssemblyPluginPackage(identifier, Assembly.LoadFrom(assemblyPath)),
                            });

                            LoadMapPlugins(pluginSources);
                        }
                    }


                    void LoadMapPluginUsing(string path)
                    {
                        PluginSourceSet mapPluginUsing = PluginSourceSet.FromPluginUsing(PluginType.MapPlugin, false, path);
                        LoadMapPlugins(mapPluginUsing);
                    }

                    void LoadMapPlugins(PluginSourceSet pluginSources)
                    {
                        Dictionary<string, PluginBase> plugins = pluginLoader.Load(pluginSources);
                        foreach (KeyValuePair<string, PluginBase> plugin in plugins)
                        {
                            mapPlugins.Add(plugin.Key, plugin.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    loadErrorResolver.Resolve(null, ex);
                }

                loadedPlugins.SetPlugins(vehiclePlugins, mapPlugins);
                return loadedPlugins;
            }
        }
    }
}
