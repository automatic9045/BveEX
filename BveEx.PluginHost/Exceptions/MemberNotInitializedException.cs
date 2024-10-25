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
    public class MemberNotInitializedException : Exception
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<MemberNotInitializedException>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> Message { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static MemberNotInitializedException()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        /// <summary>
        /// メッセージを指定して、<see cref="MemberNotInitializedException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">この例外の詳細を説明するメッセージ。</param>
        public MemberNotInitializedException(string message) : base(message)
        {
        }

        /// <summary>
        /// <see cref="MemberNotInitializedException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MemberNotInitializedException() : this(Resources.Value.Message.Value)
        {
        }
    }
}
