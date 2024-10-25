using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Samples.MapPlugins.StationController
{
    internal static class TimeTextParser
    {
        public static int ToTimeMilliseconds(this string timeText)
        {
            string[] splittedText = timeText.Split(':');
            if (splittedText.Length != 3) throw new FormatException();

            int[] time = new int[3];
            for (int i = 0; i < time.Length; i++)
            {
                if (!int.TryParse(splittedText[i], out time[i])) throw new FormatException();
            }

            return (time[0] * 3600 + time[1] * 60 + time[2]) * 1000;
        }

        public static string ToTimeText(this int timeMilliseconds)
        {
            int timeSeconds = timeMilliseconds / 1000;

            int hours = timeSeconds / 3600;
            int minutes = timeSeconds / 60 % 60;
            int seconds = timeSeconds % 60;

            return $"{hours}:{minutes:D2}:{seconds:D2}";
        }
    }
}
