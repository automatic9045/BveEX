using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UnembeddedResources;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;

namespace BveEx
{
    internal partial class VersionForm : Form
    {
        private Label Title;

        private Label LongName;
        private Label Description;
        private Label Copyright;

        private LinkLabel LicenseLink;
        private LinkLabel HomepageLink;
        private LinkLabel RepositoryLink;

        private Label PluginListHeader;
        private Dictionary<int, PluginListTabPage> PluginListPages;
        private TabControl PluginList;
        private Panel PluginListPanel;

        private Button OK;

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new Size(96, 96);
            AutoScaleMode = AutoScaleMode.Dpi;

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ClientSize = new Size(800, 480);
            Font = new Font("Yu Gothic UI", 9);
            Text = string.Format(Resources.Value.Caption.Value, App.Instance.ProductShortName);


            Title = new Label()
            {
                Left = 16,
                Top = 16,
                Width = 160,
                Height = 48,
                AutoSize = true,
                Font = new Font("Yu Gothic UI", 28, FontStyle.Bold),
                Text = App.Instance.ProductShortName,
            };
            Controls.Add(Title);


            LongName = new Label()
            {
                Left = 16,
                Top = 88,
                Width = 480,
                Height = 15,
                AutoSize = true,
                Text = App.Instance.ProductName,
            };
            Controls.Add(LongName);

            Description = new Label()
            {
                Left = 16,
                Top = 103,
                Width = 480,
                Height = 15,
                AutoSize = true,
                Text = string.Format(Resources.Value.Description.Value, App.Instance.BveExAssembly.GetName().Version),
            };
            Controls.Add(Description);

            int year = DateTime.Now.Year;
            Copyright = new Label()
            {
                Left = 16,
                Top = 133,
                Width = 400,
                Height = 15,
                AutoSize = true,
                Text = $"Copyright ©  {(year == 2022 ? "2022" : $"2022-{year}")}  おーとま (automatic9045)",
            };
            Controls.Add(Copyright);


            LicenseLink = new LinkLabel()
            {
                Name = nameof(LicenseLink),
                Left = 16,
                Top = 163,
                Width = 56,
                AutoSize = true,
                Text = Resources.Value.License.Value,
            };
            LicenseLink.LinkClicked += LinkClicked;
            Controls.Add(LicenseLink);

            HomepageLink = new LinkLabel()
            {
                Name = nameof(HomepageLink),
                Left = 96,
                Top = 163,
                Width = 176,
                AutoSize = true,
                Text = string.Format(Resources.Value.Website.Value, App.Instance.ProductShortName),
            };
            HomepageLink.LinkClicked += LinkClicked;
            Controls.Add(HomepageLink);

            RepositoryLink = new LinkLabel()
            {
                Name = nameof(RepositoryLink),
                Left = 296,
                Top = 163,
                Width = 128,
                AutoSize = true,
                Text = Resources.Value.Repository.Value,
            };
            RepositoryLink.LinkClicked += LinkClicked;
            Controls.Add(RepositoryLink);


            PluginListHeader = new Label()
            {
                Left = 16,
                Top = 193,
                Width = 400,
                AutoSize = true,
                Text = Resources.Value.PluginListHeader.Value,
            };
            Controls.Add(PluginListHeader);

            PluginListPages = ((PluginType[])Enum.GetValues(typeof(PluginType))).ToDictionary(x => (int)x, x =>
            {
                Resource<string> typeResource = x.GetTypeStringResource();
                string typeText = typeResource.Culture.TextInfo.ToTitleCase(typeResource.Value);

                return new PluginListTabPage(typeText, x == PluginType.Extension);
            });

            PluginList = new TabControl()
            {
                Dock = DockStyle.Fill,
            };
            PluginList.TabPages.AddRange(PluginListPages.Values.ToArray());

            PluginListPanel = new Panel()
            {
                Left = 16,
                Top = 216,
                Width = 768,
                Height = 224,
            };
            PluginListPanel.Controls.Add(PluginList);
            Controls.Add(PluginListPanel);

            OK = new Button()
            {
                Left = 704,
                Top = 448,
                Text = Resources.Value.OK.Value,
            };
            OK.Click += (sender, e) => Hide();
            Controls.Add(OK);


            ResumeLayout(false);
        }

        private void LinkClicked(object sender, EventArgs e)
        {
            if (sender is LinkLabel linkLabel)
            {
                linkLabel.LinkVisited = true;

                string link = "";
                switch (linkLabel.Name)
                {
                    case nameof(LicenseLink):
                        link = "https://github.com/automatic9045/BveEX/blob/main/README.md";
                        break;

                    case nameof(HomepageLink):
                        link = "https://www.okaoka-depot.com/AtsEX/";
                        break;

                    case nameof(RepositoryLink):
                        link = "https://github.com/automatic9045/BveEX/";
                        break;

                    default:
                        return;
                }

                Process.Start(link);
            }
        }
    }
}
