using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 停車場を表します。
    /// </summary>
    public class Station : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Station>();

            Constructor = members.GetSourceConstructor();

            NameGetMethod = members.GetSourcePropertyGetterOf(nameof(Name));
            NameSetMethod = members.GetSourcePropertySetterOf(nameof(Name));

            ArrivalTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(ArrivalTimeMilliseconds));
            ArrivalTimeSetMethod = members.GetSourcePropertySetterOf(nameof(ArrivalTimeMilliseconds));

            DepartureTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(DepartureTimeMilliseconds));
            DepartureTimeSetMethod = members.GetSourcePropertySetterOf(nameof(DepartureTimeMilliseconds));

            DoorCloseTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(DoorCloseTimeMilliseconds));
            DoorCloseTimeSetMethod = members.GetSourcePropertySetterOf(nameof(DoorCloseTimeMilliseconds));

            DefaultTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(DefaultTimeMilliseconds));
            DefaultTimeSetMethod = members.GetSourcePropertySetterOf(nameof(DefaultTimeMilliseconds));

            PassGetMethod = members.GetSourcePropertyGetterOf(nameof(Pass));
            PassSetMethod = members.GetSourcePropertySetterOf(nameof(Pass));

            IsTerminalGetMethod = members.GetSourcePropertyGetterOf(nameof(IsTerminal));
            IsTerminalSetMethod = members.GetSourcePropertySetterOf(nameof(IsTerminal));

            StoppageTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(StoppageTimeMilliseconds));
            StoppageTimeSetMethod = members.GetSourcePropertySetterOf(nameof(StoppageTimeMilliseconds));

            DoorSideGetMethod = members.GetSourcePropertyGetterOf(nameof(DoorSide));
            DoorSideSetMethod = members.GetSourcePropertySetterOf(nameof(DoorSide));

            DepartureSoundGetMethod = members.GetSourcePropertyGetterOf(nameof(DepartureSound));
            DepartureSoundSetMethod = members.GetSourcePropertySetterOf(nameof(DepartureSound));

            ArrivalSoundGetMethod = members.GetSourcePropertyGetterOf(nameof(ArrivalSound));
            ArrivalSoundSetMethod = members.GetSourcePropertySetterOf(nameof(ArrivalSound));

            SignalFlagGetMethod = members.GetSourcePropertyGetterOf(nameof(SignalFlag));
            SignalFlagSetMethod = members.GetSourcePropertySetterOf(nameof(SignalFlag));

            MarginMaxGetMethod = members.GetSourcePropertyGetterOf(nameof(MarginMax));
            MarginMaxSetMethod = members.GetSourcePropertySetterOf(nameof(MarginMax));

            MarginMinGetMethod = members.GetSourcePropertyGetterOf(nameof(MarginMin));
            MarginMinSetMethod = members.GetSourcePropertySetterOf(nameof(MarginMin));

            MinStopPositionGetMethod = members.GetSourcePropertyGetterOf(nameof(MinStopPosition));

            MaxStopPositionGetMethod = members.GetSourcePropertyGetterOf(nameof(MaxStopPosition));

            AlightingTimeGetMethod = members.GetSourcePropertyGetterOf(nameof(AlightingTimeMilliseconds));
            AlightingTimeSetMethod = members.GetSourcePropertySetterOf(nameof(AlightingTimeMilliseconds));

            TargetLoadFactorGetMethod = members.GetSourcePropertyGetterOf(nameof(TargetLoadFactor));
            TargetLoadFactorSetMethod = members.GetSourcePropertySetterOf(nameof(TargetLoadFactor));

            CurrentLoadFactorGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentLoadFactor));
            CurrentLoadFactorSetMethod = members.GetSourcePropertySetterOf(nameof(CurrentLoadFactor));

            DoorReopenGetMethod = members.GetSourcePropertyGetterOf(nameof(DoorReopen));
            DoorReopenSetMethod = members.GetSourcePropertySetterOf(nameof(DoorReopen));

            StuckInDoorGetMethod = members.GetSourcePropertyGetterOf(nameof(StuckInDoorMilliseconds));
            StuckInDoorSetMethod = members.GetSourcePropertySetterOf(nameof(StuckInDoorMilliseconds));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Station"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Station(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Station"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Station FromSource(object src) => src is null ? null : new Station(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// 停車場の名前を指定して <see cref="Station"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// <paramref name="name"/> には BVE 上に表示される名前を指定します。停車場名 (停車場リストファイルで定義した文字列) とは異なります。
        /// </remarks>
        /// <param name="name">停車場の名前。</param>
        public Station(string name) : this(Constructor.Invoke(new object[] { name }))
        {
        }

        private static FastMethod NameGetMethod;
        private static FastMethod NameSetMethod;
        /// <summary>
        /// 停車場の名前を取得・設定します。
        /// </summary>
        /// <remarks>
        /// BVE 上に表示される名前です。停車場名 (停車場リストファイルで定義した文字列) とは異なります。
        /// </remarks>
        public string Name
        {
            get => NameGetMethod.Invoke(Src, null);
            set => NameSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod ArrivalTimeGetMethod;
        private static FastMethod ArrivalTimeSetMethod;
        /// <summary>
        /// 到着時刻をミリ秒単位で取得・設定します。
        /// </summary>
        /// <value>0 時丁度から到着時刻までに経過したミリ秒数 [ms]。</value>
        public int ArrivalTimeMilliseconds
        {
            get => ArrivalTimeGetMethod.Invoke(Src, null);
            set => ArrivalTimeSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 到着時刻を取得・設定します。
        /// </summary>
        public TimeSpan ArrivalTime
        {
            get => TimeSpan.FromMilliseconds(ArrivalTimeMilliseconds);
            set => ArrivalTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastMethod DepartureTimeGetMethod;
        private static FastMethod DepartureTimeSetMethod;
        /// <summary>
        /// 発車時刻または通過時刻をミリ秒単位で取得・設定します。
        /// </summary>
        /// <value>0 時丁度から発車時刻または通過時刻までに経過したミリ秒数 [ms]。</value>
        public int DepartureTimeMilliseconds
        {
            get => DepartureTimeGetMethod.Invoke(Src, null);
            set => DepartureTimeSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 発車時刻または通過時刻を取得・設定します。
        /// </summary>
        public TimeSpan DepartureTime
        {
            get => TimeSpan.FromMilliseconds(DepartureTimeMilliseconds);
            set => DepartureTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        /// <summary>
        /// 発車時刻または通過時刻を取得・設定します。
        /// </summary>
        public TimeSpan DepertureTime
        {
            get => DepartureTime;
            set => DepartureTime = value;
        }

        private static FastMethod DoorCloseTimeGetMethod;
        private static FastMethod DoorCloseTimeSetMethod;
        /// <summary>
        /// ドアが閉まるのに要する時間をミリ秒単位で取得・設定します。
        /// </summary>
        public int DoorCloseTimeMilliseconds
        {
            get => DoorCloseTimeGetMethod.Invoke(Src, null);
            set => DoorCloseTimeSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// ドアが閉まるのに要する時間を取得・設定します。
        /// </summary>
        public TimeSpan DoorCloseTime
        {
            get => TimeSpan.FromMilliseconds(DoorCloseTimeMilliseconds);
            set => DoorCloseTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastMethod DefaultTimeGetMethod;
        private static FastMethod DefaultTimeSetMethod;
        /// <summary>
        /// 駅にジャンプしたときの時刻をミリ秒単位で取得・設定します。
        /// </summary>
        /// <value>0 時丁度から駅にジャンプしたときの時刻までに経過したミリ秒数 [ms]。</value>
        public int DefaultTimeMilliseconds
        {
            get => DefaultTimeGetMethod.Invoke(Src, null);
            set => DefaultTimeSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 駅にジャンプしたときの時刻を取得・設定します。
        /// </summary>
        public TimeSpan DefaultTime
        {
            get => TimeSpan.FromMilliseconds(DefaultTimeMilliseconds);
            set => DefaultTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastMethod PassGetMethod;
        private static FastMethod PassSetMethod;
        /// <summary>
        /// この停車場を通過するかどうかを取得・設定します。
        /// </summary>
        public bool Pass
        {
            get => PassGetMethod.Invoke(Src, null);
            set => PassSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod IsTerminalGetMethod;
        private static FastMethod IsTerminalSetMethod;
        /// <summary>
        /// この停車場が終点かどうかを取得・設定します。
        /// </summary>
        public bool IsTerminal
        {
            get => IsTerminalGetMethod.Invoke(Src, null);
            set => IsTerminalSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod StoppageTimeGetMethod;
        private static FastMethod StoppageTimeSetMethod;
        /// <summary>
        /// 標準停車時間をミリ秒単位で取得・設定します。
        /// </summary>
        public int StoppageTimeMilliseconds
        {
            get => StoppageTimeGetMethod.Invoke(Src, null);
            set => StoppageTimeSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 標準停車時間を取得・設定します。
        /// </summary>
        public TimeSpan StoppageTime
        {
            get => TimeSpan.FromMilliseconds(StoppageTimeMilliseconds);
            set => StoppageTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastMethod DoorSideGetMethod;
        private static FastMethod DoorSideSetMethod;
        /// <summary>
        /// 開くドアの方向を表す整数を取得・設定します。
        /// </summary>
        public int DoorSide
        {
            get => DoorSideGetMethod.Invoke(Src, null);
            set => DoorSideSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 開くドアの方向を <see cref="ClassWrappers.DoorSide"/>? 型で取得・設定します。
        /// </summary>
        /// <remarks>
        /// どちら側のドアも開かないことを表すには <see langword="null"/> を使用します。
        /// </remarks>
        public DoorSide? DoorSideEnum
        {
            get
            {
                switch (DoorSide)
                {
                    case -1:
                        return ClassWrappers.DoorSide.Left;
                    case 1:
                        return ClassWrappers.DoorSide.Right;
                    default:
                        return null;
                }
            }
            set
            {
                switch (value)
                {
                    case ClassWrappers.DoorSide.Left:
                        DoorSide = -1;
                        break;
                    case ClassWrappers.DoorSide.Right:
                        DoorSide = 1;
                        break;
                    default:
                        DoorSide = 0;
                        break;
                }
            }
        }

        private static FastMethod DepartureSoundGetMethod;
        private static FastMethod DepartureSoundSetMethod;
        /// <summary>
        /// <see cref="DepartureTime"/> の <see cref="StoppageTime"/> 前の時刻に再生されるサウンドを取得・設定します。
        /// </summary>
        public Sound DepartureSound
        {
            get => Sound.FromSource(DepartureSoundGetMethod.Invoke(Src, null));
            set => DepartureSoundSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod ArrivalSoundGetMethod;
        private static FastMethod ArrivalSoundSetMethod;
        /// <summary>
        /// ドアが開いたときに再生されるサウンドを取得・設定します。
        /// </summary>
        public Sound ArrivalSound
        {
            get => Sound.FromSource(ArrivalSoundGetMethod.Invoke(Src, null));
            set => ArrivalSoundSetMethod.Invoke(Src, new object[] { value?.Src });
        }

        private static FastMethod SignalFlagGetMethod;
        private static FastMethod SignalFlagSetMethod;
        /// <summary>
        /// <see cref="DepartureTime"/> の <see cref="StoppageTime"/> 前の時刻まで出発信号が停止を現示するかどうかを取得・設定します。
        /// </summary>
        public bool SignalFlag
        {
            get => SignalFlagGetMethod.Invoke(Src, null);
            set => SignalFlagSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MarginMaxGetMethod;
        private static FastMethod MarginMaxSetMethod;
        /// <summary>
        /// 停止位置誤差の前方許容範囲 [m] を取得・設定します。
        /// </summary>
        public double MarginMax
        {
            get => MarginMaxGetMethod.Invoke(Src, null);
            set => MarginMaxSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MarginMinGetMethod;
        private static FastMethod MarginMinSetMethod;
        /// <summary>
        /// 停止位置誤差の後方許容範囲 [m] を負の値で取得・設定します。
        /// </summary>
        public double MarginMin
        {
            get => MarginMinGetMethod.Invoke(Src, null);
            set => MarginMinSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod MinStopPositionGetMethod;
        /// <summary>
        /// 停止位置誤差の下限の距離程 [m] を取得します。
        /// </summary>
        public double MinStopPosition => MinStopPositionGetMethod.Invoke(Src, null);

        private static FastMethod MaxStopPositionGetMethod;
        /// <summary>
        /// 停止位置誤差の上限の距離程 [m] を取得します。
        /// </summary>
        public double MaxStopPosition => MaxStopPositionGetMethod.Invoke(Src, null);

        private static FastMethod AlightingTimeGetMethod;
        private static FastMethod AlightingTimeSetMethod;
        /// <summary>
        /// 降車時間をミリ秒単位で取得・設定します。
        /// </summary>
        public int AlightingTimeMilliseconds
        {
            get => AlightingTimeGetMethod.Invoke(Src, null);
            set => AlightingTimeSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 降車時間を取得・設定します。
        /// </summary>
        public TimeSpan AlightingTime
        {
            get => TimeSpan.FromMilliseconds(AlightingTimeMilliseconds);
            set => AlightingTimeMilliseconds = (int)value.TotalMilliseconds;
        }

        private static FastMethod TargetLoadFactorGetMethod;
        private static FastMethod TargetLoadFactorSetMethod;
        /// <summary>
        /// 出発時の乗車率を取得・設定します。
        /// </summary>
        public double TargetLoadFactor
        {
            get => TargetLoadFactorGetMethod.Invoke(Src, null);
            set => TargetLoadFactorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CurrentLoadFactorGetMethod;
        private static FastMethod CurrentLoadFactorSetMethod;
        /// <summary>
        /// 到着時の乗車率を取得・設定します。
        /// </summary>
        public double CurrentLoadFactor
        {
            get => CurrentLoadFactorGetMethod.Invoke(Src, null);
            set => CurrentLoadFactorSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod DoorReopenGetMethod;
        private static FastMethod DoorReopenSetMethod;
        /// <summary>
        /// ドアが再開閉される確率を取得・設定します。
        /// </summary>
        public double DoorReopen
        {
            get => DoorReopenGetMethod.Invoke(Src, null);
            set => DoorReopenSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod StuckInDoorGetMethod;
        private static FastMethod StuckInDoorSetMethod;
        /// <summary>
        /// 旅客がドアに挟まる時間をミリ秒単位で取得・設定します。
        /// </summary>
        public int StuckInDoorMilliseconds
        {
            get => StuckInDoorGetMethod.Invoke(Src, null);
            set => StuckInDoorSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// 旅客がドアに挟まる時間を取得・設定します。
        /// </summary>
        public TimeSpan StuckInDoor
        {

            get => TimeSpan.FromMilliseconds(StoppageTimeMilliseconds);
            set => StoppageTimeMilliseconds = (int)value.TotalMilliseconds;
        }
    }
}
