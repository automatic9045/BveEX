using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BveEx.Launcher
{
    public interface ICoreHost : IDisposable
    {
        event EventHandler<InputEventArgs> LeverMoved;
        event EventHandler<InputEventArgs> KeyDown;
        event EventHandler<InputEventArgs> KeyUp;

        void Configure(IWin32Window owner);
        void Load(string settingsPath);
        void SetAxisRanges(int[][] ranges);
        void Tick();
    }
}
