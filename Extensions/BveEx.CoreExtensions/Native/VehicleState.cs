using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.Native
{
    /// <summary>
    /// 車両の状態量を提供します。
    /// </summary>
    public class VehicleState
    {
        /// <summary>
        /// <see cref="VehicleState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">列車位置 [m]。</param>
        /// <param name="speed">列車速度 [km/h]。</param>
        /// <param name="time">現在時刻。</param>
        /// <param name="bcPressure">ブレーキシリンダ圧力 [kPa]。</param>
        /// <param name="mrPressure">元空気ダメ圧力 [kPa]。</param>
        /// <param name="erPressure">釣り合い空気ダメ圧力 [kPa]。</param>
        /// <param name="bpPressure">釣り合い空気ダメ圧力 [kPa]。</param>
        /// <param name="sapPressure">直通管圧力 [kPa]。</param>
        /// <param name="current">電流 [A]。</param>
        public VehicleState(double location, float speed, TimeSpan time, float bcPressure, float mrPressure, float erPressure, float bpPressure, float sapPressure, float current)
        {
            Location = location;
            Speed = speed;
            Time = time;

            BcPressure = bcPressure;
            MrPressure = mrPressure;
            ErPressure = erPressure;
            BpPressure = bpPressure;
            SapPressure = sapPressure;
            Current = current;
        }


        /// <summary>
        /// 列車位置 [m] を取得します。
        /// </summary>
        public double Location { get; }

        /// <summary>
        /// 列車速度 [km/h] を取得します。
        /// </summary>
        public float Speed { get; }

        /// <summary>
        /// 現在時刻を取得します。
        /// </summary>
        public TimeSpan Time { get; }


        /// <summary>
        /// ブレーキシリンダ圧力 [kPa] を取得します。
        /// </summary>
        public float BcPressure { get; }

        /// <summary>
        /// 元空気ダメ圧力 [kPa] を取得します。
        /// </summary>
        public float MrPressure { get; }

        /// <summary>
        /// 釣り合い空気ダメ圧力 [kPa] を取得します。
        /// </summary>
        public float ErPressure { get; }

        /// <summary>
        /// ブレーキ管圧力 [kPa] を取得します。
        /// </summary>
        public float BpPressure { get; }

        /// <summary>
        /// 直通管圧力 [kPa] を取得します。
        /// </summary>
        public float SapPressure { get; }

        /// <summary>
        /// 電流 [A] を取得します。
        /// </summary>
        public float Current { get; }
    }
}
