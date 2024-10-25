using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Scripting
{
    public interface IPluginScript<TGlobals> : ICloneable where TGlobals : Globals
    {
        IPluginScript<TGlobals> GetWithCheckErrors();
        IScriptResult Run(TGlobals globals);
    }

    public interface IPluginScript<TResult, TGlobals> : IPluginScript<TGlobals> where TGlobals : Globals
    {
        new IPluginScript<TResult, TGlobals> GetWithCheckErrors();
        new IScriptResult<TResult> Run(TGlobals globals);
    }
}
