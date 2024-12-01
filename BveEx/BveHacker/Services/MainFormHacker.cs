using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

namespace BveEx.BveHackerServices
{
    internal sealed class MainFormHacker
    {
        public IntPtr TargetFormHandle { get; }
        public Form TargetFormSource { get; }
        public MainForm TargetForm { get; }

        public MainFormHacker(Process targetProcess)
        {
            TargetFormHandle = targetProcess.MainWindowHandle;
            TargetFormSource = (Form)Control.FromHandle(TargetFormHandle);
            TargetForm = MainForm.FromSource(TargetFormSource);
        }
    }
}
