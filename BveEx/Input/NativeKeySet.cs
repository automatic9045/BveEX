using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.PluginHost.Input;
using BveEx.PluginHost.Input.Native;

namespace BveEx.Input
{
    internal class NativeKeySet : INativeKeySet
    {
        public ReadOnlyDictionary<NativeAtsKeyName, KeyBase> AtsKeys { get; }

        public event EventHandler<NativeKeyEventArgs> AnyKeyPressed;
        public event EventHandler<NativeKeyEventArgs> AnyKeyReleased;

        public NativeKeySet()
        {
            {
                NativeAtsKeyName[] allKeyNames = Enum.GetValues(typeof(NativeAtsKeyName)) as NativeAtsKeyName[];
                Dictionary<NativeAtsKeyName, KeyBase> keyDictionary = allKeyNames.ToDictionary(keyName => keyName, _ => new NativeAtsKey() as KeyBase);
                Dictionary<NativeAtsKeyName, KeyBase> sortedKeyList = new Dictionary<NativeAtsKeyName, KeyBase>(keyDictionary);

                AtsKeys = new ReadOnlyDictionary<NativeAtsKeyName, KeyBase>(sortedKeyList);
            }
        }

        public void NotifyPressed(NativeAtsKeyName keyName)
        {
            NativeAtsKey key = (NativeAtsKey)AtsKeys[keyName];
            key.NotifyPressed();
            AnyKeyPressed?.Invoke(this, new NativeKeyEventArgs(keyName, key));
        }

        public void NotifyReleased(NativeAtsKeyName keyName)
        {
            NativeAtsKey key = (NativeAtsKey)AtsKeys[keyName];
            key.NotifyReleased();
            AnyKeyReleased?.Invoke(this, new NativeKeyEventArgs(keyName, key));
        }
    }
}
