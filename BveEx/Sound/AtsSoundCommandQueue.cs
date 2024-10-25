using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Sound
{
    internal sealed class AtsSoundCommandQueue
    {
        private readonly List<AtsSoundCommand> Queue = new List<AtsSoundCommand>();

        public AtsSoundCommandQueue()
        {
        }

        private void SetNextCommand(AtsSoundCommand item, bool overwrite)
        {
            if (item == AtsSoundCommand.Continue) throw new NotSupportedException();
            if (overwrite && 0 < Queue.Count) Queue.Clear();

            Queue.Add(item);
        }

        public void SetNextCommand(AtsSoundCommand item) => SetNextCommand(item, true);

        public void SetNextCommand(params AtsSoundCommand[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                SetNextCommand(items[i], i == 0);
            }
        }

        public AtsSoundCommand Tick()
        {
            if (Queue.Count == 0)
            {
                return AtsSoundCommand.Continue;
            }
            else
            {
                AtsSoundCommand first = Queue[0];
                Queue.RemoveAt(0);
                return first;
            }
        }
    }
}
