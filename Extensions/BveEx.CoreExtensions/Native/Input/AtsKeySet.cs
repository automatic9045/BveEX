using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Input;

namespace BveEx.Extensions.Native.Input
{
    /// <summary>
    /// ATS キーのセットを表します。
    /// </summary>
    public class AtsKeySet
    {
        private readonly IReadOnlyDictionary<int, AtsKey> Items;

        /// <summary>
        /// いずれかのキーが押された瞬間に発生します。
        /// </summary>
        public event EventHandler<AtsKeyEventArgs> AnyKeyPressed;

        /// <summary>
        /// いずれかのキーが離された瞬間に発生します。
        /// </summary>
        public event EventHandler<AtsKeyEventArgs> AnyKeyReleased;

        internal AtsKeySet()
        {
            AtsKeyName[] allKeyNames = Enum.GetValues(typeof(AtsKeyName)) as AtsKeyName[];
            Items = allKeyNames.ToDictionary(keyName => (int)keyName, _ => new AtsKey());
        }

        /// <summary>
        /// 指定した種類のキーを取得します。
        /// </summary>
        /// <param name="keyName">ATS キーの種類。</param>
        /// <returns>種類 <paramref name="keyName"/> の ATS キー。</returns>
        public AtsKey GetKey(AtsKeyName keyName)
        {
            return Items[(int)keyName];
        }

        internal void NotifyPressed(AtsKeyName keyName)
        {
            AtsKey key = Items[(int)keyName];
            key.NotifyPressed();
            AnyKeyPressed?.Invoke(this, new AtsKeyEventArgs(keyName, key));
        }

        internal void NotifyReleased(AtsKeyName keyName)
        {
            AtsKey key = Items[(int)keyName];
            key.NotifyReleased();
            AnyKeyReleased?.Invoke(this, new AtsKeyEventArgs(keyName, key));
        }
    }
}
