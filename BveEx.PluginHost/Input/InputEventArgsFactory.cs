using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using Mackoy.Bvets;

using BveEx.PluginHost.Input.Native;
using BveEx.PluginHost.Native;

namespace BveEx.PluginHost.Input
{
    /// <summary>
    /// <see cref="InputEventArgs"/> を直感的に作成するための機能を提供します。
    /// </summary>
    public static class InputEventArgsFactory
    {
        /// <summary>
        /// ワンハンドル式運転台のノッチを変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。マスコンの場合は正、ブレーキの場合は負の数で表します。</param>
        /// <returns>ノッチを変更することを表す <see cref="InputEventArgs"/>。</returns>
        /// <seealso cref="OneLeverPower(int)"/>
        /// <seealso cref="OneLeverBrake(int)"/>
        public static InputEventArgs OneLeverNotch(int notch)
        {
            return new InputEventArgs(3, notch);
        }

        /// <summary>
        /// ワンハンドル式運転台のマスコンノッチを変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。</param>
        /// <returns>ノッチを変更することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs OneLeverPower(int notch)
        {
            if (notch < 0) throw new ArgumentOutOfRangeException(nameof(notch));

            InputEventArgs result = OneLeverNotch(notch);
            return result;
        }

        /// <summary>
        /// ワンハンドル式運転台のブレーキノッチを変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。</param>
        /// <returns>ノッチを変更することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs OneLeverBrake(int notch)
        {
            if (notch < 0) throw new ArgumentOutOfRangeException(nameof(notch));

            InputEventArgs result = OneLeverNotch(-notch);
            return result;
        }

        /// <summary>
        /// ワンハンドル式運転台のノッチを N 位置へ変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <returns>ノッチを変更することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs OneLeverNeutral()
        {
            InputEventArgs result = OneLeverNotch(0);
            return result;
        }

        /// <summary>
        /// ツーハンドル式運転台のマスコンノッチを変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。抑速ブレーキの場合は負の数で表します。</param>
        /// <returns>ノッチを変更することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs TwoLeverPower(int notch)
        {
            InputEventArgs result = new InputEventArgs(1, notch);
            return result;
        }

        /// <summary>
        /// ツーハンドル式運転台のブレーキノッチを変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="notch">変更先のノッチ。</param>
        /// <returns>ノッチを変更することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs TwoLeverBrake(int notch)
        {
            if (notch < 0) throw new ArgumentOutOfRangeException(nameof(notch));

            InputEventArgs result = new InputEventArgs(2, notch);
            return result;
        }

        /// <summary>
        /// レバーサー位置を変更するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="position">変更先のレバーサー位置。</param>
        /// <returns>レバーサー位置を変更することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs Reverser(ReverserPosition position)
        {
            InputEventArgs result = new InputEventArgs(0, (int)position);
            return result;
        }

        /// <summary>
        /// 警笛を鳴らすための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="type">鳴らす警笛の種類。</param>
        /// <returns>警笛を鳴らすことを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs Horn(HornType type)
        {
            switch (type)
            {
                case HornType.Primary:
                    return new InputEventArgs(-1, 0);

                case HornType.Secondary:
                    return new InputEventArgs(-1, 1);

                case HornType.Music:
                    return new InputEventArgs(-1, 3);

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 定速制御を有効化するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <returns>定速制御を有効化することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs ConstantSpeed()
        {
            return new InputEventArgs(-1, 2);
        }

        /// <summary>
        /// ATS キーを押下するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="key">押下するキー。</param>
        /// <returns>ATS キーを押下することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs AtsKey(NativeAtsKeyName key)
        {
            return new InputEventArgs(-2, (int)key);
        }

        /// <summary>
        /// システムキーを押下するための <see cref="InputEventArgs"/> を作成します。
        /// </summary>
        /// <param name="key">押下するキー。</param>
        /// <returns>システムキーを押下することを表す <see cref="InputEventArgs"/>。</returns>
        public static InputEventArgs SystemKey(SystemKeyName key)
        {
            return new InputEventArgs(-3, (int)key);
        }
    }
}
