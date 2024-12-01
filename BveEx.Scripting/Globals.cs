using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost;

namespace BveEx.Scripting
{
    public class Globals
    {
        public IBveHacker BveHacker { get; }

        protected Dictionary<string, dynamic> Variables;

        private Globals(IBveHacker bveHacker, Dictionary<string, dynamic> variables)
        {
            BveHacker = bveHacker;

            Variables = variables;
        }

        protected Globals(Globals source) : this(source.BveHacker, source.Variables)
        {
        }

        public Globals(IBveHacker bveHacker) : this(bveHacker, new Dictionary<string, dynamic>())
        {
        }

        public T GetVariable<T>(string name) => (T)Variables[name];

        public void SetVariable<T>(string name, T value) => Variables[name] = value;
    }
}
