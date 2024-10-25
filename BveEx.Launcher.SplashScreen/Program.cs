using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.Launcher.SplashScreen
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 3) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Version bveVersion = Version.Parse(args[0]);
            Version launcherVersion = Version.Parse(args[1]);
            string channelName = args[2];

            SplashForm form = new SplashForm(bveVersion, launcherVersion);
            SplashFormInfo formInfo = new SplashFormInfo(form);

            IpcServerChannel channel = new IpcServerChannel(channelName);
            ChannelServices.RegisterChannel(channel, true);
            RemotingServices.Marshal(formInfo, nameof(SplashFormInfo), typeof(SplashFormInfo));

            Application.Run(form);
        }
    }
}
