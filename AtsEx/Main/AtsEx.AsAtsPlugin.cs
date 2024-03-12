using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes;

using AtsEx.PluginHost;

namespace AtsEx
{
    internal abstract partial class AtsEx
    {
        internal sealed class AsAtsPlugin : AtsEx
        {
            private const string LegalLauncherAssemblyRelativeLocation = @"AtsEx\AtsEx.Launcher.dll";

            public AsAtsPlugin(BveTypeSet bveTypes) : base(bveTypes)
            {
                CheckAtsExAssemblyLocation();
            }

            private void CheckAtsExAssemblyLocation()
            {
                string launcherAssemblyLocation = App.Instance.AtsExLauncherAssembly.Location;

                string scenarioDirectory = BveHacker.ScenarioInfo.DirectoryName;
                string legalLauncherAssemblyLocation = Path.Combine(scenarioDirectory, LegalLauncherAssemblyRelativeLocation);

                if (GetNormalizedPath(launcherAssemblyLocation) != GetNormalizedPath(legalLauncherAssemblyLocation))
                {
                    string message = string.Format(Resources.Value.AtsExAssemblyLocationIllegalMessage.Value, App.Instance.ProductShortName, launcherAssemblyLocation);
                    string approach = string.Format(Resources.Value.AtsExAssemblyLocationIllegalApproach.Value, App.Instance.ProductShortName, legalLauncherAssemblyLocation);
                    string sender = Path.GetFileName(launcherAssemblyLocation);

                    BveHacker.LoadErrorManager.Throw(message, sender);
                    ErrorDialog.Show(1, message, approach);

                    if (MessageBox.Show(Resources.Value.IgnoreAndContinue.Value, App.Instance.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        Environment.Exit(0);
                    }
                }


                string GetNormalizedPath(string abdolutePath) => Path.GetFullPath(abdolutePath).ToLower();
            }
        }
    }
}
