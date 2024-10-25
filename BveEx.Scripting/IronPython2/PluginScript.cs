using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Scripting.Hosting;

namespace AtsEx.Scripting.IronPython2
{
    public class PluginScript<TGlobals> : IPluginScript<TGlobals> where TGlobals : Globals
    {
        public string Name { get; } = null;

        private Task<CompiledCode> CompilationTask;
        private readonly bool SkipCompile;

        protected readonly ScriptScope Scope;
        protected readonly ScriptSource Source;
        protected CompiledCode CompiledCode;

        protected PluginScript(ScriptSource source, ScriptScope scope, string name)
        {
            Source = source;
            Scope = scope;
            Name = name;
            
            SkipCompile = false;
            BeginCompile();
        }

        protected PluginScript(CompiledCode compiledCode, ScriptScope scope, string name)
        {
            CompiledCode = compiledCode;
            Scope = scope;
            Name = name;

            SkipCompile = true;
        }

        public PluginScript(string code, ScriptEngine engine, ScriptScope scope, string name) : this(engine.CreateScriptSourceFromString(code), scope, name)
        {
        }

        public static PluginScript<TGlobals> LoadFrom(string path, ScriptEngine engine, ScriptScope scope)
        {
            ScriptSource source = LoadSource(path, engine);
            return new PluginScript<TGlobals>(source, scope, Path.GetFileName(path));
        }

        protected static ScriptSource LoadSource(string path, ScriptEngine engine) => engine.CreateScriptSourceFromFile(path, Encoding.UTF8);

        public virtual object Clone()
        {
            _ = GetWithCheckErrors();
            return new PluginScript<TGlobals>(CompiledCode, Scope, Name);
        }

        private void BeginCompile()
        {
            CompilationTask = Task.Run(() =>
            {
                CompiledCode compiledCode = Source.Compile();
                return compiledCode;
            });
        }

        public IPluginScript<TGlobals> GetWithCheckErrors()
        {
            if (SkipCompile) return this;

            try
            {
                CompiledCode = CompilationTask.Result;
                return this;
            }
            catch (AggregateException ex)
            {
                throw ex.GetBaseException();
            }
        }

        public IScriptResult Run(TGlobals globals)
        {
            ExecuteCode(globals);
            return new ScriptResult(Scope);
        }

        protected void ExecuteCode(TGlobals globals)
        {
            if (!CompilationTask.IsCompleted) GetWithCheckErrors();

            Scope.SetVariable("g", globals);
            CompiledCode.Execute(Scope);
        }
    }

    public class PluginScript<TResult, TGlobals> : PluginScript<TGlobals>, IPluginScript<TResult, TGlobals> where TGlobals : Globals
    {
        protected PluginScript(ScriptSource source, ScriptScope scope, string name) : base(source, scope, name)
        {
        }

        protected PluginScript(CompiledCode compiledCode, ScriptScope scope, string name) : base(compiledCode, scope, name)
        {
        }

        public PluginScript(string code, ScriptEngine engine, ScriptScope scope, string name) : base(engine.CreateScriptSourceFromString(code), scope, name)
        {
        }

        public static new PluginScript<TResult, TGlobals> LoadFrom(string path, ScriptEngine engine, ScriptScope scope)
        {
            ScriptSource source = LoadSource(path, engine);
            return new PluginScript<TResult, TGlobals>(source, scope, Path.GetFileName(path));
        }

        public override object Clone()
        {
            _ = GetWithCheckErrors();
            return new PluginScript<TResult, TGlobals>(CompiledCode, Scope, Name);
        }

        public new IPluginScript<TResult, TGlobals> GetWithCheckErrors() => base.GetWithCheckErrors() as PluginScript<TResult, TGlobals>;

        public new IScriptResult<TResult> Run(TGlobals globals)
        {
            ExecuteCode(globals);
            return new ScriptResult<TResult>(Scope);
        }
    }
}
