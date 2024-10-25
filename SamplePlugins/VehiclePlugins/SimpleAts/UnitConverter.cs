using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Samples.VehiclePlugins.SimpleAts
{
    internal static class UnitConverter
    {
        public static double KmphToMps(this double speedKillometerPerHour) => speedKillometerPerHour / 3.6;
        public static double MpsToKmph(this double speedMeterPerSecond) => speedMeterPerSecond * 3.6;
    }
}
