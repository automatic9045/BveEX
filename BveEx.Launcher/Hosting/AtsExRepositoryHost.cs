using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Octokit;

namespace BveEx.Launcher.Hosting
{
    internal sealed class BveExRepositoryHost
    {
        private const string RepositoryOwner = "automatic9045";
        private const string RepositoryName = "AtsEX";

        public BveExRepositoryHost()
        {
        }

        public async Task<ReleaseInfo> GetLatestReleaseAsync()
        {
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("bveex"));
            IReadOnlyList<Release> releases = await gitHubClient.Repository.Release.GetAll(RepositoryOwner, RepositoryName).ConfigureAwait(false);

            if (releases.Count == 0) throw new Exception("リリースが見つかりません。");

            IEnumerable<Release> stableReleases = releases.Where(r => !r.Prerelease);
            Release latestRelease = stableReleases.Any() ? stableReleases.First() : releases.First();

            string latestVersionText = latestRelease.TagName.TrimStart('v');
            Version latestVersion = Version.Parse(latestVersionText);

            string GetUpdateDetails()
            {
                IEnumerable<ReleaseAsset> updateInfoAssets = latestRelease.Assets.Where(asset => asset.Name.StartsWith("UpdateInfo.", StringComparison.Ordinal));
                ReleaseAsset updateInfoAsset = updateInfoAssets.FirstOrDefault(asset => asset.Name.EndsWith(".md", StringComparison.Ordinal)) ?? updateInfoAssets.First();
                using (HttpClient httpClient = new HttpClient())
                {
                    string updateDetails = httpClient.GetStringAsync(updateInfoAsset.BrowserDownloadUrl).Result;
                    return updateDetails;
                }
            }

            return new ReleaseInfo(latestVersion, new Uri(latestRelease.HtmlUrl), GetUpdateDetails);
        }
    }
}
