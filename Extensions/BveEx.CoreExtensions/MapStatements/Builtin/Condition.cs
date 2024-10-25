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
                        case double num:
                            return num != 0;
                        default:
                            throw new ArgumentException();
                    }

                case 3:
                    object left = args[0];
                    if (left is int leftNum) left = Convert.ToDouble(leftNum);

                    object right = args[2];
                    if (right is int rightNum) right = Convert.ToDouble(rightNum);

                    switch (args[1])
                    {
                        case "==":
                            return left.Equals(right);
                        case "!=":
                            return !left.Equals(right);
                        case "<":
                            return Convert.ToDouble(left) < Convert.ToDouble(right);
                        case "<=":
                            return Convert.ToDouble(left) <= Convert.ToDouble(right);
                        case ">":
                            return Convert.ToDouble(left) > Convert.ToDouble(right);
                        case ">=":
                            return Convert.ToDouble(left) >= Convert.ToDouble(right);
                        default:
                            throw new ArgumentException();
                    }

                default:
                    throw new ArgumentException();
            }
        }
    }
}
