using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.PluginHost.Handles
{
    /// <summary>
    /// プラグインからノッチの出力を編集するためのコマンドを表します。
    /// </summary>
    public abstract class NotchCommandBase
    {
        /// <summary>
        /// ノッチの出力を編集しないコマンドを表す <see cref="ContinueCommand"/> を取得します。
        /// </summary>
        public static readonly NotchCommandBase Continue = new ContinueCommand();

        /// <summary>
        /// <see cref="NotchCommandBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public NotchCommandBase()
        {
        }

        /// <summary>
        /// このコマンドによって編集したノッチの値を取得します。
        /// </summary>
        /// <param name="notch">編集元のノッチの値。</param>
        /// <returns>編集後のノッチの値。変更しないことを示すには <see langword="null"/> を返します。</returns>
        public abstract int? GetOverridenNotch(int notch);


        /// <summary>
        /// ノッチの出力を編集しないコマンドを表します。
        /// </summary>
        public sealed class ContinueCommand : NotchCommandBase
        {
            /// <summary>
            /// <see cref="ContinueCommand"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            public ContinueCommand() : base()
            {
            }

            /// <inheritdoc/>
            public override int? GetOverridenNotch(int notch) => null;
        }

        /// <summary>
        /// 指定した値にノッチの出力を変更するコマンドを表します。
        /// </summary>
        public sealed class SetNotchCommand : NotchCommandBase
        {
            private readonly int Notch;

            /// <summary>
            /// <see cref="SetNotchCommand"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            public SetNotchCommand(int notch) : base()
            {
                Notch = notch;
            }

            /// <inheritdoc/>
            public override int? GetOverridenNotch(int notch) => Notch;
        }
    }
}
