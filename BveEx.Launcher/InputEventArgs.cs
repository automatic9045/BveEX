using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    public class InputEventArgs : EventArgs
    {
        public int Axis { get; }
        public int Value { get; }

        public InputEventArgs(int axis, int value)
        {
            Axis = axis;
            Value = value;
        }
    }
}
