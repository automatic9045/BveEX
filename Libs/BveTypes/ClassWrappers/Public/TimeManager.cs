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
    /// 時間に関する処理を行います。
    /// </summary>
    public partial class TimeManager : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TimeManager>();

            StateGetMethod = members.GetSourcePropertyGetterOf(nameof(State));
            StateSetMethod = members.GetSourcePropertySetterOf(nameof(State));

            TimeMillisecondsGetMethod = members.GetSourcePropertyGetterOf(nameof(TimeMilliseconds));

            SetTimeMethod = members.GetSourceMethodOf(nameof(SetTime));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TimeManager"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TimeManager(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TimeManager"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static TimeManager FromSource(object src) => src is null ? null : new TimeManager(src);

        private static FastMethod StateGetMethod;
        private static FastMethod StateSetMethod;
        /// <summary>
        /// 時間進行の設定を取得・設定します。
        /// </summary>
        public GameState State
        {
            get => (GameState)StateGetMethod.Invoke(Src, null);
            set => StateSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TimeMillisecondsGetMethod;
        /// <summary>
        /// 0 時丁度から現在までに経過した時間をミリ秒単位で取得します。
        /// </summary>
        /// <remarks>
        /// 現在時刻の変更には <see cref="SetTime(int)"/> メソッド、<see cref="SetTime(TimeSpan)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="Time"/>
        /// <seealso cref="SetTime(int)"/>
        /// <seealso cref="SetTime(TimeSpan)"/>
        public int TimeMilliseconds => (int)TimeMillisecondsGetMethod.Invoke(Src, null);

        /// <summary>
        /// 0 時丁度から現在までに経過した時間を表す <see cref="TimeSpan"/> を取得します。
        /// </summary>
        /// <remarks>
        /// 現在時刻の変更には <see cref="SetTime(int)"/> メソッド、<see cref="SetTime(TimeSpan)"/> メソッドを使用してください。
        /// </remarks>
        /// <seealso cref="TimeMilliseconds"/>
        /// <seealso cref="SetTime(int)"/>
        /// <seealso cref="SetTime(TimeSpan)"/>
        public TimeSpan Time => TimeSpan.FromMilliseconds(TimeMilliseconds);

        private static FastMethod SetTimeMethod;
        /// <summary>
        /// 現在時刻を変更します。
        /// </summary>
        /// <param name="timeMilliseconds">0 時丁度から現在までに経過したミリ秒数 [ms]。</param>
        /// <remarks>
        /// 現在時刻の取得には <see cref="TimeMilliseconds"/> プロパティ、<see cref="Time"/> プロパティを使用してください。
        /// </remarks>
        /// <seealso cref="TimeMilliseconds"/>
        /// <seealso cref="Time"/>
        public void SetTime(int timeMilliseconds) => SetTimeMethod.Invoke(Src, new object[] { timeMilliseconds });

        /// <summary>
        /// 現在時刻を変更します。
        /// </summary>
        /// <param name="time">0 時丁度から現在までに経過した時間。</param>
        /// <remarks>
        /// 現在時刻の取得には <see cref="TimeMilliseconds"/> プロパティ、<see cref="Time"/> プロパティを使用してください。
        /// </remarks>
        /// <see cref="TimeMilliseconds"/>
        /// <see cref="Time"/>
        public void SetTime(TimeSpan time) => SetTime((int)time.TotalMilliseconds);
    }
}
