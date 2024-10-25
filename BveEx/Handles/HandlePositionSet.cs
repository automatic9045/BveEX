using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.Handles
{
    internal class HandlePositionSet
    {
        public int Power { get; }
        public int Brake { get; }
        public ReverserPosition ReverserPosition { get; }
        public ConstantSpeedMode ConstantSpeed { get; }

        public HandlePositionSet(int power, int brake, ReverserPosition reverserPosition, ConstantSpeedMode constantSpeedMode)
        {
            Power = power;
            Brake = brake;
            ReverserPosition = reverserPosition;
            ConstantSpeed = constantSpeedMode;
        }
    }
}
