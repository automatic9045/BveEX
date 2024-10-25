using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.Extensions.ConductorPatch
{
    /// <summary>
    /// 車掌動作を上書きするための基本クラスを表します。
    /// </summary>
    public abstract class ConductorBase
    {
        /// <summary>
        /// オリジナルの車掌動作が定義されている <see cref="Conductor"/> です。
        /// </summary>
        protected readonly Conductor Original;

        /// <summary>
        /// 停止位置の修正を要求したことを通知します。
        /// </summary>
        protected abstract event EventHandler FixStopPositionRequested;

        /// <summary>
        /// 停止位置が良いことを確認したことを通知します。
        /// </summary>
        protected abstract event EventHandler StopPositionChecked;

        /// <summary>
        /// ドアスイッチを開方向に扱ったことを通知します。
        /// </summary>
        protected abstract event EventHandler DoorOpening;

        /// <summary>
        /// 発車ベルスイッチを扱ったこと、つまり停車場リストで定義した departureSound の再生を開始することを通知します。
        /// </summary>
        protected abstract event EventHandler DepartureSoundPlaying;

        /// <summary>
        /// ドアスイッチを閉方向に扱ったことを通知します。
        /// </summary>
        protected abstract event EventHandler DoorClosing;

        /// <summary>
        /// 車側灯の滅灯を確認したこと、つまり全てのドアが閉まりきったことを通知します。
        /// </summary>
        protected abstract event EventHandler DoorClosed;

        /// <summary>
        /// <see cref="ConductorBase"/> クラスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="original">オリジナルの車掌動作が定義されている <see cref="Conductor"/>。</param>
        protected ConductorBase(Conductor original)
        {
            Original = original;

            FixStopPositionRequested += (sender, e) => Original.FixStopPositionRequested_Invoke();
            StopPositionChecked += (sender, e) => Console.WriteLine("車掌: 停止位置よし");
            DoorOpening += (sender, e) => Console.WriteLine("車掌スイッチ: 開");
            DepartureSoundPlaying += (sender, e) => Console.WriteLine("発車ベル: ON");
            DoorClosing += (sender, e) => Console.WriteLine("車掌スイッチ: 閉");
            DoorClosed += (sender, e) =>
            {
                Console.WriteLine("側灯滅");
                Original.DepartureRequested_Invoke();
            };
        }

        /// <summary>
        /// 自列車がテレポートしたときに呼び出されます。
        /// </summary>
        /// <param name="nextStationIndex">ジャンプ先の距離程における次駅のインデックス。</param>
        /// <param name="isDoorClosed">ドアが閉まっているかどうか。</param>
        /// <returns>オリジナルの処理をオーバーライドする方法。</returns>
        internal protected virtual MethodOverrideMode OnJumped(int nextStationIndex, bool isDoorClosed)
        {
            return MethodOverrideMode.RunOriginal;
        }

        /// <summary>
        /// ドアの状態が変更されたときに呼び出されます。
        /// </summary>
        /// <returns>オリジナルの処理をオーバーライドする方法。</returns>
        internal protected virtual MethodOverrideMode OnDoorStateChanged()
        {
            return MethodOverrideMode.RunOriginal;
        }

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <returns>オリジナルの処理をオーバーライドする方法。</returns>
        internal protected virtual MethodOverrideMode OnTick()
        {
            return MethodOverrideMode.RunOriginal;
        }
    }
}
