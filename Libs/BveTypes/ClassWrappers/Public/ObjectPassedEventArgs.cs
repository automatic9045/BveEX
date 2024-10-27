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
    /// 自列車がマップ オブジェクトを通過したときに発生するイベントのデータを提供します。
    /// </summary>
    public class ObjectPassedEventArgs : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ObjectPassedEventArgs>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(int), typeof(MapObjectBase) });

            MapObjectGetMethod = members.GetSourcePropertyGetterOf(nameof(MapObject));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ObjectPassedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ObjectPassedEventArgs(object src) : base(src)
        {
        }

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="ObjectPassedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="direction">通過方向。前方向の場合は 1、後方向の場合は -1 を指定してください。</param>
        /// <param name="mapObject">通過したマップオブジェクト。</param>
        public ObjectPassedEventArgs(int direction, MapObjectBase mapObject)
            : this(Constructor.Invoke(new object[] { direction, mapObject?.Src }))
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ObjectPassedEventArgs"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ObjectPassedEventArgs FromSource(object src) => src is null ? null : new ObjectPassedEventArgs(src);

        private static FastMethod MapObjectGetMethod;
        /// <summary>
        /// 通過したマップ オブジェクトを取得します。
        /// </summary>
        public MapObjectBase MapObject => (MapObjectBase)CreateFromSource(MapObjectGetMethod.Invoke(Src, null));
    }
}
