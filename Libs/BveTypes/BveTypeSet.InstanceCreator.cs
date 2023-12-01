using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using TypeWrapping;

using BveTypes.ClassWrappers;

namespace BveTypes
{
    public sealed partial class BveTypeSet
    {
        /// <summary>
        /// BVE のアセンブリとバージョンを指定して、クラスラッパーに対応する BVE の型とメンバーの定義を読み込みます。
        /// </summary>
        /// <param name="bveAssembly">BVE の <see cref="Assembly"/>。</param>
        /// <param name="bveVersion">BVE のバージョン。</param>
        /// <param name="allowLoadProfileForDifferentBveVersion">実行中の BVE がサポートされないバージョンの場合、他のバージョン向けのプロファイルで代用するか。</param>
        /// <param name="profileForDifferentBveVersionLoaded">実行中の BVE がサポートされないバージョンであり、他のバージョン向けのプロファイルで代用された時に実行するデリケート。パラメータにはプロファイルのバージョンが渡されます。</param>
        /// <returns><see cref="BveTypeSet"/> クラスの新しいインスタンス。</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static BveTypeSet Load(Assembly bveAssembly, Version bveVersion, bool allowLoadProfileForDifferentBveVersion, Action<Version> profileForDifferentBveVersionLoaded = null)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                AssemblyName assemblyName = new AssemblyName(e.Name);
                if (assemblyName.Name == "SlimDX")
                {
                    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly asm in loadedAssemblies)
                    {
                        if (asm.GetName().Name == "SlimDX") return asm;
                    }
                }

                return null;
            };

            Assembly assembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> classWrapperTypes = assembly.GetExportedTypes().Where(t => t.Namespace.StartsWith(typeof(ClassWrapperBase).Namespace, StringComparison.Ordinal));
            IEnumerable<Type> bveTypes = bveAssembly.GetTypes();

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
            using (Profile profile = profileSelector.GetProfileStream(allowLoadProfileForDifferentBveVersion))
            {
                profileVersion = profile.Version;

                if (profileVersion != bveVersion)
                {
                    profileForDifferentBveVersionLoaded?.Invoke(profileVersion);
                }

                using (Stream schema = SchemaProvider.GetSchemaStream())
                {
                    types = WrapTypeSet.LoadXml(profile.Stream, schema, classWrapperTypes, bveTypes, additionalWrapTypes);
                }
            }

            BveTypeSet result = new BveTypeSet(types.Types, types.Bridge, profileVersion);

            ClassWrapperInitializer classWrapperInitializer = new ClassWrapperInitializer(result);
            classWrapperInitializer.InitializeAll();

            return result;
        }
    }
}