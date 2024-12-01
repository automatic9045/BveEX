using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BveEx.Launcher.SplashScreen
{
    internal partial class SplashForm : Form
    {
        public string ProgressText
        {
            get => ProgressTextLabel.Text;
            set
            {
                ProgressTextLabel.Text = value;
                Application.DoEvents();
            }
        }

        public SplashForm(Version bveVersion, Version launcherVersion)
        {
            InitializeComponent(bveVersion, launcherVersion);

            FormClosing += (sender, e) => e.Cancel = true;
        }
    }
}
