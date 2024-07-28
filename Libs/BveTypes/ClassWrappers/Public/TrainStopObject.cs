using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// <c>Train[trainKey].Stop</c> ステートメントにより設置された他列車の停止位置を表します。
    /// </summary>
    public class TrainStopObject : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TrainStopObject>();

            Constructor = members.GetSourceConstructor();

            StopTimeMillisecondsGetMethod = members.GetSourcePropertyGetterOf(nameof(StopTimeMilliseconds));
            StopTimeMillisecondsSetMethod = members.GetSourcePropertySetterOf(nameof(StopTimeMilliseconds));

            AccelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(Acceleration));
            AccelerationSetMethod = members.GetSourcePropertySetterOf(nameof(Acceleration));

            DecelerationGetMethod = members.GetSourcePropertyGetterOf(nameof(Deceleration));
            DecelerationSetMethod = members.GetSourcePropertySetterOf(nameof(Deceleration));

            SpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(Speed));
            SpeedSetMethod = members.GetSourcePropertySetterOf(nameof(Speed));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TrainStopObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TrainStopObject(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TrainStopObject"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static TrainStopObject FromSource(object src) => src is null ? null : new TrainStopObject(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="TrainStopObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="deceleration">減速度 [m/s^2]。</param>
        /// <param name="stopTimeMilliseconds">停車時間 [ms]。</param>
        /// <param name="acceleration">加速度 [m/s^2]。</param>
        /// <param name="speed">加速後の走行速度 [m/s]。</param>
        public TrainStopObject(double location, double deceleration, int stopTimeMilliseconds, double acceleration, double speed)
            : this(Constructor.Invoke(new object[] { location, deceleration, stopTimeMilliseconds, acceleration, speed }))
        {
        }

        /// <summary>
        /// <see cref="TrainStopObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="deceleration">減速度 [m/s^2]。</param>
        /// <param name="stopTime">停車時間。</param>
        /// <param name="acceleration">加速度 [m/s^2]。</param>
        /// <param name="speed">加速後の走行速度 [m/s]。</param>
        public TrainStopObject(double location, double deceleration, TimeSpan stopTime, double acceleration, double speed)
            : this(location, deceleration, (int)stopTime.TotalMilliseconds, acceleration, speed)
        {
        }

        private static FastMethod StopTimeMillisecondsGetMethod;
        private static FastMethod StopTimeMillisecondsSetMethod;
        /// <summary>
        /// 停車時間をミリ秒単位で取得・設定します。
        /// </summary>
        public int StopTimeMilliseconds
        {
            get => StopTimeMillisecondsGetMethod.Invoke(Src, null);
            set => StopTimeMillisecondsSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 停車時間を取得・設定します。
        /// </summary>
        public TimeSpan StopTime
        {
            get => TimeSpan.FromMilliseconds(StopTimeMilliseconds);
            set => StopTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastMethod AccelerationGetMethod;
        private static FastMethod AccelerationSetMethod;
        /// <summary>
        /// 加速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double Acceleration
        {
            get => AccelerationGetMethod.Invoke(Src, null);
            set => AccelerationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DecelerationGetMethod;
        private static FastMethod DecelerationSetMethod;
        /// <summary>
        /// 減速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double Deceleration
        {
            get => DecelerationGetMethod.Invoke(Src, null);
            set => DecelerationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SpeedGetMethod;
        private static FastMethod SpeedSetMethod;
        /// <summary>
        /// 加速後の走行速度 [m/s] を取得・設定します。
        /// </summary>
        public double Speed
        {
            get => SpeedGetMethod.Invoke(Src, null);
            set => SpeedSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
