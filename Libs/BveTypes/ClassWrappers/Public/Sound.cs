using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;
using SlimDX.DirectSound;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// サウンドを表します。
    /// </summary>
    public partial class Sound : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Sound>();

            Constructor1 = members.GetSourceConstructor(new Type[] { typeof(TimeManager), typeof(CameraLocation), typeof(SecondarySoundBuffer), typeof(double), typeof(SoundPosition) });
            Constructor2 = members.GetSourceConstructor(new Type[] { typeof(TimeManager), typeof(CameraLocation), typeof(SecondarySoundBuffer[]), typeof(double), typeof(SoundPosition) });

            PositionInBlockGetMethod = members.GetSourcePropertyGetterOf(nameof(PositionInBlock));
            PositionInBlockSetMethod = members.GetSourcePropertySetterOf(nameof(PositionInBlock));

            SpeedInBlockGetMethod = members.GetSourcePropertyGetterOf(nameof(SpeedInBlock));
            SpeedInBlockSetMethod = members.GetSourcePropertySetterOf(nameof(SpeedInBlock));

            PlayMethod1 = members.GetSourceMethodOf(nameof(Play), new Type[] { typeof(double), typeof(double), typeof(int) });
            PlayMethod2 = members.GetSourceMethodOf(nameof(Play), new Type[] { typeof(double), typeof(double), typeof(int), typeof(int) });
            PlayLoopingMethod = members.GetSourceMethodOf(nameof(PlayLooping));
            StopMethod = members.GetSourceMethodOf(nameof(Stop));
            SetVolumeAndPitchMethod = members.GetSourceMethodOf(nameof(SetVolumeAndPitch));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Sound"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Sound(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Sound"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Sound FromSource(object src) => src is null ? null : new Sound(src);

        private static FastConstructor Constructor1;
        public Sound(TimeManager timeManager, CameraLocation cameraLocation, SecondarySoundBuffer buffer, double minRadius, SoundPosition position)
            : this(Constructor1.Invoke(new object[] { timeManager, cameraLocation, buffer, minRadius, position }))
        {
        }

        private static FastConstructor Constructor2;
        public Sound(TimeManager timeManager, CameraLocation cameraLocation, SecondarySoundBuffer[] buffers, double minRadius, SoundPosition position)
            : this(Constructor2.Invoke(new object[] { timeManager, cameraLocation, buffers, minRadius, position }))
        {
        }

        private static FastMethod PositionInBlockGetMethod;
        private static FastMethod PositionInBlockSetMethod;
        /// <summary>
        /// 現在自列車が走行しているマップ ブロックの原点に対する、この音声の再生位置の位置ベクトル [m] を取得します。
        /// </summary>
        public Vector3 PositionInBlock
        {
            get => PositionInBlockGetMethod.Invoke(Src, null);
            set => PositionInBlockSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SpeedInBlockGetMethod;
        private static FastMethod SpeedInBlockSetMethod;
        /// <summary>
        /// 現在自列車が走行しているマップ ブロックに対する、この音声の速度ベクトル [m/s] を取得します。
        /// </summary>
        public Vector3 SpeedInBlock
        {
            get => SpeedInBlockGetMethod.Invoke(Src, null);
            set => SpeedInBlockSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod PlayMethod1;
        /// <summary>
        /// 音声を冒頭から再生します。
        /// </summary>
        /// <param name="volume">音声を再生する音量。</param>
        /// <param name="pitch">音声を再生するピッチ。</param>
        /// <param name="fadeTimeMilliseconds">音量のフェードインにかける時間 [ms]。</param>
        public void Play(double volume, double pitch, int fadeTimeMilliseconds)
            => PlayMethod1.Invoke(Src, new object[] { volume, pitch, fadeTimeMilliseconds });

        private static FastMethod PlayMethod2;
        /// <summary>
        /// 音声を指定した位置から再生します。
        /// </summary>
        /// <param name="volume">音声を再生する音量。</param>
        /// <param name="pitch">音声を再生するピッチ。</param>
        /// <param name="fadeTimeMilliseconds">音量のフェードインにかける時間 [ms]。</param>
        /// <param name="playPositionBytes">音声の再生を開始する位置 [bytes]。</param>
        public void Play(double volume, double pitch, int fadeTimeMilliseconds, int playPositionBytes)
            => PlayMethod2.Invoke(Src, new object[] { volume, pitch, fadeTimeMilliseconds, playPositionBytes });

        private static FastMethod PlayLoopingMethod;
        /// <summary>
        /// 音声をループ再生します。
        /// </summary>
        /// <param name="volume">音声を再生する音量。</param>
        /// <param name="pitch">音声を再生するピッチ。</param>
        /// <param name="fadeTimeMilliseconds">音量のフェードインにかける時間 [ms]。</param>
        /// <param name="playPositionBytes">音声の再生を開始する位置 [bytes]。</param>
        public void PlayLooping(double volume, double pitch, int fadeTimeMilliseconds, int playPositionBytes)
            => PlayLoopingMethod.Invoke(Src, new object[] { volume, pitch, fadeTimeMilliseconds, playPositionBytes });

        private static FastMethod SetVolumeAndPitchMethod;
        /// <summary>
        /// 音声を再生する音量とピッチを設定します。
        /// </summary>
        /// <param name="volume">音声を再生する音量。</param>
        /// <param name="pitch">音声を再生するピッチ。</param>
        public void SetVolumeAndPitch(double volume, double pitch)
            => SetVolumeAndPitchMethod.Invoke(Src, new object[] { volume, pitch });

        private static FastMethod StopMethod;
        /// <summary>
        /// 音声の再生を停止します。
        /// </summary>
        /// <param name="fadeTimeMilliseconds">音量のフェードアウトにかける時間 [ms]。</param>
        public void Stop(int fadeTimeMilliseconds)
            => StopMethod.Invoke(Src, new object[] { fadeTimeMilliseconds });
    }
}
