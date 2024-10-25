using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// アセンブリ (.dll) 形式の BveEX プラグインを表します。
    /// </summary>
    public abstract class AssemblyPluginBase : PluginBase
    {
        /// <inheritdoc/>
        public override string Location { get; }

        /// <inheritdoc/>
        public override string Name { get; }

        /// <inheritdoc/>
        public override string Title { get; }

        /// <inheritdoc/>
        public override string Version { get; }

        /// <inheritdoc/>
        public override string Description { get; }

        /// <inheritdoc/>
        public override string Copyright { get; }

        /// <summary>
        /// BveEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// <see cref="PluginAttribute"/> を付加して、プラグインの種類を指定してください。
        /// </remarks>
        /// <param name="builder">BveEX から渡される BVE、BveEX の情報。</param>
        public AssemblyPluginBase(PluginBuilder builder) : base(builder)
        {
            Assembly pluginAssembly = GetType().Assembly;

            Location = pluginAssembly.Location;
            Name = Path.GetFileName(Location);
            Title = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(pluginAssembly, typeof(AssemblyTitleAttribute))).Title;
            Version = pluginAssembly.GetName().Version.ToString();
            Description = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(pluginAssembly, typeof(AssemblyDescriptionAttribute))).Description;
            Copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(pluginAssembly, typeof(AssemblyCopyrightAttribute))).Copyright;
        }
    }
}
