using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace AtsEx.PluginHost.Handles
{
    public enum ConstantSpeedCommand
    {
        Continue = 0,
        Enable = 1,
        Disable = 2,
    }

    public static class ConstantSpeedCommandExtensions
    {
        public static ConstantSpeedMode ToConstantSpeedMode(this ConstantSpeedCommand source)
        {
            switch (source)
            {
                case ConstantSpeedCommand.Enable:
                    return ConstantSpeedMode.Enable;

                case ConstantSpeedCommand.Disable:
                    return ConstantSpeedMode.Disable;

                case ConstantSpeedCommand.Continue:
                    return ConstantSpeedMode.Continue;

                default:
                    throw new NotImplementedException();
            }
        }
        public static ConstantSpeedCommand ToConstantSpeedCommand(this ConstantSpeedMode source)
        {
            switch (source)
            {
                case ConstantSpeedMode.Continue:
                    return ConstantSpeedCommand.Continue;

                case ConstantSpeedMode.Enable:
                    return ConstantSpeedCommand.Enable;

                case ConstantSpeedMode.Disable:
                    return ConstantSpeedCommand.Disable;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
