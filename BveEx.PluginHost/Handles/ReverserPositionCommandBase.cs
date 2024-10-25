using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace BveEx.PluginHost.Handles
{
    /// <summary>
    /// プラグインから逆転器の位置の出力を編集するためのコマンドを表します。
    /// </summary>
    public abstract class ReverserPositionCommandBase
    {
        /// <summary>
        /// 逆転器の位置の出力を編集しないコマンドを表す <see cref="ContinueCommand"/> を取得します。
        /// </summary>
        public static readonly ReverserPositionCommandBase Continue = new ContinueCommand();

        /// <summary>
        /// <see cref="ReverserPositionCommandBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ReverserPositionCommandBase()
        {
        }

        /// <summary>
        /// このコマンドによって編集した逆転器の位置の値を取得します。
        /// </summary>
        /// <param name="position">編集元の逆転器の位置の値。</param>
        /// <returns>編集後の逆転器の位置の値。変更しないことを示すには <see langword="null"/> を返します。</returns>
        public abstract ReverserPosition? GetOverridenPosition(ReverserPosition position);


        /// <summary>
        /// 逆転器の位置の出力を編集しないコマンドを表します。
        /// </summary>
        public sealed class ContinueCommand : ReverserPositionCommandBase
        {
            /// <summary>
            /// <see cref="ContinueCommand"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            public ContinueCommand() : base()
            {
            }

            /// <inheritdoc/>
            public override ReverserPosition? GetOverridenPosition(ReverserPosition position) => null;
        }

        /// <summary>
        /// 指定した値に逆転器の位置の出力を変更するコマンドを表します。
        /// </summary>
        public sealed class SetPositionCommand : ReverserPositionCommandBase
        {
            private readonly ReverserPosition Position;

            /// <summary>
            /// <see cref="SetPositionCommand"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            public SetPositionCommand(ReverserPosition position) : base()
            {
                Position = position;
            }

            /// <inheritdoc/>
            public override ReverserPosition? GetOverridenPosition(ReverserPosition position) => Position;
        }
    }
}
