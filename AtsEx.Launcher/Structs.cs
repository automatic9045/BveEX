using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Launcher
{
    /// <summary>列車のスペックに関する構造体</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VehicleSpec
    {
        /// <summary>ブレーキ段数</summary>
        public int BrakeNotches;
        /// <summary>力行ノッチ段数</summary>
        public int PowerNotches;
        /// <summary>ATS確認段数</summary>
        public int AtsNotch;
        /// <summary>常用最大段数</summary>
        public int B67Notch;
        /// <summary>編成車両数</summary>
        public int Cars;
    };

    /// <summary>列車状態に関する構造体</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VehicleState
    {
        /// <summary>列車位置[m]</summary>
        public double Location;
        /// <summary>列車速度[km/h]</summary>
        public float Speed;
        /// <summary>0時からの経過時間[ms]</summary>
        public int Time;
        /// <summary>BC圧力[kPa]</summary>
        public float BcPressure;
        /// <summary>MR圧力[kPa]</summary>
        public float MrPressure;
        /// <summary>ER圧力[kPa]</summary>
        public float ErPressure;
        /// <summary>BP圧力[kPa]</summary>
        public float BpPressure;
        /// <summary>SAP圧力[kPa]</summary>
        public float SapPressure;
        /// <summary>電流[A]</summary>
        public float Current;
    };

    /// <summary>ハンドル位置に関する構造体</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AtsHandles
    {
        /// <summary>ブレーキハンドル位置</summary>
        public int Brake;
        /// <summary>力行ノッチハンドル位置</summary>
        public int Power;
        /// <summary>レバーサーハンドル位置</summary>
        public int Reverser;
        /// <summary>定速制御状態</summary>
        public int ConstantSpeed;
    };

    /// <summary>Beaconに関する構造体</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BeaconData
    {
        /// <summary>Beaconの番号</summary>
        public int Num;
        /// <summary>対応する閉塞の現示番号</summary>
        public int Sig;
        /// <summary>対応する閉塞までの距離[m]</summary>
        public float Z;
        /// <summary>Beaconの第三引数の値</summary>
        public int Data;
    };
}
