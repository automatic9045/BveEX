using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using BveEx.Diagnostics;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.PluginHost.Plugins
{
    /// <summary>
    /// 全ての BveEX プラグインの基本クラスを表します。
    /// </summary>
    /// <remarks>
    /// このクラスを直接継承する必要があるのは、特殊な BveEX プラグインの場合のみです。
    /// アセンブリ (.dll) 形式の通常の BveEX プラグインでは <see cref="AssemblyPluginBase"/> を継承してください。
    /// </remarks>
    public abstract class PluginBase : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginBase>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> BveExVersionTooOld { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> PluginTypeNotSpecified { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static PluginBase()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        private readonly PluginAttribute Info;

        /// <summary>
        /// この BveEX プラグインの種類を取得します。
        /// </summary>
        public PluginType PluginType => Info.PluginType;

        /// <summary>
        /// この BveEX プラグインが必要とする BveEX 本体の最低バージョンを取得します。最低バージョンが設定されていない場合は <see langword="null"/> を返します。
        /// </summary>
        public Version MinRequiredVersion => Info.MinRequiredVersion;

        /// <summary>
        /// 読み込まれた BveEX 拡張機能の一覧を取得します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>BveEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合)、<see cref="IExtensionSet.AllExtensionsLoaded"/> イベントが発生するまでは項目を取得できません。</item>
        /// <item>BveEX 拡張機能以外の BveEX プラグインは <see cref="Plugins"/> プロパティから取得できます。</item>
        /// </list>
        /// </remarks>
        ///////// <seealso cref="Plugins"/>
        protected IExtensionSet Extensions { get; }

        /// <summary>
        /// 読み込まれた BveEX プラグインの一覧を取得します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>BveEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合) は取得できません。</item>
        /// <item>BveEX 拡張機能は <see cref="Extensions"/> プロパティから取得できます。</item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Extensions"/>
        protected IPluginSet Plugins { get; }

        /// <summary>
        /// 本来 ATS プラグインからは利用できない BVE 本体の諸機能へアクセスするための <see cref="IBveHacker"/> を取得します。
        /// </summary>
        protected IBveHacker BveHacker { get; }

        /// <summary>
        /// PluginUsing ファイルで指定したこの BveEX プラグインの識別子を取得します。このプロパティの値は全プラグインにおいて一意であることが保証されています。
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// この BveEX プラグインのファイルの完全パスを取得します。
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// この BveEX プラグインのファイル名を取得します。
        /// </summary>
        /// <remarks>
        /// 通常はプラグイン パッケージ ファイルの名前と拡張子 (例: MyPlugin.dll) を表しますが、<br/>
        /// スクリプト プラグインの場合など、ファイル名でプラグインを判別できない場合 (例: Package.xml) は代替の文字列を使用することもできます。
        /// </remarks>
        public abstract string Name { get; }

        /// <summary>
        /// この BveEX プラグインのタイトルを取得します。
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// この BveEX プラグインのバージョンを表す文字列を取得します。
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// この BveEX プラグインの説明を取得します。
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// この BveEX プラグインの著作権表示を取得します。
        /// </summary>
        public abstract string Copyright { get; }

        private PluginBase(PluginBuilder builder, PluginAttribute info, bool allowInfoIsNull)
        {
            Extensions = builder.Extensions;
            Plugins = builder.Plugins;
            BveHacker = builder.BveHacker;
            Identifier = builder.Identifier;
            Location = builder.Location;

            if (info is null)
            {
                if (!allowInfoIsNull) throw new ArgumentNullException(nameof(info));
            }
            else
            {
                Info = info;
                Validate();
            }
        }

        /// <summary>
        /// 動的に作成された <see cref="PluginAttribute"/> 属性を参照して、BveEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="builder">BveEX から渡される BVE、BveEX の情報。</param>
        /// <param name="info">プラグインの詳細な仕様を指定する <see cref="PluginAttribute"/>。</param>
        public PluginBase(PluginBuilder builder, PluginAttribute info) : this(builder, info, false)
        {
        }

        /// <summary>
        /// クラスに付加されている <see cref="PluginAttribute"/> 属性を参照して、BveEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// <see cref="PluginAttribute"/> を付加して、プラグインの詳細な仕様を指定してください。
        /// </remarks>
        /// <param name="builder">BveEX から渡される BVE、BveEX の情報。</param>
        public PluginBase(PluginBuilder builder) : this(builder, null, true)
        {
            foreach (Attribute attribute in GetType().GetCustomAttributes())
            {
                switch (attribute)
                {
                    case PluginAttribute pluginAttribute:
                        Info = pluginAttribute;
                        break;
                }
            }

            if (Info is null) throw new InvalidOperationException(string.Format(Resources.Value.PluginTypeNotSpecified.Value, typeof(PluginAttribute).FullName));
            Validate();
        }

        private void Validate()
        {
            if (!(MinRequiredVersion is null) && App.Instance.BveExVersion < MinRequiredVersion)
            {
                string sender = Path.GetFileName(GetType().Assembly.Location);
                string message = string.Format(Resources.Value.BveExVersionTooOld.Value, App.Instance.ProductShortName, MinRequiredVersion, App.Instance.BveExVersion);
                ErrorDialogInfo errorDialogInfo = new ErrorDialogInfo(null, sender, message);

                ErrorDialog.Show(errorDialogInfo);
            }
        }

        /// <inheritdoc/>
        public abstract void Dispose();

        /// <summary>
        /// 通常の ATS プラグインの Elapse 関数と同様のタイミングで、運転中毎フレーム呼び出されます。
        /// </summary>
        /// <param name="elapsed">前フレームから経過した時間。</param>
        public abstract void Tick(TimeSpan elapsed);
    }
}
