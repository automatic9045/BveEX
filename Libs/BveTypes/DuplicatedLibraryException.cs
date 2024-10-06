using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace BveTypes
{
    /// <summary>
    /// 同一ライブラリの複数バージョンが読み込まれたときにスローされる例外です。
    /// </summary>
    public class DuplicatedLibraryException : Exception
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<DuplicatedLibraryException>("BveTypes");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Message { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static DuplicatedLibraryException()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        /// <summary>
        /// 原因となっているアセンブリの名前を取得します。
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// 原因となっているアセンブリの一覧を取得します。
        /// </summary>
        public IReadOnlyCollection<Assembly> Assemblies { get; }

        internal DuplicatedLibraryException(string assemblyName, IEnumerable<Assembly> assemblies, Exception innerException)
            : base(string.Format(Resources.Value.Message.Value, nameof(BveTypeSet), assemblyName), innerException)
        {
            AssemblyName = assemblyName;
            Assemblies = assemblies.ToList();
        }
    }
}
