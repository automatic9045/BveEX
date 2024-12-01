using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;

using BveTypes.ClassWrappers;

namespace BveTypes
{
    internal class WrapperEvent<THandler> where THandler : Delegate
    {
        private readonly Dictionary<THandler, Delegate> Delegates = new Dictionary<THandler, Delegate>();
        private readonly FastEvent Source;
        private readonly Converter<THandler, EventHandler> HandlerConverter;

        public WrapperEvent(FastEvent source, Converter<THandler, EventHandler> handlerConverter)
        {
            Source = source;
            HandlerConverter = handlerConverter;
        }

        public void Add(object instance, THandler value)
        {
            EventHandler originalHandler = HandlerConverter(value);
            Delegate originalDelegate = originalHandler.Method.CreateDelegate(Source.DelegateField.Source.FieldType, originalHandler.Target);
            Source.Add(instance, originalDelegate);
            Delegates.Add(value, originalDelegate);
        }

        public void Remove(object instance, THandler value)
        {
            Source.Remove(instance, Delegates[value]);
            Delegates.Remove(value);
        }

        public void Invoke(object instance, ClassWrapperBase eventArgs)
        {
            Source.Invoke(instance, new object[] { instance, eventArgs?.Src });
        }
    }
}
