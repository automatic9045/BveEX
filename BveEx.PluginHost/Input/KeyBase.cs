using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Input
{
    /// <summary>
    /// すべてのキーの基本クラスを表します。
    /// このクラスはスレッド セーフです。
    /// </summary>
    public abstract class KeyBase
    {
        /// <summary>
        /// キーが押されているかを取得します。
        /// </summary>
        public bool IsPressed { get; protected set; }

        /// <summary>
        /// キーが押されてから経過した時間 [ms] を取得します。
        /// キーが押されていない場合は 0 を返します。
        /// </summary>
        public long ElapsedMillisecondsSincePressed => IsPressed ? Stopwatch.ElapsedMilliseconds : 0;
        /// <summary>
        /// キーが押されてから経過した時間を取得します。
        /// キーが押されていない場合は <see cref="TimeSpan.Zero"/> を返します。
        /// </summary>
        public TimeSpan ElapsedTimeSincePressed => IsPressed ? Stopwatch.Elapsed : TimeSpan.Zero;

        /// <summary>
        /// キーが押された瞬間に発生します。
        /// </summary>
        public event EventHandler Pressed;
        /// <summary>
        /// キーが離され、<see cref="IsPressed"/> に <see langword="false"/> が設定される直前に発生します。
        /// </summary>
        /// <remarks>
        /// このイベントが発生した時点で経過時間を取得するための <see cref="System.Diagnostics.Stopwatch"/> は停止しているため、<br/>
        /// <see cref="ElapsedMillisecondsSincePressed"/> プロパティ、<see cref="ElapsedTimeSincePressed"/> プロパティから取得できる値は常に不変です。
        /// </remarks>
        /// <seealso cref="Released"/>
        public event EventHandler PreviewReleased;
        /// <summary>
        /// キーが離された瞬間に発生します。
        /// </summary>
        /// <remarks>
        /// キーが押されてから離されるまでに経過した時間を取得するには、<see cref="PreviewReleased"/> イベントを使用してください。
        /// </remarks>
        /// <seealso cref="PreviewReleased"/>
        public event EventHandler Released;

        /// <summary>
        /// キーが押されてから経過した時間を提供するための <see cref="System.Diagnostics.Stopwatch"/> です。
        /// </summary>
        protected readonly Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// スレッド セーフを保証するためのロック オブジェクトです。
        /// </summary>
        protected readonly object LockObj = new object();

        /// <summary>
        /// <see cref="KeyBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public KeyBase()
        {
        }

        /// <summary>
        /// この <see cref="KeyBase"/> オブジェクトに対するロックを取得し、指定したデリゲートを実行します。<br/>
        /// <see cref="NotifyPressed"/>、<see cref="NotifyReleased"/> を含むデリゲートを指定するとデッドロックするので注意してください。
        /// </summary>
        /// <param name="action">実行する <see cref="Action"/> デリゲート。</param>
        public void LockAndInvoke(Action action)
        {
            lock (LockObj) action();
        }

        /// <summary>
        /// キーが押されたことをこの <see cref="KeyBase"/> オブジェクトに通知します。
        /// </summary>
        protected void NotifyPressed()
        {
            lock (LockObj)
            {
                if (IsPressed) return;

                IsPressed = true;
                Stopwatch.Restart();
                Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// キーが離されたことをこの <see cref="KeyBase"/> オブジェクトに通知します。
        /// </summary>
        protected void NotifyReleased()
        {
            lock (LockObj)
            {
                if (!IsPressed) return;

                Stopwatch.Stop();
                PreviewReleased?.Invoke(this, EventArgs.Empty);
                IsPressed = false;
                Released?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
