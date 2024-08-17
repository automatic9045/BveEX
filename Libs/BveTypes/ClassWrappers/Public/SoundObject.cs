using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 自列車が通過した時にサウンドを再生するマップ オブジェクトを表します。
    /// </summary>
    public class SoundObject : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SoundObject>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(double), typeof(int), typeof(Sound) });

            SourceGetMethod = members.GetSourcePropertyGetterOf(nameof(Source));
            SourceSetMethod = members.GetSourcePropertySetterOf(nameof(Source));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SoundObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SoundObject(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SoundObject"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static SoundObject FromSource(object src) => src is null ? null : new SoundObject(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="SoundObject"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="_">使用しません。0 を指定してください。</param>
        /// <param name="source">自列車が通過した時に再生するサウンド。</param>
        public SoundObject(double location, int _, Sound source)
            : this(Constructor.Invoke(new object[] { location, _, source }))
        {
        }

        private static FastMethod SourceGetMethod;
        private static FastMethod SourceSetMethod;
        /// <summary>
        /// 自列車が通過した時に再生するサウンドを取得・設定します。
        /// </summary>
        public Sound Source
        {
            get => Sound.FromSource(SourceGetMethod.Invoke(Src, null));
            set => SourceSetMethod.Invoke(Src, new object[] { value.Src });
        }
    }
}
