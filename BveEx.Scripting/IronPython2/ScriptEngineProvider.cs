using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IronPython.Hosting;

using Microsoft.Scripting.Hosting;

using BveEx.PluginHost;

namespace BveEx.Scripting.IronPython2
{
    public static class ScriptEngineProvider
    {
        private static readonly IEnumerable<string> PluginHostNamespaces;

        static ScriptEngineProvider()
        {
            PluginHostNamespaces = App.Instance.BveExPluginHostAssembly.GetTypes().Select(t => t.Namespace).Distinct().Where(n => !(n is null));
        }

        public static ScriptEngine CreateEngine(ICollection<string> searchPaths)
        {
            ScriptEngine scriptEngine = Python.CreateEngine();

            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            scriptEngine.Runtime.LoadAssembly(typeof(Form).Assembly);
            scriptEngine.Runtime.LoadAssembly(App.Instance.BveExPluginHostAssembly);
            scriptEngine.Runtime.LoadAssembly(executingAssembly);

            AddImportsFromArray("System", "System.Collections.Generic", "System.Text", "System.Windows.Forms");
            AddImportsFromAssembly(App.Instance.BveExPluginHostAssembly);
            AddImportsFromAssembly(executingAssembly);

            ICollection<string> engineSearchPaths = scriptEngine.GetSearchPaths();
            foreach (string path in searchPaths) engineSearchPaths.Add(path);
            scriptEngine.SetSearchPaths(engineSearchPaths);

            return scriptEngine;


            void AddImport(string name) => scriptEngine.Execute($"import {name}");

            void AddImports(IEnumerable<string> names)
            {
                foreach (string name in names)
                {
                    AddImport(name);
                }
            }

            void AddImportsFromArray(params string[] names) => AddImports(names);

            void AddImportsFromAssembly(Assembly assembly)
                => AddImports(assembly.GetTypes().Where(t => t.IsPublic).Select(t => t.Namespace).Distinct().Where(n => !(n is null)));
        }

        public static ScriptEngine CreateEngine(params string[] searchPaths) => CreateEngine(searchPaths as ICollection<string>);

        public static ScriptScope CreateScope(ScriptEngine engine)
        {
            ScriptScope scope = engine.CreateScope();
            return scope;
        }
    }
}
