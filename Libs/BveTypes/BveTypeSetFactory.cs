using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;
using TypeWrapping;

namespace BveTypes
{
    /// <summary>
    /// <see cref="BveTypeSet"/> を読み込むための機能を提供します。
    /// </summary>
    public class BveTypeSetFactory
    {
        /// <summary>
        /// BVE 本体のものである <see cref="Assembly"/> を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定値は <see cref="Assembly.GetEntryAssembly"/> です。
        /// </remarks>
        public Assembly BveAssembly { get; set; } = Assembly.GetEntryAssembly();

        /// <summary>
        /// 実行中の BVE がサポートされないバージョンの場合、他のバージョン向けのプロファイルで代用するかどうかを取得・設定します。
        /// </summary>
        /// <remarks>
        /// 既定値は <see langword="false"/> です。
        /// </remarks>
        public bool AllowProfileForDifferentBveVersion { get; set; } = false;

        /// <summary>
        /// 実行中の BVE がサポートされないバージョンであり、他のバージョン向けのプロファイルで代用された時に呼び出されます。
        /// </summary>
        /// <remarks>
        /// パラメータにはプロファイルのバージョンが渡されます。
        /// </remarks>
        public event EventHandler<DifferentVersionProfileLoadedEventArgs> DifferentVersionProfileLoaded;

        /// <summary>
        /// <see cref="BveTypeSetFactory"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public BveTypeSetFactory()
        {
        }

        /// <summary>
        /// この <see cref="BveTypeSetFactory"/> に設定された内容をもとに、クラスラッパーに対応する BVE の型とメンバーの定義を読み込みます。
        /// </summary>
        /// <returns>読み込まれた <see cref="BveTypeSet"/>。</returns>
        public BveTypeSet Load()
        {
            try
            {
                return Load();
            }
            catch (Exception ex)
            {
                Exception first = GetTypicalException(ex);

                IGrouping<string, Assembly> duplicatedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(asm => !asm.IsDynamic)
                    .GroupBy(asm => Path.GetFileName(asm.Location))
                    .FirstOrDefault(g => g.Skip(1).Any());

                if (duplicatedAssemblies is null)
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    throw;
                }
                else
                {
                    throw new DuplicatedLibraryException(duplicatedAssemblies.Key, duplicatedAssemblies, first);
                }
            }


            BveTypeSet Load()
            {
                Version bveVersion = BveAssembly.GetName().Version;
                Assembly assembly = Assembly.GetExecutingAssembly();

                IEnumerable<Type> classWrapperTypes = assembly.GetExportedTypes().Where(t => t.Namespace.StartsWith(typeof(ClassWrapperBase).Namespace, StringComparison.Ordinal));
                IEnumerable<Type> bveTypes = BveAssembly.GetTypes();

                Dictionary<Type, Type> additionalWrapTypes = new Dictionary<Type, Type>();
                foreach (Type type in classWrapperTypes)
                {
                    AdditionalTypeWrapperAttribute attribute = type.GetCustomAttribute<AdditionalTypeWrapperAttribute>(false);
                    if (attribute is null) continue;

                    additionalWrapTypes.Add(type, attribute.Original);
                }

                ProfileSelector profileSelector = new ProfileSelector(bveVersion);
                Version profileVersion;
                WrapTypeSet types;
                using (Profile profile = profileSelector.GetProfileStream(AllowProfileForDifferentBveVersion))
                {
                    profileVersion = profile.Version;

                    if (profileVersion != bveVersion)
                    {
                        DifferentVersionProfileLoadedEventArgs args = new DifferentVersionProfileLoadedEventArgs(bveVersion, profileVersion);
                        DifferentVersionProfileLoaded?.Invoke(this, args);
                    }

                    using (Stream schema = SchemaProvider.GetSchemaStream())
                    {
                        types = AssemblyResolver.WithResolve(()
                            => WrapTypeSet.LoadXml(profile.Stream, schema, classWrapperTypes, bveTypes, additionalWrapTypes),
                            "System.Drawing", "System.Windows.Forms", "Irony", "SlimDX");
                    }
                }

                BveTypeSet result = new BveTypeSet(types, profileVersion);

                ClassWrapperInitializer classWrapperInitializer = new ClassWrapperInitializer(result);
                classWrapperInitializer.InitializeAll();

                return result;
            }

            Exception GetTypicalException(Exception exception)
            {
                switch (exception)
                {
                    case AggregateException aggregateException:
                        return GetTypicalException(aggregateException.InnerExceptions[0]);

                    default:
                        return exception;
                }
            }
        }
    }
}
