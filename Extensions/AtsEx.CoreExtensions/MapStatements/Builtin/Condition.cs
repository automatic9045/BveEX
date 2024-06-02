using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.MapStatements.Builtin
{
    internal static class Condition
    {
        public static bool Parse(IReadOnlyList<object> args)
        {
            switch (args.Count)
            {
                case 1:
                    switch (args[0])
                    {
                        case int num:
                            return num != 0;
                        case float num:
                            return num != 0;
                        default:
                            throw new ArgumentException();
                    }

                case 3:
                    switch (args[1])
                    {
                        case "==":
                            return args[0].Equals(args[2]);
                        case "!=":
                            return !args[0].Equals(args[2]);
                        case "<":
                            return Convert.ToDouble(args[0]) < Convert.ToDouble(args[2]);
                        case "<=":
                            return Convert.ToDouble(args[0]) <= Convert.ToDouble(args[2]);
                        case ">":
                            return Convert.ToDouble(args[0]) > Convert.ToDouble(args[2]);
                        case ">=":
                            return Convert.ToDouble(args[0]) >= Convert.ToDouble(args[2]);
                        default:
                            throw new ArgumentException();
                    }

                default:
                    throw new ArgumentException();
            }
        }
    }
}
