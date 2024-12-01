using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

namespace BveEx.Samples.MapPlugins.TrainControllerEx.Manual
{
    internal static class MatrixExtensions
    {
        public static Vector3 GetTranslation(this Matrix matrix) => new Vector3(matrix.M41, matrix.M42, matrix.M43);
    }
}
