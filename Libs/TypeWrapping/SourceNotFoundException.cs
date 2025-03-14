using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWrapping
{
    internal class SourceNotFoundException : KeyNotFoundException
    {
        public SourceNotFoundException(string message) : base(message)
        { 
        }
    }
}
