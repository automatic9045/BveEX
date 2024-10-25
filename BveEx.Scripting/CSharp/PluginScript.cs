using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

using BveEx.PluginHost;

namespace BveEx.Scripting.CSharp
{
    public class PluginScript<TGlobals> : IPluginScript<TGlobals> where TGlobals : Globals
    {
        protected static readonly ScriptOptions ScriptOptions;

        static PluginScript()
        {
            if (typeof(TGlobals) == typeof(Globals))
            {
                ScriptOptions scriptOptions = ScriptOptions.Default
                    .AddReferences(typeof(Form).Assembly)
                    .WithImports("System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Windows.Forms");

                AddReferenceAndImports(Assembly.GetExecutingAssembly());
                AddReferenceAndImports(App.Instance.BveExPluginHostAssembly);

                ScriptOptions = scriptOptions;


                void AddReferenceAndImports(Assembly assembly) => scriptOptions = scriptOptions
                    .AddReferences(assembly)
                    .AddImports(assembly.GetTypes().Select(t => t.Namespace).Distinct().Where(n => !(n is null)));
            }
            else
            {
                ScriptOptions = PluginScript<Globals>.ScriptOptions;
            }
        }

        public string Name { get; } = null;

        private Task<ImmutableArray<Diagnostic>> CompilationTask;
        private readonly bool SkipCompile;

        protected readonly Script Script;

        protected PluginScript(Script script, string name, bool skipCompile)
        {
            Script = script;
            Name = name;
            SkipCompile = skipCompile;

            if (!SkipCompile) BeginCompile();
        }

        public PluginScript(string code, ScriptSourceResolver scriptSourceResolver, string name)
            : this(CSharpScript.Create(code, ScriptOptions.WithSourceResolver(scriptSourceResolver), typeof(TGlobals)), name, false)
        {
        }

        public PluginScript(string code, string searchBaseDirectory, string name)
            : this(code, ScriptSourceResolver.Default.WithBaseDirectory(searchBaseDirectory), name)
        {
        }

        public PluginScript(string code, string name) : this(code, ScriptSourceResolver.Default, name)
        {
        }

        protected PluginScript(Stream code, ScriptOptions scriptOptions, string name) : this(CSharpScript.Create(code, scriptOptions, typeof(TGlobals)), name, false)
        {
        }

        public static PluginScript<TGlobals> LoadFrom(string path)
        {
            (ScriptOptions scriptOptions, string name) = LoadObjectsToConstruct(path);

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return new PluginScript<TGlobals>(stream, scriptOptions, name);
            }
        }

        protected static (ScriptOptions scriptOptions, string name) LoadObjectsToConstruct(string path)
        {
            ScriptSourceResolver scriptSourceResolver = ScriptSourceResolver.Default.WithBaseDirectory(Path.GetDirectoryName(path));
            ScriptOptions scriptOptions = ScriptOptions.WithSourceResolver(scriptSourceResolver);

            return (scriptOptions, Path.GetFileName(path));
        }

        public virtual object Clone() => new PluginScript<TGlobals>(Script, Name, true);

        private void BeginCompile()
        {
            CompilationTask = Task.Run(() =>
            {
                ImmutableArray<Diagnostic> compilationErrors = Script.Compile();
                return compilationErrors;
            });
        }

        public IPluginScript<TGlobals> GetWithCheckErrors()
        {
            if (SkipCompile) return this;

            ImmutableArray<Diagnostic> compilationErrors = CompilationTask.Result;
            return compilationErrors.Any() ? throw new CompilationException(Name, compilationErrors) : this;
        }

        public IScriptResult Run(TGlobals globals)
        {
            ScriptState state = ExecuteCodeAsync(globals).Result;
            return new ScriptResult(state);
        }

        protected async Task<ScriptState> ExecuteCodeAsync(TGlobals globals)
        {
            if (!SkipCompile && !CompilationTask.IsCompleted) GetWithCheckErrors();

            ScriptState state = await Script.RunAsync(globals).ConfigureAwait(false);
            return state;
        }
    }

    public class PluginScript<TResult, TGlobals> : PluginScript<TGlobals>, IPluginScript<TResult, TGlobals> where TGlobals : Globals
    {
        protected PluginScript(Script script, string name, bool skipCompile) : base(script, name, skipCompile)
        {
        }

        public PluginScript(string code, ScriptSourceResolver scriptSourceResolver, string name) : base(code, scriptSourceResolver, name)
        {
        }

        public PluginScript(string code, string searchBaseDirectory, string name) : base(code, searchBaseDirectory, name)
        {
        }

        public PluginScript(string code, string name) : base(code, name)
        {
        }

        protected PluginScript(Stream code, ScriptOptions scriptOptions, string name) : base(code, scriptOptions, name)
        {
        }

        public static new PluginScript<TResult, TGlobals> LoadFrom(string path)
        {
            (ScriptOptions scriptOptions, string name) = LoadObjectsToConstruct(path);

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return new PluginScript<TResult, TGlobals>(stream, scriptOptions, name);
            }
        }

        public override object Clone() => new PluginScript<TResult, TGlobals>(Script, Name, true);

        public new IPluginScript<TResult, TGlobals> GetWithCheckErrors() => base.GetWithCheckErrors() as PluginScript<TResult, TGlobals>;

        public new IScriptResult<TResult> Run(TGlobals globals)
        {
            ScriptState state = ExecuteCodeAsync(globals).Result;
            return new ScriptResult<TResult>(state);
        }
    }
}
