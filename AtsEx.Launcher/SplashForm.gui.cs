using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.Launcher
{
    internal partial class SplashForm
    {
        private const int VerticalMargin = 16;
        private const int HorizonalMargin = 12;

        private Label LoadingLabel;
        private Label BveVersionLabel;
        private Label AtsExVersionLabel;
        private Label ProgressTextLabel;

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new Size(96, 96);
            AutoScaleMode = AutoScaleMode.Dpi;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            Cursor = Cursors.WaitCursor;
            DoubleBuffered = true;
            Text = "Splash Screen";
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
                Text = $"BVE version: {Assembly.GetEntryAssembly().GetName().Version}",
            };
            Controls.Add(BveVersionLabel);

            AtsExVersionLabel = new Label()
            {
                Left = HorizonalMargin,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackColor = Color.Transparent,
                Text = $"AtsEX Launcher version: {Assembly.GetExecutingAssembly().GetName().Version}",
            };
            Controls.Add(AtsExVersionLabel);

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
            AtsExVersionLabel.Top = ProgressTextLabel.Top - VerticalMargin / 2 - AtsExVersionLabel.Height;
            BveVersionLabel.Top = AtsExVersionLabel.Top - BveVersionLabel.Height;
        }
    }
}
