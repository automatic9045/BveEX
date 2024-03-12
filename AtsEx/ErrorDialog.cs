using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.PluginHost;

namespace AtsEx
{
    internal static class ErrorDialog
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(ErrorDialog), "Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ErrorCode { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static ErrorDialog()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        public static void Show(int id, string message, string approach = null)
        {
            string errorCode = $"E-{id}";
            string sender = App.IsInitialized ? App.Instance.ProductName : null;
            Diagnostics.ErrorDialog.Show($"{Resources.Value.ErrorCode.Value} {errorCode}\n{message}", sender, approach, $"https://www.okaoka-depot.com/AtsEX.Docs/support/errors/#{errorCode}");
        }
    }
}
