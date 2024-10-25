using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Plugins.Native
{
    internal static class LibraryProcedure
    {
        public static TDelegate Load<TDelegate>(IntPtr library, string procedureName) where TDelegate : Delegate
        {
            IntPtr procedure = Imports.GetProcAddress(library, procedureName);
            if (procedure == IntPtr.Zero) return null;

            TDelegate @delegate = Marshal.GetDelegateForFunctionPointer<TDelegate>(procedure);
            return @delegate;
        }
    }
}
