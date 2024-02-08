using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.Direct3D9;

using BveTypes.ClassWrappers;

namespace AtsEx.Samples.MapPlugins.TrainControllerEx.Manual
{
    internal class TrainLocator
    {
        private readonly Train Target;

        private readonly float RotationFactorA; // 曲がりやすさ。単位は [m/s]^-1
        private readonly float RotationFactorB; // 速度の上昇につれて曲がりづらくなる率。単位は [m/s]^-2

        private readonly Func<Matrix> GetOriginLocationFunc;

        private float Direction;
        private Matrix Location = Matrix.Identity;

        public TrainLocator(Train target, Vector3 initialLocation, float initialDirection, float rotationFactorA, float rotationFactorB, Func<Matrix> getOriginLocationFunc)
        {
            Target = target;

            Location = Matrix.Translation(initialLocation);
            Direction = initialDirection;

            RotationFactorA = rotationFactorA;
            RotationFactorB = rotationFactorB;

            GetOriginLocationFunc = getOriginLocationFunc;
        }

        public void Tick(float rotationSpeedFactor, float speed, TimeSpan elapsed)
        {
            float rotationSpeed =
                speed > 0 ?   Math.Max(0, RotationFactorA * speed - RotationFactorB * speed * speed)
                : speed < 0 ? Math.Min(0, RotationFactorA * speed + RotationFactorB * speed * speed)
                : 0;

            Direction += rotationSpeedFactor * rotationSpeed * (float)elapsed.TotalSeconds;

            Vector3 location = Location.GetTranslation();
            Location = Matrix.Translation(0, 0, -speed * (float)elapsed.TotalSeconds) * Matrix.RotationY(Direction) * Matrix.Translation(location);
        }

        public void Draw(Direct3DProvider direct3DProvider, Matrix viewMatrix)
        {
            foreach (Structure structure in Target.TrainInfo.Structures)
            {
                if (structure.Model is null) return;

                direct3DProvider.Device.SetTransform(TransformState.World, Location * GetOriginLocationFunc() * viewMatrix);
                structure.Model.Draw(direct3DProvider, false);
                structure.Model.Draw(direct3DProvider, true);
            }
        }
    }
}
