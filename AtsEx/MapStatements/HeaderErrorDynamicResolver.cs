using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.Plugins;
using AtsEx.PluginHost.LoadErrorManager;

namespace AtsEx.MapStatements
{
    internal class HeaderErrorDynamicResolver
    {
        private readonly ILoadErrorManager LoadErrorManager;
        private readonly IEnumerable<Header> Headers;

        public HeaderErrorDynamicResolver(ILoadErrorManager loadErrorManager, IEnumerable<Header> headers)
        {
            LoadErrorManager = loadErrorManager;
            Headers = headers;
        }

        public void Resolve()
        {
            PluginLoadErrorResolver loadErrorResolver = new PluginLoadErrorResolver(LoadErrorManager);

            try
            {
                IEnumerable<LoadError> removeTargetErrors = LoadErrorManager.Errors.Where(error =>
                {
                    bool isTargetError = Headers.Any(header
                        => Path.GetFileName(header.DefinedFilePath).ToLowerInvariant() == error.SenderFileName.ToLowerInvariant() && header.LineIndex == error.LineIndex && header.CharIndex == error.CharIndex);
                    return isTargetError;
                });
                foreach (LoadError error in removeTargetErrors)
                {
                    LoadErrorManager.Errors.Remove(error);
                }
            }
            catch (Exception ex)
            {
                loadErrorResolver.Resolve(null, ex);
            }
        }
    }
}
