using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx
{
    internal partial class AtsEx
    {
        internal class ExtensionService : IDisposable
        {
            private readonly IExtensionSet Extensions;

            public ExtensionService(IExtensionSet extensions)
            {
                Extensions = extensions;
            }

            public void Dispose()
            {
                foreach (PluginBase extension in Extensions)
                {
                    extension.Dispose();
                }
            }

            public void Tick(TimeSpan elapsed)
            {
                foreach (PluginBase extension in Extensions)
                {
                    TickResult tickResult = extension.Tick(elapsed);
                    if (!(tickResult is ExtensionTickResult))
                    {
                        throw new InvalidOperationException(string.Format(Resources.Value.ExtensionTickResultTypeInvalid.Value,
                           $"{nameof(PluginBase)}.{nameof(PluginBase.Tick)}", nameof(ExtensionTickResult)));
                    }
                }
            }
        }
    }
}
