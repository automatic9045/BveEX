using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Handles;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// 車両プラグインの <see cref="PluginBase.Tick(TimeSpan)"/> メソッドの実行結果を表します。
    /// </summary>
    public class VehiclePluginTickResult : IPluginTickResult
    {
        private HandleCommandSet _HandleCommandSet = HandleCommandSet.DoNothing;
        /// <summary>
        /// ハンドルの出力を編集するためのコマンドのセットを取得・設定します。
        /// </summary>
        /// <remarks>何もしないことを表すには <see cref="HandleCommandSet.DoNothing"/> を指定してください。</remarks>
        public HandleCommandSet HandleCommandSet
        {
            get => _HandleCommandSet;
            set => _HandleCommandSet = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// <see cref="VehiclePluginTickResult"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public VehiclePluginTickResult()
        {
        }
    }
}
