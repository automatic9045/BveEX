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

namespace AtsEx.Launcher.Hosting
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
            AutoScaleMode = AutoScaleMode.Dpi;

            Text = "アップデート情報 - AtsEX";
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
            RedrawHeaderBackground();
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
            Header.Top = HeaderBackground.Height - Header.PreferredSize.Height - VerticalMargin / 2;

            CurrentVersionKey = new Label()
            {
                Top = HeaderBackground.Bottom + VerticalMargin,
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "現在のバージョン",
            };
            Controls.Add(CurrentVersionKey);

            CurrentVersionValue = new Label()
            {
                Top = CurrentVersionKey.Top - 3,
                Left = CurrentVersionKey.Right + HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = currentVersion.ToString(),
                Font = ValueFont,
            };
            Controls.Add(CurrentVersionValue);

            NewVersionKey = new Label()
            {
                Top = CurrentVersionKey.Top,
                Left = CurrentVersionValue.Right + HorizonalMargin * 4,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "利用可能なバージョン",
            };
            Controls.Add(NewVersionKey);

            NewVersionValue = new Label()
            {
                Top = CurrentVersionValue.Top,
                Left = NewVersionKey.Right + HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = newVersion.ToString(),
                Font = ValueFont,
            };
            Controls.Add(NewVersionValue);

            Details = new Label()
            {
                Top = CurrentVersionKey.Bottom + VerticalMargin,
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "アップデートの詳細:",
            };

            GoToDownloadPageButton = new Button()
            {
                Height = 40,
                Width = 200,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "ダウンロードページへ",
                DialogResult = DialogResult.OK,
            };
            GoToDownloadPageButton.Top = ClientSize.Height - VerticalMargin - GoToDownloadPageButton.Height;
            GoToDownloadPageButton.Left = ClientSize.Width - HorizonalMargin - GoToDownloadPageButton.Width;

            NoThanksButton = new Button()
            {
                Top = GoToDownloadPageButton.Top,
                Height = 40,
                Width = 100,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "スキップ",
                DialogResult = DialogResult.Cancel,
            };
            NoThanksButton.Left = GoToDownloadPageButton.Left - HorizonalMargin - NoThanksButton.Width;

            DoNotShowAgainCheckBox = new CheckBox()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Text = "この案内を今後 12 時間のあいだ表示しない",
            };
            DoNotShowAgainCheckBox.Top = ClientSize.Height - VerticalMargin - DoNotShowAgainCheckBox.Height;

            InfoBrowser = new WebBrowser()
            {
                Top = Details.Bottom + VerticalMargin / 2,
                Left = 0,
                Height = GoToDownloadPageButton.Top - VerticalMargin - (Details.Bottom + VerticalMargin),
                Width = ClientSize.Width,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                DocumentText = "<font face='Yu Gothic UI'>" + updateDetailsHtml,
                AllowNavigation = false,
                AllowWebBrowserDrop = false,
            };

            Controls.Add(Details);
            Controls.Add(InfoBrowser);

            Controls.Add(DoNotShowAgainCheckBox);
            Controls.Add(NoThanksButton);
            Controls.Add(GoToDownloadPageButton);

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

                        float logoHeight = 56;
                        SizeF size = new SizeF(logoHeight * logo.Width / logo.Height, logoHeight);
                        PointF point = new PointF(g.VisibleClipBounds.Width - size.Width - 15, g.VisibleClipBounds.Height - size.Height);
                        g.DrawImage(logo, new RectangleF(point, size));
                    }
                }

                HeaderBackground.BackgroundImage = bitmap;
            }
        }
    }
}
