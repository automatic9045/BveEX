using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BveEx.Launcher.SplashScreen
{
    internal partial class SplashForm
    {
        private const int VerticalMargin = 16;
        private const int HorizonalMargin = 12;

        private Label LoadingLabel;
        private Label BveVersionLabel;
        private Label BveExVersionLabel;
        private Label ProgressTextLabel;

        private void InitializeComponent(Version bveVersion, Version launcherVersion)
        {
            SuspendLayout();

            AutoScaleDimensions = new Size(96, 96);
            AutoScaleMode = AutoScaleMode.Dpi;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            Cursor = Cursors.WaitCursor;
            DoubleBuffered = true;
            Text = "BVE Trainsim を起動中";
            Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(SplashForm), "icon.ico"));
            ClientSize = new Size(300, 150);
            Font = new Font("Yu Gothic UI", 9);


            LoadingLabel = new Label()
            {
                Top = VerticalMargin,
                Left = 0,
                Width = ClientSize.Width,
                Height = 50,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent,
                Text = "BVE Trainsim を起動中",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.FontFamily, 15),
            };
            Controls.Add(LoadingLabel);

            BveVersionLabel = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackColor = Color.Transparent,
                Text = $"BVE version: {bveVersion}",
            };
            Controls.Add(BveVersionLabel);

            BveExVersionLabel = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackColor = Color.Transparent,
                Text = $"BveEX Launcher version: {launcherVersion}",
            };
            Controls.Add(BveExVersionLabel);

            ProgressTextLabel = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackColor = Color.Transparent,
                Text = "準備中...",
            };
            Controls.Add(ProgressTextLabel);


            ResumeLayout(false);

            ProgressTextLabel.Top = ClientSize.Height - VerticalMargin - ProgressTextLabel.Height;
            BveExVersionLabel.Top = ProgressTextLabel.Top - VerticalMargin / 2 - BveExVersionLabel.Height;
            BveVersionLabel.Top = BveExVersionLabel.Top - BveVersionLabel.Height;
        }
    }
}
