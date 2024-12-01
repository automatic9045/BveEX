using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.Extensions.Native
{
    internal class PanelArray : IList<int>
    {
        private readonly AtsPlugin AtsPlugin;

        private IList<int> Array => AtsPlugin.PanelArray;
        private IList<double> StoreArray => AtsPlugin.StateStore.PanelArray;

        public int this[int index]
        {
            get => Array[index];
            set
            {
                Array[index] = value;
                StoreArray[index] = value;
            }
        }

        public int Count => Array.Count;
        public bool IsReadOnly => Array.IsReadOnly;

        public PanelArray(AtsPlugin atsPlugin)
        {
            AtsPlugin = atsPlugin;
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
