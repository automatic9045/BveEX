using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AtsEx.PluginHost;
using AtsEx.PluginHost.MapStatements;

namespace AtsEx.Samples.MapPlugins.MapStatementTest
{
    internal static class Headers
    {
        private static readonly string AssemblyFileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);

        private static readonly Namespace Namespace = Namespace.GetUserNamespace("Automatic9045").Child("Alert");
        private static readonly Identifier HeaderAlert = new Identifier(Namespace, "HeaderAlert");

        public static void Load(IHeaderSet mapHeaders)
        {
            IReadOnlyList<IHeader> headers = mapHeaders.GetAll(HeaderAlert);
            for (int i = 0; i < headers.Count; i++)
            {
                IHeader header = headers[i];
                MessageBox.Show($"ヘッダー {header.Name.FullName} が読み込まれました ({i}):\n\n{header.Argument}");
            }

            if (!headers.Any())
            {
                throw new BveFileLoadException($"ヘッダー {HeaderAlert.FullName} が見つかりませんでした。", AssemblyFileName);
            }
        }
    }
}
