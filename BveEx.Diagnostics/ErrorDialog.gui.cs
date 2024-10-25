using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.Diagnostics
{
    public partial class ErrorDialog
    {
        private const int VerticalMargin = 16;
        private const int HorizonalMargin = 12;

        private Font ValueFont;

        private PictureBox HeaderBackground;
        private Label Header;

        private Label SenderKey;
        private Label SenderValue;

        private Label Message;
        private TextBox MessageText;

        private Label Approach;
        private TextBox ApproachText;

        private Button HelpLinkButton = null;
        private Button ExitButton;
        private Button IgnoreButton;
        private Label HelpInfoLabel = null;

        private void InitializeComponent(ErrorDialogInfo info)
        {
            SuspendLayout();

            AutoScaleDimensions = new Size(96, 96);
            AutoScaleMode = AutoScaleMode.Dpi;

            Text = info.GetCaption();
            Width = 800;
            Height = 600;
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
                Text = info.GetHeader(),
                Font = new Font(Font.FontFamily, 24, FontStyle.Regular),
                ForeColor = Color.White,
            };
            HeaderBackground.Controls.Add(Header);

            SenderKey = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "場所:",
            };
            Controls.Add(SenderKey);

            SenderValue = new Label()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = info.GetSender(),
                Font = ValueFont,
            };
            Controls.Add(SenderValue);

            Message = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "内容:",
            };
            Controls.Add(Message);

            MessageText = new TextBox()
            {
                Left = HorizonalMargin,
                Height = 100,
                Width = ClientSize.Width - HorizonalMargin * 2,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.White,
                Text = info.Message?.Replace("\n", "\r\n")?.Replace("\r\r", "\r"),
                ReadOnly = true,
                Multiline = true,
                SelectionStart = 0,
            };
            Controls.Add(MessageText);

            Approach = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Text = "対処方法:",
            };
            Controls.Add(Approach);

            ApproachText = new TextBox()
            {
                Left = HorizonalMargin,
                Width = ClientSize.Width - HorizonalMargin * 2,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                BackColor = Color.White,
                Text = info.Approach?.Replace("\n", "\r\n")?.Replace("\r\r", "\r"),
                ReadOnly = true,
                Multiline = true,
                SelectionStart = 0,
            };
            Controls.Add(ApproachText);

            ExitButton = new Button()
            {
                Height = 40,
                Width = 125,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "中断 / Abort",
            };
            Controls.Add(ExitButton);
            ExitButton.Click += (sender2, e) =>
            {
                DialogResult result = MessageBox.Show($"読込を中断し、BVE を終了してもよろしいですか?\nAbort loading and exit BVE Trainsim?",
                    "AtsEX Diagnostics", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) Application.Exit();
            };

            IgnoreButton = new Button()
            {
                Height = 40,
                Width = 125,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Text = "無視 / Ignore",
                DialogResult = DialogResult.OK,
            };
            Controls.Add(IgnoreButton);

            if (!(info.HelpLink is null))
            {
                HelpLinkButton = new Button()
                {
                    Height = 40,
                    Width = 100,
                    Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                    Text = "(?) ヘルプ",
                };
                Controls.Add(HelpLinkButton);
                HelpLinkButton.Click += (sender2, e) =>
                {
                    DialogResult result = MessageBox.Show($"このヘルプリンクを信頼し、エラーに関する情報を表示しますか?\nTrust this help link and open it?\n\n{info.HelpLink}",
                        "AtsEX Diagnostics", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes) Process.Start(info.HelpLink.ToString());
                };

                HelpInfoLabel = new Label()
                {
                    AutoSize = true,
                    Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                    Text = $"[ヘルプ] をクリックすると '{info.HelpLink}' が開きます",
                    Font = new Font(Font.FontFamily, 9),
                };
                Controls.Add(HelpInfoLabel);
            }


            ResumeLayout(false);
            RedrawHeaderBackground();

            Header.Top = HeaderBackground.Height - Header.PreferredSize.Height - VerticalMargin / 2;
            SenderKey.Top = HeaderBackground.Bottom + VerticalMargin;
            SenderValue.Top = SenderKey.Top - 2;
            SenderValue.Left = SenderKey.Right + HorizonalMargin;
            Message.Top = SenderKey.Bottom + VerticalMargin;
            MessageText.Top = Message.Bottom + VerticalMargin / 2;
            Approach.Top = MessageText.Bottom + VerticalMargin;

            if (!(HelpInfoLabel is null))
            {
                HelpInfoLabel.Top = ClientSize.Height - VerticalMargin - HelpInfoLabel.Height;
                HelpInfoLabel.Left = ClientSize.Width - HorizonalMargin - HelpInfoLabel.Width;
            }
            IgnoreButton.Top = (HelpInfoLabel is null ? ClientSize.Height - VerticalMargin : HelpInfoLabel.Top - VerticalMargin / 4) - IgnoreButton.Height;
            IgnoreButton.Left = ClientSize.Width - HorizonalMargin - IgnoreButton.Width;
            ExitButton.Top = IgnoreButton.Top;
            ExitButton.Left = IgnoreButton.Left - HorizonalMargin - ExitButton.Width;
            if (!(HelpInfoLabel is null))
            {
                HelpLinkButton.Top = ExitButton.Top;
                HelpLinkButton.Left = ExitButton.Left - HorizonalMargin - HelpLinkButton.Width;
            }

            ApproachText.Top = Approach.Bottom + VerticalMargin / 2;
            ApproachText.Height = IgnoreButton.Top - VerticalMargin - (Approach.Bottom + VerticalMargin);

            Resize += (sender2, e) => RedrawHeaderBackground();


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
                    using (Stream logoStream = assembly.GetManifestResourceStream(typeof(ErrorDialog), "Logo.png"))
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
