using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 他列車の情報を提供します。
    /// </summary>
    public class TrainInfo : MapObjectList
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TrainInfo>();

            Constructor = members.GetSourceConstructor();

            StructuresGetMethod = members.GetSourcePropertyGetterOf(nameof(Structures));
            SoundsGetMethod = members.GetSourcePropertyGetterOf(nameof(Sounds));

            DirectionGetMethod = members.GetSourcePropertyGetterOf(nameof(Direction));
            DirectionSetMethod = members.GetSourcePropertySetterOf(nameof(Direction));

            TrackKeyGetMethod = members.GetSourcePropertyGetterOf(nameof(TrackKey));
            TrackKeySetMethod = members.GetSourcePropertySetterOf(nameof(TrackKey));

            EnableLocationGetMethod = members.GetSourcePropertyGetterOf(nameof(EnableLocation));
            EnableLocationSetMethod = members.GetSourcePropertySetterOf(nameof(EnableLocation));

            EnableTimeMillisecondsGetMethod = members.GetSourcePropertyGetterOf(nameof(EnableTimeMilliseconds));
            EnableTimeMillisecondsSetMethod = members.GetSourcePropertySetterOf(nameof(EnableTimeMilliseconds));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TrainInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TrainInfo(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TrainInfo"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new TrainInfo FromSource(object src) => src is null ? null : new TrainInfo((IList)src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="TrainInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public TrainInfo()
            : this((IList)Constructor.Invoke(null))
        {
        }

        private static FastMethod StructuresGetMethod;
        /// <summary>
        /// この他列車に紐づけるストラクチャーの一覧を取得・設定します。
        /// </summary>
        public WrappedList<Structure> Structures => WrappedList<Structure>.FromSource((IList)StructuresGetMethod.Invoke(Src, null));

        private static FastMethod SoundsGetMethod;
        /// <summary>
        /// この他列車に紐づける音源の一覧を取得・設定します。
        /// </summary>
        public WrappedList<Sound3DObject> Sounds => WrappedList<Sound3DObject>.FromSource((IList)SoundsGetMethod.Invoke(Src, null));

        private static FastMethod DirectionGetMethod;
        private static FastMethod DirectionSetMethod;
        /// <summary>
        /// この他列車の進行方向を取得・設定します。
        /// </summary>
        public int Direction
        {
            get => (int)DirectionGetMethod.Invoke(Src, null);
            set => DirectionSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TrackKeyGetMethod;
        private static FastMethod TrackKeySetMethod;
        /// <summary>
        /// この他列車が走行する軌道を取得・設定します。
        /// </summary>
        public string TrackKey
        {
            get => (string)TrackKeyGetMethod.Invoke(Src, null);
            set => TrackKeySetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod EnableLocationGetMethod;
        private static FastMethod EnableLocationSetMethod;
        /// <summary>
        /// この他列車の動作を有効にする距離程 [m] を取得・設定します。
        /// </summary>
        public double EnableLocation
        {
            get => (double)EnableLocationGetMethod.Invoke(Src, null);
            set => EnableLocationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod EnableTimeMillisecondsGetMethod;
        private static FastMethod EnableTimeMillisecondsSetMethod;
        /// <summary>
        /// この他列車の動作を有効にする時刻をミリ秒単位で取得・設定します。
        /// </summary>
        public int EnableTimeMilliseconds
        {
            get => (int)EnableTimeMillisecondsGetMethod.Invoke(Src, null);
            set => EnableTimeMillisecondsSetMethod.Invoke(Src, new object[] { value });
        }

        /// <summary>
        /// この他列車の動作を有効にする時刻を取得・設定します。
        /// </summary>
        public TimeSpan EnableTime
        {
            get => TimeSpan.FromMilliseconds(EnableTimeMilliseconds);
            set => EnableTimeMilliseconds = (int)value.TotalMilliseconds;
        }
    }
}
