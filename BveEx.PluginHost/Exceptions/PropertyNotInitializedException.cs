using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

namespace AtsEx.PluginHost
{
    /// <summary>
    /// まだ初期化されておらず取得できないプロパティを取得しようとしたときにスローされる例外です。
    /// </summary>
    public class PropertyNotInitializedException : MemberNotInitializedException
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PropertyNotInitializedException>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Message { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static PropertyNotInitializedException()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        /// <summary>
        /// <see cref="PropertyNotInitializedException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="propertyName">この例外の対象となるプロパティの名前。</param>
        public PropertyNotInitializedException(string propertyName) : base(string.Format(Resources.Value.Message.Value, propertyName))
        {
        }
    }
}
