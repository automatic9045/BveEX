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
    /// 他列車の運行スケジュールを表します。
    /// </summary>
    /// <remarks>
    /// このクラスのオリジナル型は構造体であることに注意してください。
    /// </remarks>
    public class TrainSchedule : ClassWrapperBase
    {
        private static Type OriginalType;

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TrainSchedule>();

            OriginalType = members.OriginalType;

            StartTimeMillisecondsField = members.GetSourceFieldOf(nameof(StartTimeMilliseconds));
            OriginTimeMillisecondsField = members.GetSourceFieldOf(nameof(OriginTimeMilliseconds));
            BaseLocationField = members.GetSourceFieldOf(nameof(Location));
            SpeedField = members.GetSourceFieldOf(nameof(Speed));
            AccelerationField = members.GetSourceFieldOf(nameof(Acceleration));

            GetLocationMethod = members.GetSourceMethodOf(nameof(GetLocation));
            GetSpeedMethod = members.GetSourceMethodOf(nameof(GetSpeed));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TrainSchedule"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TrainSchedule(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TrainSchedule"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static TrainSchedule FromSource(object src) => src is null ? null : new TrainSchedule(src);

        /// <summary>
        /// <see cref="TrainSchedule"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="startTimeMilliseconds">このスケジュールの開始時刻 [ms]。</param>
        /// <param name="originTimeMilliseconds">このスケジュールの計算上の基準時刻 [ms]。</param>
        /// <param name="location">基準時刻 (<paramref name="originTimeMilliseconds"/>) における距離程 [m]。</param>
        /// <param name="speed">基準時刻 (<paramref name="originTimeMilliseconds"/>) における速度 [m/s]。</param>
        /// <param name="acceleration">加速度 [m/s^2]。</param>
        public TrainSchedule(int startTimeMilliseconds, int originTimeMilliseconds, double location, double speed, double acceleration)
            : this(Activator.CreateInstance(OriginalType, startTimeMilliseconds, originTimeMilliseconds, location, speed, acceleration ))
        {
        }

        /// <summary>
        /// <see cref="TrainSchedule"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="startTime">このスケジュールの開始時刻。</param>
        /// <param name="originTime">このスケジュールの計算上の基準時刻。</param>
        /// <param name="location">基準時刻 (<paramref name="originTime"/>) における距離程 [m]。</param>
        /// <param name="speed">基準時刻 (<paramref name="originTime"/>) における速度 [m/s]。</param>
        /// <param name="acceleration">加速度 [m/s^2]。</param>
        public TrainSchedule(TimeSpan startTime, TimeSpan originTime, double location, double speed, double acceleration)
            : this((int)startTime.TotalMilliseconds, (int)originTime.TotalMilliseconds, location, speed, acceleration)
        {
        }

        private static FastField StartTimeMillisecondsField;
        /// <summary>
        /// このスケジュールの開始時刻をミリ秒単位で取得します。
        /// </summary>
        public int StartTimeMilliseconds => (int)StartTimeMillisecondsField.GetValue(Src);

        /// <summary>
        /// このスケジュールの開始時刻を取得します。
        /// </summary>
        public TimeSpan StartTime => TimeSpan.FromMilliseconds(StartTimeMilliseconds);

        private static FastField OriginTimeMillisecondsField;
        /// <summary>
        /// このスケジュールの計算上の基準時刻をミリ秒単位で取得します。
        /// </summary>
        public int OriginTimeMilliseconds => (int)OriginTimeMillisecondsField.GetValue(Src);

        /// <summary>
        /// このスケジュールの計算上の基準時刻を取得します。
        /// </summary>
        public TimeSpan OriginTime => TimeSpan.FromMilliseconds(OriginTimeMilliseconds);

        private static FastField BaseLocationField;
        /// <summary>
        /// 基準時刻 (<see cref="OriginTime"/> プロパティの値) における距離程 [m] を取得します。
        /// </summary>
        public double Location => (double)BaseLocationField.GetValue(Src);

        private static FastField SpeedField;
        /// <summary>
        /// 基準時刻 (<see cref="OriginTime"/> プロパティの値) における速度 [m/s] を取得・設定します。
        /// </summary>
        public double Speed => (double)SpeedField.GetValue(Src);

        private static FastField AccelerationField;
        /// <summary>
        /// 加速度 [m/s^2] を取得・設定します。
        /// </summary>
        public double Acceleration => (double)AccelerationField.GetValue(Src);


        private static FastMethod GetLocationMethod;
        /// <summary>
        /// 指定した時刻における距離程を計算します。
        /// </summary>
        /// <param name="timeMilliseconds">時刻 [ms]。</param>
        /// <returns>時刻 <paramref name="timeMilliseconds"/> [ms] における距離程 [m]。</returns>
        public double GetLocation(int timeMilliseconds)
            => (double)GetLocationMethod.Invoke(Src, new object[] { timeMilliseconds });

        /// <summary>
        /// 指定した時刻における距離程を計算します。
        /// </summary>
        /// <param name="time">時刻。</param>
        /// <returns>時刻 <paramref name="time"/> における距離程 [m]。</returns>
        public double GetLocation(TimeSpan time) => GetLocation((int)time.TotalMilliseconds);

        private static FastMethod GetSpeedMethod;
        /// <summary>
        /// 指定した時刻における速度を計算します。
        /// </summary>
        /// <param name="timeMilliseconds">時刻 [ms]。</param>
        /// <returns>時刻 <paramref name="timeMilliseconds"/> [ms] における速度 [m/s]。</returns>
        public double GetSpeed(int timeMilliseconds)
            => (double)GetSpeedMethod.Invoke(Src, new object[] { timeMilliseconds });

        /// <summary>
        /// 指定した時刻における速度を計算します。
        /// </summary>
        /// <param name="time">時刻。</param>
        /// <returns>時刻 <paramref name="time"/> における速度 [m/s]。</returns>
        public double GetSpeed(TimeSpan time) => GetSpeed((int)time.TotalMilliseconds);
    }
}
