using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using BveTypes.ClassWrappers;

using AtsEx.Extensions.TrainDrawPatch;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Input;
using AtsEx.PluginHost.Input.Native;
using AtsEx.PluginHost.Plugins;

namespace AtsEx.Samples.MapPlugins.TrainControllerEx.Manual
{
    [PluginType(PluginType.MapPlugin)]
    public class PluginMain : AssemblyPluginBase
    {
        private Train Train;
        private TrainLocator TrainLocator;
        private TrainDrawPatch Patch;

        private float RotationSpeedFactor = 0; // -1 ～ 1
        private float Speed = 15 / 3.6f; // 初速度 15 [km/h] = (15 / 3.6) [m/s]

        public PluginMain(PluginBuilder builder) : base(builder)
        {
            BveHacker.ScenarioCreated += OnScenarioCreated;
        }

        public override void Dispose()
        {
            Patch.Dispose();
            BveHacker.ScenarioCreated -= OnScenarioCreated;
        }

        private void OnScenarioCreated(ScenarioCreatedEventArgs e)
        {
            if (!e.Scenario.Trains.ContainsKey("test_ex_1"))
            {
                throw new BveFileLoadException("キーが 'test_ex_1' の他列車が見つかりませんでした。", "TrainControllerEx.Manual");
            }

            Train = e.Scenario.Trains["test_ex_1"];

            Vector3 initialLocation = new Vector3(15, 0, 30); // 初期位置はワールド座標系で (15, 0, 30)
            float initialDirection = (float)(Math.PI * 0.5); // 回転の基点は手前方向、右回りを正とする。初期状態は π/2 = 左向き

            TrainLocator = new TrainLocator(Train, initialLocation, initialDirection, 0.2f, 0.01f,
                () => BveHacker.Scenario.Route.MyTrack.GetTransform(0, BveHacker.Scenario.LocationManager.BlockIndex * 25));
            Patch = Extensions.GetExtension<ITrainDrawPatchFactory>().Patch(nameof(Manual), Train, TrainLocator.Draw);
        }

        public override TickResult Tick(TimeSpan elapsed)
        {
            IReadOnlyDictionary<NativeAtsKeyName, KeyBase> atsKeys = Native.NativeKeys.AtsKeys;

            RotationSpeedFactor = Math.Min(1, Math.Max(-1, CalculateSpeed(RotationSpeedFactor, 2, 1, elapsed, atsKeys[NativeAtsKeyName.H].IsPressed, atsKeys[NativeAtsKeyName.I].IsPressed)));

            float maxSpeed = 60 / 3.6f; // 60 [km/h] = (60 / 3.6) [m/s]
            Speed = Math.Min(maxSpeed, Math.Max(-maxSpeed, CalculateSpeed(Speed, 5, 3, elapsed, atsKeys[NativeAtsKeyName.J].IsPressed, atsKeys[NativeAtsKeyName.K].IsPressed)));

            TrainLocator.Tick(RotationSpeedFactor, Speed, elapsed);

            return new MapPluginTickResult();
        }

        /// <summary>
        /// 前フレームの速度をもとに、今フレームの速度を計算します。
        /// </summary>
        /// <param name="currentSpeed">前フレームの速度。</param>
        /// <param name="accelerationFactor">加速・減速時、1 秒当たりに変化する値。値が大きいほど早く加速・減速するようになります。</param>
        /// <param name="attenuationFactor">1 秒当たりに減衰する値。絶対値で指定してください。</param>
        /// <param name="elapsed">前フレームから経過した時間。</param>
        /// <param name="isAccelerating">加速しているか。</param>
        /// <param name="isDecelerating">減速しているか。</param>
        /// <returns>今フレームの速度。</returns>
        float CalculateSpeed(float currentSpeed, float accelerationFactor, float attenuationFactor, TimeSpan elapsed, bool isAccelerating, bool isDecelerating)
        {
            float newSpeed = currentSpeed;

            if (isAccelerating) newSpeed -= accelerationFactor * (float)elapsed.TotalSeconds;
            if (isDecelerating) newSpeed += accelerationFactor * (float)elapsed.TotalSeconds;

            if (newSpeed > 0)
            {
                newSpeed = Math.Max(0, newSpeed - attenuationFactor * (float)elapsed.TotalSeconds);
            }
            else if (newSpeed < 0)
            {
                newSpeed = Math.Min(0, newSpeed + attenuationFactor * (float)elapsed.TotalSeconds);
            }

            return newSpeed;
        }
    }
}
