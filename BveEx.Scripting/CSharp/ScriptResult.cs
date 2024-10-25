using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace BveEx.Scripting.CSharp
{
    internal class ScriptResult : IScriptResult
    {
        protected readonly ScriptState State;

        public ScriptResult(ScriptState state)
        {
            State = state;
        }

        public T GetVariable<T>(string name) => (T)State.GetVariable(name).Value;
    }

    internal class ScriptResult<TResult> : ScriptResult, IScriptResult<TResult>
    {
        public TResult ReturnValue => (TResult)State.ReturnValue;

        public ScriptResult(ScriptState state) : base(state)
        {
        }
    }
}
