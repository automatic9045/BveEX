using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Plugins
{
    internal class Identifier : IComparable<Identifier>
    {
        public string Text { get; }

        public Identifier(string text)
        {
            Text = text;
        }

        public int CompareTo(Identifier other) => Text.CompareTo(other.Text);
    }

    internal sealed class RandomIdentifier : Identifier
    {
        public RandomIdentifier() : base(Guid.NewGuid().ToString())
        {
        }
    }
}
