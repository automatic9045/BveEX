using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Launcher.Hosting
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
                ErrorDialog.Show(1, "BVE 本体の読込に失敗しました。Assembly.GetEntryAssembly が null を返しました。",
                    "想定されない方法で BveEX を起動しています。BVE 本体と異なるプロセス上で BveEX を実行していませんか?");
                throw new NotSupportedException();
            }
            else if (!TargetAssembly.GetTypes().Any(t => t.Namespace == "Mackoy.Bvets"))
            {
                ErrorDialog.Show(2, "同一プロセス上に BVE 本体が見つかりませんでした。",
                    "想定されない方法で BveEX を起動しています。BVE 本体と異なるプロセス上で BveEX を実行していませんか?");
                throw new NotSupportedException();
            }
        }
    }
}
