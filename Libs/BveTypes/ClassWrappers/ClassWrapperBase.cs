using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using UnembeddedResources;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// すべてのクラスラッパーの基本クラスを表します。
    /// </summary>
    public abstract class ClassWrapperBase
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ClassWrapperBase>("PluginHost");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> TypeNotClassWrapper { get; private set; }
            [ResourceStringHolder(nameof(Localizer))] public Resource<string> FromSourceMethodNotFound { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();
        private static BveTypeSet BveTypes;

        static ClassWrapperBase()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            BveTypes = bveTypes;
        }

        /// <summary>
        /// オリジナル オブジェクトから指定したラッパー型のインスタンスを生成します。
        /// </summary>
        /// <param name="wrapperType">生成するラッパーの型。</param>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <paramref name="wrapperType"/> で指定したラッパー型のインスタンス。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="wrapperType"/> が <see langword="null"/> です。</exception>
        /// <exception cref="ArgumentException"><paramref name="wrapperType"/> で指定した型はクラスラッパーではありません。<see cref="ClassWrapperBase"/> を継承していません。</exception>
        /// <exception cref="InvalidOperationException"><paramref name="wrapperType"/> で指定した型内に有効なクラスラッパー生成メソッドが定義されていません。</exception>
        public static ClassWrapperBase CreateFromSource(Type wrapperType, object src)
        {
            if (src is null)
            {
                return null;
            }
            else if (wrapperType is null)
            {
                throw new ArgumentNullException(nameof(wrapperType));
            }
            else if (!wrapperType.IsSubclassOf(typeof(ClassWrapperBase)))
            {
                throw new ArgumentException(string.Format(Resources.Value.TypeNotClassWrapper.Value, wrapperType.FullName, typeof(ClassWrapperBase).Name), nameof(wrapperType));
            }

            FastMethod method = BveTypes.GetCreateFromSourceMethod(wrapperType);
            return method is null
                ? throw new InvalidOperationException(string.Format(Resources.Value.FromSourceMethodNotFound.Value, wrapperType.FullName))
                : method.Invoke(null, new object[] { src }) as ClassWrapperBase;
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// ラッパー型はオリジナル オブジェクトの型から自動で推定されます。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップしたインスタンス。</returns>
        /// <exception cref="InvalidOperationException">ラッパー型内に有効なクラスラッパー生成メソッドが定義されていません。</exception>
        public static ClassWrapperBase CreateFromSource(object src)
        {
            if (src is null) return null;

            Type wrapperType = BveTypes.GetWrapperTypeOf(src.GetType());
            return CreateFromSource(wrapperType, src);
        }

        /// <inheritdoc/>
        public static bool operator ==(ClassWrapperBase left, ClassWrapperBase right) => left is null ? right is null : left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(ClassWrapperBase left, ClassWrapperBase right) => !(left == right);

        /// <summary>
        /// ラップされているオリジナル オブジェクトを取得します。
        /// </summary>
        public object Src { get; }

        /// <summary>
        /// <see cref="ClassWrapperBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="src"/> が <see langword="null"/> です。</exception>
        public ClassWrapperBase(object src)
        {
            if (src is null) throw new ArgumentNullException();

            Src = src;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is ClassWrapperBase classWrapper && Src.Equals(classWrapper.Src);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Src.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Src.ToString();
        }
    }
}
