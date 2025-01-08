using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveTypes
{
    internal class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            return x.FullName.CompareTo(y.FullName);
        }
    }

    internal class TypeArrayComparer : IComparer<Type[]>
    {
        protected TypeComparer TypeComparer = new TypeComparer();

        public int Compare(Type[] x, Type[] y)
        {
            if (x.Length != y.Length)
            {
                return x.Length - y.Length;
            }
            else
            {
                for (int i = 0; i < x.Length; i++)
                {
                    int result = TypeComparer.Compare(x[i], y[i]);
                    if (result != 0) return result;
                }

                return 0;
            }
        }
    }

    internal class StringTypeArrayTupleComparer : IComparer<(string, Type[])>
    {
        public int Compare((string, Type[]) x, (string, Type[]) y)
        {
            string xText = x.Item1 + x.Item2.ToString();
            string yText = y.Item1 + y.Item2.ToString();
            return string.Compare(xText, yText);
        }
    }
}
