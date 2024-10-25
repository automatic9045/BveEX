using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Native
{
    internal class FrameSpan
    {
        private bool IsFirstFrame = false;
        private TimeSpan Time = TimeSpan.Zero;

        public FrameSpan()
        {
        }

        public void Initialize()
        {
            IsFirstFrame = true;
        }

        public TimeSpan Tick(TimeSpan now)
        {
            TimeSpan span = IsFirstFrame ? TimeSpan.Zero : now - Time;

            Time = now;
            IsFirstFrame= false;

            return span;
        }
    }
}
