using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Diagnostics;

namespace AtsEx.Launcher.Hosting
{
    internal class TargetBveFinder
    {
        public Process TargetProcess { get; }
        public AppDomain TargetAppDomain { get; }
        public Assembly TargetAssembly { get; }

        public TargetBveFinder()
        {
            TargetProcess = Process.GetCurrentProcess();
            TargetAppDomain = AppDomain.CurrentDomain;
            TargetAssembly = Assembly.GetEntryAssembly();

            CheckAssembly();
        }

        private void CheckAssembly()
        {
            if (TargetAssembly is null)
            {
                ShowErrorDialog("BVE 本体の読込に失敗しました。Assembly.GetEntryAssembly が null を返しました。",
                    "想定されない方法で AtsEX を起動しています。BVE 本体と異なるプロセス上で AtsEX を実行していませんか?");
            }
            else if (!TargetAssembly.GetTypes().Any(t => t.Namespace == "Mackoy.Bvets"))
            {
                ShowErrorDialog("同一プロセス上に BVE 本体が見つかりませんでした。",
                    "想定されない方法で AtsEX を起動しています。BVE 本体と異なるプロセス上で AtsEX を実行していませんか?");
            }


            void ShowErrorDialog(string message, string approach)
            {
                ErrorDialog.Show(message, "AtsEX Launcher", approach);
                throw new NotSupportedException(message);
            }
        }
    }
}
