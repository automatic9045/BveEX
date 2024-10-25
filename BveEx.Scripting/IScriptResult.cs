using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Scripting
{
    public interface IScriptResult
    {
        T GetVariable<T>(string name);
    }

    public interface IScriptResult<TResult> : IScriptResult
    {
        TResult ReturnValue { get; }
    }
}
