using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// AtsEX 本体の不具合によるエラーを表します。この例外をキャッチした場合は AtsEX 開発者までご連絡ください。
    /// </summary>
    public class DevelopException : Exception
    {
        public DevelopException(string message) : base(message)
        {
        }
        public DevelopException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
