using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Native;

namespace AtsEx.Sound
{
    internal class AtsSoundCommand
    {
        private const int MinVolume = int.MinValue;
        private const int MaxVolume = 0;

        public static readonly AtsSoundCommand Stop = new AtsSoundCommand((int)SoundPlayType.Stop);
        public static readonly AtsSoundCommand Once = new AtsSoundCommand((int)SoundPlayType.Once);
        public static readonly AtsSoundCommand Continue = new AtsSoundCommand((int)SoundPlayType.Continue);

        public static AtsSoundCommand PlayLoop(int volumeBell)
        {
            return volumeBell < MinVolume || MaxVolume < volumeBell
                ? throw new ArgumentOutOfRangeException(nameof(volumeBell))
                : new AtsSoundCommand(volumeBell);
        }

        public static bool operator ==(AtsSoundCommand left, AtsSoundCommand right) => left.SerializedValue == right.SerializedValue;
        public static bool operator !=(AtsSoundCommand left, AtsSoundCommand right) => !(left == right);

        public int SerializedValue { get; }

        private AtsSoundCommand(int value)
        {
            SerializedValue = value;
        }

        public override bool Equals(object obj) => this == obj as AtsSoundCommand;
        public override int GetHashCode() => SerializedValue.GetHashCode();
    }
}
