using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UnembeddedResources;

using AtsEx.Diagnostics;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.PluginHost.Plugins
{
    /// <summary>
    /// 全ての AtsEX プラグインの基本クラスを表します。
    /// </summary>
    /// <remarks>
    /// このクラスを直接継承する必要があるのは、特殊な AtsEX プラグインの場合のみです。
    /// アセンブリ (.dll) 形式の通常の AtsEX プラグインでは <see cref="AssemblyPluginBase"/> を継承してください。
    /// </remarks>
    public abstract class PluginBase : IDisposable
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<PluginBase>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> AtsExVersionTooOld { get; private set; }
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
        /// この AtsEX プラグインの種類を取得します。
        /// </summary>
        public PluginType PluginType => Info.PluginType;

        /// <summary>
        /// この AtsEX プラグインが必要とする AtsEX 本体の最低バージョンを取得します。最低バージョンが設定されていない場合は <see langword="null"/> を返します。
        /// </summary>
        public Version MinRequiredVersion => Info.MinRequiredVersion;

        /// <summary>
        /// 使用できません。常に <see langword="true"/> を返します。
        /// </summary>
        [Obsolete]
        public bool UseBveHacker { get; } = true;

        /// <summary>
        /// BVE が標準で提供する ATS プラグイン向けの機能のラッパーを取得します。
        /// </summary>
        /// <remarks>
        /// AtsEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合) は取得できません。
        /// </remarks>
        protected INative Native { get; }

        /// <summary>
        /// 読み込まれた AtsEX 拡張機能の一覧を取得します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>AtsEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合)、<see cref="IExtensionSet.AllExtensionsLoaded"/> イベントが発生するまでは項目を取得できません。</item>
        /// <item>AtsEX 拡張機能以外の AtsEX プラグインは <see cref="Plugins"/> プロパティから取得できます。</item>
        /// </list>
        /// </remarks>
        ///////// <seealso cref="Plugins"/>
        protected IExtensionSet Extensions { get; }

        /// <summary>
        /// 読み込まれた AtsEX プラグインの一覧を取得します。
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>AtsEX 拡張機能の場合 (<see cref="PluginType"/> が <see cref="PluginType.Extension"/> の場合) は取得できません。</item>
        /// <item>AtsEX 拡張機能は <see cref="Extensions"/> プロパティから取得できます。</item>
        /// </list>
        /// </remarks>
        /// <seealso cref="Extensions"/>
        protected IPluginSet Plugins { get; }

        /// <summary>
        /// 本来 ATS プラグインからは利用できない BVE 本体の諸機能へアクセスするための <see cref="IBveHacker"/> を取得します。
        /// </summary>
        protected IBveHacker BveHacker { get; }

        /// <summary>
        /// PluginUsing ファイルで指定したこの  AtsEX プラグインの識別子を取得します。このプロパティの値は全プラグインにおいて一意であることが保証されています。
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// この AtsEX プラグインのファイルの完全パスを取得します。
        /// </summary>
        public abstract string Location { get; }

        /// <summary>
        /// この AtsEX プラグインのファイル名を取得します。
        /// </summary>
        /// <remarks>
        /// 通常はプラグイン パッケージ ファイルの名前と拡張子 (例: MyPlugin.dll) を表しますが、<br/>
        /// スクリプト プラグインの場合など、ファイル名でプラグインを判別できない場合 (例: Package.xml) は代替の文字列を使用することもできます。
        /// </remarks>
        public abstract string Name { get; }

        /// <summary>
        /// この AtsEX プラグインのタイトルを取得します。
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// この AtsEX プラグインのバージョンを表す文字列を取得します。
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// この AtsEX プラグインの説明を取得します。
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// この AtsEX プラグインの著作権表示を取得します。
        /// </summary>
        public abstract string Copyright { get; }

        private PluginBase(PluginBuilder builder, PluginAttribute info, bool allowInfoIsNull)
        {
            Native = builder.Native;
            Extensions = builder.Extensions;
            Plugins = builder.Plugins;
            BveHacker = builder.BveHacker;
            Identifier = builder.Identifier;

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
        /// 動的に作成された <see cref="PluginAttribute"/> 属性を参照して、AtsEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="builder">AtsEX から渡される BVE、AtsEX の情報。</param>
        /// <param name="info">プラグインの詳細な仕様を指定する <see cref="PluginAttribute"/>。</param>
        public PluginBase(PluginBuilder builder, PluginAttribute info) : this(builder, info, false)
        {
        }

        /// <summary>
        /// AtsEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// 互換性のために残されているコンストラクタです。パラメータに <see cref="PluginAttribute"/> を指定するオーバーロードを使用してください。
        /// </remarks>
        /// <param name="builder">AtsEX から渡される BVE、AtsEX の情報。</param>
        /// <param name="pluginType">AtsEX プラグインの種類。</param>
        /// <param name="useBveHacker">使用されません。</param>
        [Obsolete]
        public PluginBase(PluginBuilder builder, PluginType pluginType, bool useBveHacker) : this(builder, new PluginAttribute(pluginType))
        {
        }

        /// <summary>
        /// クラスに付加されている <see cref="PluginAttribute"/> 属性を参照して、AtsEX プラグインの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// <see cref="PluginAttribute"/> を付加して、プラグインの詳細な仕様を指定してください。
        /// </remarks>
        /// <param name="builder">AtsEX から渡される BVE、AtsEX の情報。</param>
        public PluginBase(PluginBuilder builder) : this(builder, null, true)
        {
            foreach (Attribute attribute in GetType().GetCustomAttributes())
            {
                switch (attribute)
                {
                    case PluginAttribute pluginAttribute:
                        Info = pluginAttribute;
                        break;

#pragma warning disable CS0612 // 型またはメンバーが旧型式です
                    case PluginTypeAttribute pluginTypeAttribute:
                        Info = PluginAttribute.FromPluginTypeAttribute(pluginTypeAttribute);
                        break;
#pragma warning restore CS0612 // 型またはメンバーが旧型式です
                }
            }

            if (Info is null) throw new InvalidOperationException(string.Format(Resources.Value.PluginTypeNotSpecified.Value, typeof(PluginAttribute).FullName));
            Validate();
        }

        private void Validate()
        {
            if (!(MinRequiredVersion is null) && App.Instance.AtsExVersion < MinRequiredVersion)
            {
                string sender = Path.GetFileName(GetType().Assembly.Location);
                string message = string.Format(Resources.Value.AtsExVersionTooOld.Value, App.Instance.ProductShortName, MinRequiredVersion, App.Instance.AtsExVersion);
                ErrorDialogInfo errorDialogInfo = new ErrorDialogInfo(null, sender, message);

                ErrorDialog.Show(errorDialogInfo);
            }
        }

        /// <inheritdoc/>
        public abstract void Dispose();

        /// <summary>
        /// 毎フレーム呼び出されます。ネイティブ ATS プラグインの Elapse(ATS_VEHICLESTATE vehicleState, int[] panel, int[] sound) に当たります。
        /// </summary>
        /// <param name="elapsed">前フレームから経過した時間。</param>
        /// <returns>
        /// このメソッドの実行結果を表す <see cref="TickResult"/>。<br/>
        /// 拡張機能では <see cref="ExtensionTickResult"/> を、車両プラグインでは <see cref="VehiclePluginTickResult"/> を、マッププラグインでは <see cref="MapPluginTickResult"/> を返してください。
        /// </returns>
        public abstract TickResult Tick(TimeSpan elapsed);
    }
}
