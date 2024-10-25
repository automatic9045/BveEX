using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.PluginHost.Native
{
    /// <summary>
    /// <see cref="INative.Started"/> イベントのデータを表します。
    /// </summary>
    public class StartedEventArgs : EventArgs
    {
        /// <summary>
        /// ブレーキハンドルの初期位置を取得します。
        /// </summary>
        public BrakePosition DefaultBrakePosition { get; }

        /// <summary>
        /// <see cref="StartedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="defaultBrakePosition">ブレーキハンドルの初期位置。</param>
        public StartedEventArgs(BrakePosition defaultBrakePosition) : base()
        {
            DefaultBrakePosition = defaultBrakePosition;
        }
    }
}
