using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.LoadErrorManager
{
    [Plugin(PluginType.Extension)]
    [ExtensionMainDisplayType(typeof(ILoadErrorManager))]
    internal class LoadErrorManager : AssemblyPluginBase, ILoadErrorManager
    {
        private readonly LoadingProgressForm LoadingProgressForm;

        public IList<LoadError> Errors { get; private set; }

        public LoadErrorManager(PluginBuilder builder) : base(builder)
        {
            LoadingProgressForm = BveHacker.LoadingProgressForm;
            Errors = new LoadErrorList(LoadingProgressForm);
        }

        public override void Dispose()
        {
        }

        public override void Tick(TimeSpan elapsed)
        {
        }

        public void Throw(string text, string senderFileName, int lineIndex, int charIndex)
        {
            LoadingProgressForm.ThrowError(text, senderFileName, lineIndex, charIndex);
        }

        public void Throw(string text, string senderFileName, int lineIndex) => Throw(text, senderFileName, lineIndex, 0);

        public void Throw(string text, string senderFileName) => Throw(text, senderFileName, 0);

        public void Throw(string text) => Throw(text, "");

        public void Throw(LoadError error)
        {
            LoadingProgressForm.ThrowError(error);
        }
    }
}
