﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using BveEx.Plugins.Native;
using BveEx.Plugins.Scripting;
using BveEx.Plugins.Scripting.CSharp;
using BveEx.Plugins.Scripting.IronPython2;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Plugins
{
    internal partial class PluginLoader
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginLoader>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginClassNotFound { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginVersionNotSupported { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> ConstructorNotFound { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> CannotSetIdentifier { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> WrongPluginType { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> MaybeBecauseBuiltForDifferentVersion { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();
        private static readonly Version SupportedMinPluginHostVersion = new Version(0, 17);

        static PluginLoader()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        protected readonly BveHacker BveHacker;
        protected readonly IExtensionSet Extensions;
        protected readonly IPluginSet Plugins;

        public PluginLoader(BveHacker bveHacker, IExtensionSet extensions, IPluginSet plugins)
        {
            BveHacker = bveHacker;
            Extensions = extensions;
            Plugins = plugins;
        }

        public Dictionary<string, PluginBase> Load(PluginSourceSet pluginSources)
        {
            Dictionary<string, PluginBase> plugins = new Dictionary<string, PluginBase>();
            PluginLoadErrorQueue loadErrorQueue = new PluginLoadErrorQueue(BveHacker.LoadingProgressForm);

            foreach (IPluginPackage pluginPackage in pluginSources)
            {
                switch (pluginPackage)
                {
                    case AssemblyPluginPackage assemblyPluginPackage:
                    {
                        try
                        {
                            List<PluginBase> loadedPlugins = LoadFromAssembly(assemblyPluginPackage, pluginSources.PluginType, pluginSources.AllowNonPluginAssembly);
                            loadedPlugins.ForEach(plugin => plugins[plugin.Identifier] = plugin);
                        }
                        catch (Exception ex)
                        {
                            loadErrorQueue.OnFailedToLoadAssembly(assemblyPluginPackage.Assembly, ex);
                        }

                        break;
                    }

                    case ScriptPluginPackage scriptPluginPackage:
                    {
                        try
                        {
                            PluginBuilder pluginBuilder = new PluginBuilder(BveHacker, Extensions, Plugins, scriptPluginPackage.Identifier.Text, scriptPluginPackage.Location);

                            ScriptPluginBase plugin;
                            switch (scriptPluginPackage.ScriptLanguage)
                            {
                                case ScriptLanguage.CSharpScript:
                                    plugin = CSharpScriptPlugin.FromPackage(pluginBuilder, pluginSources.PluginType, scriptPluginPackage);
                                    break;

                                case ScriptLanguage.IronPython2:
                                    plugin = IronPython2Plugin.FromPackage(pluginBuilder, pluginSources.PluginType, scriptPluginPackage);
                                    break;

                                default:
                                    throw new NotImplementedException();
                            }

                            plugins[scriptPluginPackage.Identifier.Text] = plugin;
                        }
                        catch (Exception ex)
                        {
                            loadErrorQueue.OnFailedToLoadScriptPlugin(scriptPluginPackage, ex);
                        }

                        break;
                    }

                    case NativePluginPackage nativePluginPackage:
                    {
                        try
                        {
                            PluginBuilder pluginBuilder = new PluginBuilder(BveHacker, Extensions, Plugins, nativePluginPackage.Identifier.Text, nativePluginPackage.LibraryPath);
                            NativePlugin plugin = new NativePlugin(pluginBuilder);
                            plugins[nativePluginPackage.Identifier.Text] = plugin;
                        }
                        catch (Exception ex)
                        {
                            loadErrorQueue.OnFailedToLoadNativePlugin(nativePluginPackage.LibraryPath, ex);
                        }

                        break;
                    }

                    // TODO: ここで他の種類のプラグインを読み込む

                    default:
                        throw new NotImplementedException();
                }
            }

            loadErrorQueue.Resolve();
            return plugins;


            List<PluginBase> LoadFromAssembly(AssemblyPluginPackage package, PluginType pluginType, bool allowNonPluginAssembly)
            {
                string fileName = Path.GetFileName(package.Path);

                Version pluginHostVersion = App.Instance.BveExPluginHostAssembly.GetName().Version;
                AssemblyName referencedPluginHost = package.Assembly.GetReferencedPluginHost();
                if (referencedPluginHost is null)
                {
                    return allowNonPluginAssembly
                        ? new List<PluginBase>()
                        : throw new BveFileLoadException(string.Format(Resources.Value.PluginClassNotFound.Value, nameof(PluginBase), App.Instance.ProductShortName), fileName);
                }
                else if (referencedPluginHost.Version < SupportedMinPluginHostVersion)
                {
                    throw new BveFileLoadException(string.Format(
                        Resources.Value.PluginVersionNotSupported.Value,
                        referencedPluginHost.Version, App.Instance.ProductShortName, pluginHostVersion, SupportedMinPluginHostVersion.ToString(2)), fileName);
                }

                Type[] allTypes = package.Assembly.GetTypes();
                IEnumerable<Type> pluginTypes = allTypes.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(PluginBase)));
                if (!pluginTypes.Any())
                {
                    throw new BveFileLoadException(string.Format(Resources.Value.PluginClassNotFound.Value, nameof(PluginBase), App.Instance.ProductShortName), fileName);
                }

                List<(Type, ConstructorInfo)> constructors = new List<(Type, ConstructorInfo)>();
                foreach (Type type in pluginTypes)
                {
                    ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(PluginBuilder) });
                    if (constructor is null) continue;

                    constructors.Add((type, constructor));
                }

                switch (constructors.Count)
                {
                    case 0:
                        throw new BveFileLoadException(string.Format(Resources.Value.ConstructorNotFound.Value, nameof(PluginBase), typeof(PluginBuilder)), fileName);

                    case 1:
                        break;

                    default:
                        if (!(package.Identifier is RandomIdentifier))
                        {
                            throw new BveFileLoadException(string.Format(Resources.Value.CannotSetIdentifier.Value, fileName), pluginSources.Name);
                        }
                        break;
                }

                List<PluginBase> constructedPlugins = constructors.ConvertAll(constructor =>
                {
                    (Type type, ConstructorInfo constructorInfo) = constructor;

                    PluginBase pluginInstance = constructorInfo.Invoke(new object[] { new PluginBuilder(BveHacker, Extensions, Plugins, GenerateIdentifier(), package.Path) }) as PluginBase;
                    if (pluginInstance.PluginType != pluginType)
                    {
                        throw new InvalidOperationException(string.Format(Resources.Value.WrongPluginType.Value, pluginType.GetTypeString(), pluginInstance.PluginType.GetTypeString()));
                    }

                    return pluginInstance;
                });

                return constructedPlugins;


                string GenerateIdentifier() => constructors.Count == 1 ? package.Identifier.Text : Guid.NewGuid().ToString();
            }
        }
    }
}
