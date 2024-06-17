using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using UnembeddedResources;

using AtsEx.Extensions.ContextMenuHacker;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx
{
    internal abstract partial class AtsEx : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<AtsEx>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExAssemblyLocationIllegalMessage { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExAssemblyLocationIllegalApproach { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> IgnoreAndContinue { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ExtensionTickResultTypeInvalid { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ManualDisposeHeader { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ManualDisposeMessage { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static AtsEx()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly ExtensionService _ExtensionService;

        public BveHacker BveHacker { get; }
        public IExtensionSet Extensions { get; }

        public VersionFormProvider VersionFormProvider { get; }

        protected AtsEx(BveTypeSet bveTypes)
        {
            BveHacker = new BveHacker(bveTypes);

            ExtensionLoader extensionLoader = new ExtensionLoader(BveHacker);
            Extensions = extensionLoader.Load();
            _ExtensionService = new ExtensionService(Extensions);

            VersionFormProvider = CreateVersionFormProvider(Extensions);
        }

        private VersionFormProvider CreateVersionFormProvider(IEnumerable<PluginBase> extensions)
            => new VersionFormProvider(BveHacker.MainFormSource, extensions, Extensions.GetExtension<IContextMenuHacker>());

        public virtual void Dispose()
        {
            VersionFormProvider.Dispose();
            _ExtensionService.Dispose();
            BveHacker.Dispose();
        }

        public void Tick(TimeSpan elapsed)
        {
            _ExtensionService.Tick(elapsed);
        }
    }
}