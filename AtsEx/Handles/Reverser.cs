using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost.Handles;

namespace AtsEx.Handles
{
    internal class Reverser : IReverser
    {
        private ReverserPosition _Position = ReverserPosition.N;
        public ReverserPosition Position
        {
            get => _Position;
            set
            {
                ReverserPosition oldPosition = Position;
                _Position = value;

                if (_Position != oldPosition) PositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler PositionChanged;

        public Reverser()
        {
        }
    }
}
