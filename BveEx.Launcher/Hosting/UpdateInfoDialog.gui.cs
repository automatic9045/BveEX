using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BveEx.Launcher.Hosting
{
    internal partial class UpdateInfoDialog : Form
    {
        private const int VerticalMargin = 16;
        private const int HorizonalMargin = 12;

        private Font ValueFont;

        private PictureBox HeaderBackground;
        private Label Header;

        private Label CurrentVersionKey;
        private Label CurrentVersionValue;
        private Label NewVersionKey;
        private Label NewVersionValue;

        private Label Details;
        private WebBrowser InfoBrowser;

        private CheckBox DoNotShowAgainCheckBox;
        private Button NoThanksButton;
        private Button GoToDownloadPageButton;

        private void InitializeComponent(Version currentVersion, Version newVersion, string updateDetailsHtml)
        {
            SuspendLayout();

            AutoScaleDimensions = new Size(96, 96);
            AutoScaleMode = AutoScaleMode.Dpi;

            Text = "アップデート情報 - BveEX";
            Height = 600;
            Width = 800;
            Font = new Font("Yu Gothic UI", 10);

            ValueFont = new Font(Font.FontFamily, 12, FontStyle.Bold);


            HeaderBackground = new PictureBox()
            {
                Top = 0,
                Left = 0,
                Height = 80,
                Width = ClientSize.Width,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Center,
            };
            Controls.Add(HeaderBackground);

            Header = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Text = "新しいバージョンがご利用可能です!",
                Font = new Font(Font.FontFamily, 24, FontStyle.Regular),
                ForeColor = Color.White,
            };
            HeaderBackground.Controls.Add(Header);

            CurrentVersionKey = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "現在のバージョン",
            };
            Controls.Add(CurrentVersionKey);

            CurrentVersionValue = new Label()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = currentVersion.ToString(),
                Font = ValueFont,
            };
            Controls.Add(CurrentVersionValue);

            NewVersionKey = new Label()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "利用可能なバージョン",
            };
            Controls.Add(NewVersionKey);

            NewVersionValue = new Label()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = newVersion.ToString(),
                Font = ValueFont,
            };
            Controls.Add(NewVersionValue);

            Details = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "アップデートの詳細:",
            };
            Controls.Add(Details);

            InfoBrowser = new WebBrowser()
            {
                Left = 0,
                Width = ClientSize.Width,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                DocumentText = "<font face='Yu Gothic UI'>" + updateDetailsHtml,
                AllowNavigation = false,
                AllowWebBrowserDrop = false,
            };
            Controls.Add(InfoBrowser);

            DoNotShowAgainCheckBox = new CheckBox()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Text = "この案内を今後 12 時間のあいだ表示しない",
            };
            Controls.Add(DoNotShowAgainCheckBox);

            NoThanksButton = new Button()
            {
                Height = 40,
                Width = 100,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "スキップ",
                DialogResult = DialogResult.Cancel,
            };
            Controls.Add(NoThanksButton);

            GoToDownloadPageButton = new Button()
            {
                Height = 40,
                Width = 200,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "ダウンロードページへ",
                DialogResult = DialogResult.OK,
            };
            Controls.Add(GoToDownloadPageButton);


            ResumeLayout(false);
            RedrawHeaderBackground();

            Header.Top = HeaderBackground.Height - Header.PreferredSize.Height - VerticalMargin / 2;

            CurrentVersionKey.Top = HeaderBackground.Bottom + VerticalMargin;
            CurrentVersionValue.Top = CurrentVersionKey.Top - 3;
            CurrentVersionValue.Left = CurrentVersionKey.Right + HorizonalMargin;
            NewVersionKey.Top = CurrentVersionKey.Top;
            NewVersionKey.Left = CurrentVersionValue.Right + HorizonalMargin * 4;
            NewVersionValue.Top = CurrentVersionValue.Top;
            NewVersionValue.Left = NewVersionKey.Right + HorizonalMargin;

            Details.Top = CurrentVersionKey.Bottom + VerticalMargin;

            GoToDownloadPageButton.Top = ClientSize.Height - VerticalMargin - GoToDownloadPageButton.Height;
            GoToDownloadPageButton.Left = ClientSize.Width - HorizonalMargin - GoToDownloadPageButton.Width;
            NoThanksButton.Top = GoToDownloadPageButton.Top;
            NoThanksButton.Left = GoToDownloadPageButton.Left - HorizonalMargin - NoThanksButton.Width;
            DoNotShowAgainCheckBox.Top = ClientSize.Height - VerticalMargin - DoNotShowAgainCheckBox.Height;

            InfoBrowser.Top = Details.Bottom + VerticalMargin / 2;
            InfoBrowser.Height = GoToDownloadPageButton.Top - VerticalMargin - (Details.Bottom + VerticalMargin);

            Resize += (sender, e) => RedrawHeaderBackground();


            void RedrawHeaderBackground()
            {
                Bitmap bitmap = new Bitmap(HeaderBackground.Width, HeaderBackground.Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(g.VisibleClipBounds, Color.Black, Color.FromArgb(0, 0xC0, 0xC0), LinearGradientMode.Horizontal))
                    {
                        g.FillRectangle(brush, g.VisibleClipBounds);
                    }

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    using (Stream logoStream = assembly.GetManifestResourceStream(typeof(UpdateInfoDialog), "Logo.png"))
                    {
                        Image logo = Image.FromStream(logoStream);

                        float scale = g.DpiX / 96;
                        float logoHeight = 56 * scale;
                        SizeF size = new SizeF(logoHeight * logo.Width / logo.Height, logoHeight);
                        PointF point = new PointF(g.VisibleClipBounds.Width - size.Width - 15 * scale, g.VisibleClipBounds.Height - size.Height);

                        g.DrawImage(logo, new RectangleF(point, size));
                    }
                }

                HeaderBackground.BackgroundImage = bitmap;
            }
        }
    }
}
