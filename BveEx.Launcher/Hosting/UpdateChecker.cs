using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Markdig;

using BveEx.Launcher.Properties;

namespace BveEx.Launcher.Hosting
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
                BveExRepositoryHost repositoryHost = new BveExRepositoryHost();

                ReleaseInfo latestRelease = repositoryHost.GetLatestReleaseAsync().Result;
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                if (currentVersion < latestRelease.Version)
                {
                    UpdateInfoDialog dialog = new UpdateInfoDialog(currentVersion, latestRelease.Version, GetUpdateDetailsHtmlAsync().Result);

                    DialogResult confirm = dialog.ShowDialog();
                    if (confirm == DialogResult.OK)
                    {
                        Process.Start("https://bveex.okaoka-depot.com/download");
                    }

                    if (dialog.DoNotShowAgain)
                    {
                        Settings.Default.StoppedUpdateNotificationOn = now;
                        Settings.Default.Save();
                    }


                    async Task<string> GetUpdateDetailsHtmlAsync()
                        => await Task.Run(() =>
                        {
                            string updateDetailsHtml = "【詳細の取得に失敗しました】";
                            try
                            {
                                string updateDetailsMarkdown = latestRelease.GetUpdateDetails();
                                updateDetailsHtml = Markdown.ToHtml(updateDetailsMarkdown);
                            }
                            catch { }

                            return updateDetailsHtml;
                        }).ConfigureAwait(false);
                }
            }
            catch { }
        }
    }
}
