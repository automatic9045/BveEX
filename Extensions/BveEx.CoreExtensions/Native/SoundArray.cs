using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.Extensions.Native
{
    internal class SoundArray : IList<int>
    {
        private readonly AtsPlugin AtsPlugin;
        private readonly IList<int> Array;

        public int this[int index]
        {
            get => Array[index];
            set
            {
                Array[index] = value;
                if (Array[index] == AtsPlugin.OldSoundArray[index]) return;

                AtsPlugin.OldSoundArray[index] = Array[index];

                if (Array[index] == 1)
                {
                    AtsPlugin.PlaySoundRequested_Invoke(new AtsPlugin.AtsSoundEventArgs(index, 0));
                    Array[index] = 2;
                }
                else if (Array[index] <= 0)
                {
                    AtsPlugin.LoopSoundRequested_Invoke(new AtsPlugin.AtsSoundEventArgs(index, Array[index]));
                }
            }
        }

        public int Count => Array.Count;
        public bool IsReadOnly => Array.IsReadOnly;

        public SoundArray(AtsPlugin atsPlugin)
        {
            AtsPlugin = atsPlugin;
            Array = AtsPlugin.SoundArray;
        }

        public void Add(int item) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(int item) => Array.Contains(item);
        public void CopyTo(int[] array, int arrayIndex) => Array.CopyTo(array, arrayIndex);
        public IEnumerator<int> GetEnumerator() => Array.GetEnumerator();
        public int IndexOf(int item) => Array.IndexOf(item);
        public void Insert(int index, int item) => throw new NotSupportedException();
        public bool Remove(int item) => throw new NotSupportedException();
        public void RemoveAt(int index) => throw new NotSupportedException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
