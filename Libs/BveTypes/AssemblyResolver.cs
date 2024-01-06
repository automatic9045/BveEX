using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes
{
    internal static class AssemblyResolver
    {
        public static T WithResolve<T>(Func<T> func, params string[] targetAssemblyNames)
        {
            Dictionary<string, Assembly> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .AsParallel()
                .Select(asm => new KeyValuePair<string, Assembly>(asm.GetName().Name, asm))
                .Distinct(new EqualityComparer())
                .ToDictionary(x => x.Key, x => x.Value);

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            T result = func();
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;

            return result;


            Assembly AssemblyResolve(object sender, ResolveEventArgs e)
            {
                AssemblyName assemblyName = new AssemblyName(e.Name);
                foreach (string name in targetAssemblyNames)
                {
                    if (assemblyName.Name != name) continue;
                    if (!loadedAssemblies.TryGetValue(name, out Assembly assembly)) continue;

                    return assembly;
                }

                return null;
            }
        }


        private class EqualityComparer : IEqualityComparer<KeyValuePair<string, Assembly>>
        {
            public bool Equals(KeyValuePair<string, Assembly> x, KeyValuePair<string, Assembly> y) => x.Key == y.Key;
            public int GetHashCode(KeyValuePair<string, Assembly> obj) => obj.Key.GetHashCode();
        }
    }
}
