using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using BveEx.PluginHost;

namespace BveEx
{
    internal static class ErrorDialog
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType(typeof(ErrorDialog), "Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Header { get; private set; }
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
            string code = $"E-{id}";

            string sender = App.IsInitialized ? App.Instance.ProductShortName : null;
            string header = sender is null ? null : string.Format(Resources.Value.Header.Value, sender, code);
            string messageWithCode = $"{Resources.Value.ErrorCode.Value} {code}\n{message}";
            Diagnostics.ErrorDialogInfo errorDialogInfo = new Diagnostics.ErrorDialogInfo(header, sender, messageWithCode)
            {
                Approach = approach,
                HelpLink = new Uri($"https://www.okaoka-depot.com/AtsEX.Docs/support/errors/#{code}"),
            };

            Diagnostics.ErrorDialog.Show(errorDialogInfo);
        }
    }
}
