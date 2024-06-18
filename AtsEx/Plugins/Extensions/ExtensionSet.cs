using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Plugins.Extensions
{
    internal class ExtensionSet : IExtensionSet
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ExtensionSet>(@"Core\Plugins\Extensions");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> DisplayTypeNotExtension { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> NotSubclassOfDisplayType { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TogglableInterfaceNotImplemented { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TogglableAttributeNotApplied { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static ExtensionSet()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private Dictionary<Type, ExtensionInfo> Items = null;

        public event EventHandler AllExtensionsLoaded;

        public ExtensionSet()
        {
        }

        public void SetExtensions(IEnumerable<PluginBase> extensions)
        {
            if (!(Items is null)) throw new InvalidOperationException();

            Items = extensions.Select(x =>
            {
                Type type = x.GetType();

                Type displayType = type;
                bool hide = false;
                bool canToggle = false;
                foreach (Attribute attribute in type.GetCustomAttributes())
                {
                    switch (attribute)
                    {
                        case HideExtensionMainAttribute hideAttribute:
                            hide = true;
                            break;

                        case ExtensionMainDisplayTypeAttribute displayTypeAttribute:
                            displayType = displayTypeAttribute.DisplayType;
                            break;

                        case TogglableAttribute togglableAttribute:
                            canToggle = true;
                            break;
                    }
                }

                return new ExtensionInfo(x, hide, displayType, canToggle);
            }).ToDictionary(x => x.DisplayType, x => x);
            AllExtensionsLoaded?.Invoke(this, EventArgs.Empty);
        }

        public TExtension GetExtension<TExtension>() where TExtension : IExtension
        {
            if (Items is null) throw new MemberNotInitializedException();

            ExtensionInfo result = Items[typeof(TExtension)];
            return !result.Hide && result.Body is TExtension extension ? extension : throw new KeyNotFoundException();
        }

        public IEnumerator<PluginBase> GetEnumerator() => Items is null ? throw new MemberNotInitializedException() : Items.Values.Select(x => x.Body).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        private class ExtensionInfo
        {
            public PluginBase Body { get; }
            public bool Hide { get; }
            public Type DisplayType { get; }
            public bool CanToggle { get; }

            public ExtensionInfo(PluginBase body, bool hide, Type displayType, bool canToggle)
            {
                if (!displayType.GetInterfaces().Contains(typeof(IExtension)))
                {
                    string message = string.Format(Resources.Value.DisplayTypeNotExtension.Value,
                        nameof(displayType), displayType.FullName, typeof(IExtension).FullName);
                    throw new InvalidCastException(message);
                }

                Type bodyType = body.GetType();
                if (bodyType != displayType && !bodyType.GetInterfaces().Contains(displayType) && !bodyType.IsSubclassOf(displayType))
                {
                    string message = string.Format(Resources.Value.NotSubclassOfDisplayType.Value,
                        body.GetType().FullName, nameof(displayType), displayType.FullName);
                    throw new InvalidCastException(message);
                }

                if (canToggle != displayType.GetInterfaces().Contains(typeof(ITogglableExtension)))
                {
                    Resource<string> resource = canToggle ? Resources.Value.TogglableInterfaceNotImplemented : Resources.Value.TogglableAttributeNotApplied;
                    string message = string.Format(resource.Value,
                        body.GetType().FullName, typeof(ITogglableExtension).FullName, typeof(TogglableAttribute).FullName);
                    throw new InvalidCastException(message);
                }

                Body = body;
                Hide = hide;
                DisplayType = displayType;
                CanToggle = canToggle;
            }
        }
    }
}
