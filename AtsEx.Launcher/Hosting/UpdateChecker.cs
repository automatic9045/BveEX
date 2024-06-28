using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Markdig;

using AtsEx.Launcher.Properties;

namespace AtsEx.Launcher.Hosting
{
    internal static class UpdateChecker
    {
        public static void CheckUpdates()
        {
            DateTime stoppedUpdateNotificationOn = DateTime.MinValue;
            try
            {
                stoppedUpdateNotificationOn = Settings.Default.StoppedUpdateNotificationOn;
            }
            catch { }

            DateTime now = DateTime.Now;
            if (now < stoppedUpdateNotificationOn)
            {
                Settings.Default.StoppedUpdateNotificationOn = DateTime.MinValue;
                Settings.Default.Save();
            }
            else
            {
                if (now - stoppedUpdateNotificationOn < new TimeSpan(12, 0, 0)) return;
            }

            try
            {
                AtsExRepositoryHost repositoryHost = new AtsExRepositoryHost();

                ReleaseInfo latestRelease = repositoryHost.GetLatestReleaseAsync().Result;
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                if (currentVersion < latestRelease.Version)
                {
                    UpdateInfoDialog dialog = new UpdateInfoDialog(currentVersion, latestRelease.Version, GetUpdateDetailsHtmlAsync().Result);

                    DialogResult confirm = dialog.ShowDialog();
                    if (confirm == DialogResult.OK)
                    {
                        Process.Start("https://www.okaoka-depot.com/AtsEX.Docs/download/");
                    }

                    if (dialog.DoNotShowAgain)
                    {
                        Settings.Default.StoppedUpdateNotificationOn = now;
                        Settings.Default.Save();
                    }


                    async Task<string> GetUpdateDetailsHtmlAsync()
                        => await Task.Run(() =>
                        {
                            string updateDetailsMarkdown = "【詳細の取得に失敗しました】";
                            try
                            {
                                updateDetailsMarkdown = latestRelease.GetUpdateDetails();
                            }
                            catch { }

                            string updateDetailsHtml = Markdown.ToHtml(updateDetailsMarkdown);
                            return updateDetailsHtml;
                        }).ConfigureAwait(false);
                }
            }
            catch { }
        }
    }
}
