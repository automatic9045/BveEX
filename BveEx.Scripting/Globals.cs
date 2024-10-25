using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost;

namespace AtsEx.Scripting
{
    public class Globals
    {
        public INative Native { get; }
        public IBveHacker BveHacker { get; }

        protected Dictionary<string, dynamic> Variables;

        private Globals(INative native, IBveHacker bveHacker, Dictionary<string, dynamic> variables)
        {
            Native = native;
            BveHacker = bveHacker;

            Variables = variables;
        }

        protected Globals(Globals source) : this(source.Native, source.BveHacker, source.Variables)
        {
        }

        public Globals(INative native, IBveHacker bveHacker) : this(native, bveHacker, new Dictionary<string, dynamic>())
        {
        }

        public T GetVariable<T>(string name) => (T)Variables[name];

        public void SetVariable<T>(string name, T value) => Variables[name] = value;
    }
}
