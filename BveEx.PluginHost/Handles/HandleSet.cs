using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Handles
{
    /// <summary>
    /// 全てのハンドルのセットを表します。
    /// </summary>
    public class HandleSet
    {
        /// <summary>
        /// ブレーキハンドルを表す <see cref="IBrakeHandle"/> を取得します。
        /// </summary>
        public IBrakeHandle Brake { get; }

        /// <summary>
        /// 力行ハンドルを表す <see cref="IPowerHandle"/> を取得します。
        /// </summary>
        public IPowerHandle Power { get; }

        /// <summary>
        /// 逆転器を表す <see cref="IReverser"/> を取得します。
        /// </summary>
        public IReverser Reverser { get; }

        /// <summary>
        /// <see cref="HandleSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="brake">ブレーキハンドルを表す <see cref="IBrakeHandle"/>。</param>
        /// <param name="power">力行ハンドルを表す <see cref="IPowerHandle"/>。</param>
        /// <param name="reverser">逆転器を表す <see cref="IReverser"/>。</param>
        public HandleSet(IBrakeHandle brake, IPowerHandle power, IReverser reverser)
        {
            Brake = brake;
            Power = power;
            Reverser = reverser;
        }
    }
}
