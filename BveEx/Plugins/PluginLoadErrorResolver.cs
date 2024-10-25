using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using UnembeddedResources;

using AtsEx.Diagnostics;
using AtsEx.PluginHost;
using AtsEx.PluginHost.LoadErrorManager;
using AtsEx.Scripting.CSharp;

namespace AtsEx.Plugins
{
    internal class PluginLoadErrorResolver : WrapperExceptionExtractor
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginLoadErrorResolver>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> UnhandledExceptionCaption { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static PluginLoadErrorResolver()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly ILoadErrorManager LoadErrorManager;

        public PluginLoadErrorResolver(ILoadErrorManager loadErrorManager)
        {
            LoadErrorManager = loadErrorManager;
        }

        protected override void Throw(string senderName, Exception exception, bool isWrapperException)
        {
            switch (exception)
            {
                case CompilationException ex:
                    foreach (Diagnostic error in ex.CompilationErrors)
                    {
                        LinePosition linePosition = error.Location.GetLineSpan().StartLinePosition;
                        LoadErrorManager.Throw(error.GetMessage(System.Globalization.CultureInfo.CurrentUICulture), ex.SenderName, linePosition.Line, linePosition.Character + 1);
                    }
                    break;

                case BveFileLoadException ex:
                    LoadErrorManager.Throw(ex.Message, ex.SenderFileName, ex.LineIndex, ex.CharIndex);
                    break;

                default:
                    if (!isWrapperException) MessageBox.Show(exception.ToString(), string.Format(Resources.Value.UnhandledExceptionCaption.Value, App.Instance.ProductShortName));
                    LoadErrorManager.Throw(exception.Message, senderName);
                    break;
            }
        }
    }
}
