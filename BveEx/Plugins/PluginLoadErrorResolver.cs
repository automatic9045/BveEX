using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using BveTypes.ClassWrappers;
using UnembeddedResources;

using BveEx.Diagnostics;
using BveEx.PluginHost;
using BveEx.Scripting.CSharp;

namespace BveEx.Plugins
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

        private readonly LoadingProgressForm LoadingProgressForm;

        public PluginLoadErrorResolver(LoadingProgressForm loadingProgressForm)
        {
            LoadingProgressForm = loadingProgressForm;
        }

        protected override void Throw(string senderName, Exception exception, bool isWrapperException)
        {
            switch (exception)
            {
                case CompilationException ex:
                    foreach (Diagnostic error in ex.CompilationErrors)
                    {
                        LinePosition linePosition = error.Location.GetLineSpan().StartLinePosition;
                        LoadingProgressForm.ThrowError(error.GetMessage(System.Globalization.CultureInfo.CurrentUICulture), ex.SenderName, linePosition.Line, linePosition.Character + 1);
                    }
                    break;

                case BveFileLoadException ex:
                    LoadingProgressForm.ThrowError(ex.Message, ex.SenderFileName, ex.LineIndex, ex.CharIndex);
                    break;

                default:
                    if (!isWrapperException) MessageBox.Show(exception.ToString(), string.Format(Resources.Value.UnhandledExceptionCaption.Value, App.Instance.ProductShortName));
                    LoadingProgressForm.ThrowError(exception.Message, senderName, 0, 0);
                    break;
            }
        }
    }
}
