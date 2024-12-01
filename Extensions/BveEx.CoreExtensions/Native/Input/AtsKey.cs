using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.Native.Input
{
    /// <summary>
    /// すべてのキーの基本クラスを表します。
    /// このクラスはスレッド セーフです。
    /// </summary>
    public class AtsKey
    {
        private readonly Stopwatch Stopwatch = new Stopwatch();
        private readonly object LockObj = new object();

        /// <summary>
        /// キーが押されているかを取得します。
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// キーが押されてから経過した時間 [ms] を取得します。
        /// キーが押されていない場合は 0 を返します。
        /// </summary>
        public long SincePressedMilliseconds => IsPressed ? Stopwatch.ElapsedMilliseconds : 0;

        /// <summary>
        /// キーが押されてから経過した時間を取得します。
        /// キーが押されていない場合は <see cref="TimeSpan.Zero"/> を返します。
        /// </summary>
        public TimeSpan SincePressed => IsPressed ? Stopwatch.Elapsed : TimeSpan.Zero;

        /// <summary>
        /// キーが押された瞬間に発生します。
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// キーが離され、<see cref="IsPressed"/> に <see langword="false"/> が設定される直前に発生します。
        /// </summary>
        /// <remarks>
        /// このイベントが発生した時点で経過時間を取得するための <see cref="System.Diagnostics.Stopwatch"/> は停止しているため、<br/>
        /// <see cref="SincePressedMilliseconds"/> プロパティ、<see cref="SincePressed"/> プロパティから取得できる値は常に不変です。
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
        /// <see cref="AtsKey"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public AtsKey()
        {
        }

        /// <summary>
        /// この <see cref="AtsKey"/> オブジェクトに対するロックを取得し、指定したデリゲートを実行します。
        /// </summary>
        /// <param name="action">実行するデリゲート。</param>
        public void LockAndInvoke(Action action)
        {
            lock (LockObj) action();
        }

        internal void NotifyPressed()
        {
            lock (LockObj)
            {
                if (IsPressed) return;

                IsPressed = true;
                Stopwatch.Restart();
                Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void NotifyReleased()
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
