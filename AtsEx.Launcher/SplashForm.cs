using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsEx.Launcher
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

        public SplashForm()
        {
            InitializeComponent();
        }
    }
}
