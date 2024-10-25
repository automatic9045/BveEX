using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost
{
    public class BveFileLoadException : Exception
    {
        public string SenderFileName { get; } = null;
        public int LineIndex { get; } = 0;
        public int CharIndex { get; } = 0;

        public BveFileLoadException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public BveFileLoadException(Exception innerException, string senderFileName) : this(innerException)
        {
            SenderFileName = senderFileName;
        }

        public BveFileLoadException(Exception innerException, string senderFileName, int lineIndex) : this(innerException, senderFileName)
        {
            LineIndex = lineIndex;
        }

        public BveFileLoadException(Exception innerException, string senderFileName, int lineIndex, int charIndex) : this(innerException, senderFileName, lineIndex)
        {
            CharIndex = charIndex;
        }

        public BveFileLoadException(string message) : base(message)
        {
        }

        public BveFileLoadException(string message, string senderFileName) : this(message)
        {
            SenderFileName = senderFileName;
        }

        public BveFileLoadException(string message, string senderFileName, int lineIndex) : this(message, senderFileName)
        {
            LineIndex = lineIndex;
        }

        public BveFileLoadException(string message, string senderFileName, int lineIndex, int charIndex) : this(message, senderFileName, lineIndex)
        {
            CharIndex = charIndex;
        }
    }
}
