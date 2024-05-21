using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes;
using UnembeddedResources;

using AtsEx.PluginHost;

namespace AtsEx.Native
{
    internal class BveTypeSetLoader
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<BveTypeSetLoader>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> BveVersionNotSupported { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> MultipleSlimDXLoadedApproach { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static BveTypeSetLoader()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly BveTypeSetFactory Factory;

        public event EventHandler<DifferentVersionProfileLoadedEventArgs> DifferentVersionProfileLoaded;

        public BveTypeSetLoader()
        {
            Factory = new BveTypeSetFactory();
            Factory.DifferentVersionProfileLoaded += (sender, e) =>
            {
                DifferentVersionProfileLoadedEventArgs args = new DifferentVersionProfileLoadedEventArgs(e);
                DifferentVersionProfileLoaded?.Invoke(this, args);
            };
        }

        public BveTypeSet Load()
        {
            try
            {
                BveTypeSet bveTypes = Factory.Load();
                return bveTypes;
            }
            catch (MultipleSlimDXLoadedException ex)
            {
                string approach = string.Format(Resources.Value.MultipleSlimDXLoadedApproach.Value, App.Instance.ProductShortName);
                ErrorDialog.Show(3, ex.Message, approach);
                throw;
            }
            catch (Exception ex)
            {
                ExceptionResolver exceptionResolver = new ExceptionResolver();
                string senderName = Path.GetFileName(typeof(BveTypeSet).Assembly.Location);
                exceptionResolver.Resolve(senderName, ex);
                throw;
            }
        }


        internal class DifferentVersionProfileLoadedEventArgs : BveTypes.DifferentVersionProfileLoadedEventArgs
        {
            public string Message => string.Format(Resources.Value.BveVersionNotSupported.Value, BveVersion, ProfileVersion);

            public DifferentVersionProfileLoadedEventArgs(BveTypes.DifferentVersionProfileLoadedEventArgs source)
                : base(source.BveVersion, source.ProfileVersion)
            {
            }
        }
    }
}
