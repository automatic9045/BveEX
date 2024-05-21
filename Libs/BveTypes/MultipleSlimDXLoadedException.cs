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
    /// 複数のバージョンの SlimDX が読み込まれたときにスローされる例外です。
    /// </summary>
    public class MultipleSlimDXLoadedException : Exception
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<MultipleSlimDXLoadedException>("BveTypes");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Message { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static MultipleSlimDXLoadedException()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        /// <summary>
        /// 読み込まれている SlimDX のアセンブリの一覧を取得します。
        /// </summary>
        public IReadOnlyCollection<Assembly> SlimDXAssemblies { get; }

        internal MultipleSlimDXLoadedException(IEnumerable<Assembly> slimDXAssemblies, Exception innerException)
            : base(string.Format(Resources.Value.Message.Value, nameof(BveTypeSet)), innerException)
        {
            SlimDXAssemblies = slimDXAssemblies.ToList();
        }
    }
}
