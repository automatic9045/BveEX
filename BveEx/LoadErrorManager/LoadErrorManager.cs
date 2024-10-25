using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.LoadErrorManager;

namespace BveEx.LoadErrorManager
{
    internal sealed class LoadErrorManager : ILoadErrorManager
    {
        private readonly LoadingProgressForm LoadingProgressForm;

        internal LoadErrorManager(LoadingProgressForm loadingProgressForm)
        {
            LoadingProgressForm = loadingProgressForm;
            Errors = new LoadErrorList(LoadingProgressForm);
        }

        public IList<LoadError> Errors { get; private set; }

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
