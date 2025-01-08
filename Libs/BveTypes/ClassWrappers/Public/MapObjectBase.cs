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
    /// マップ上に設置されたすべてのオブジェクトの基本クラスを表します。
    /// </summary>
    /// <remarks>
    /// ストラクチャーに限らず、停車場や信号機、地上子など、ステートメントによって設置したすべてのオブジェクトを表します。
    /// </remarks>
    /// <seealso cref="MapObjectList"/>
    /// <seealso cref="LocatableMapObject"/>
    /// <seealso cref="PutBetweenStructure"/>
    /// <seealso cref="Station"/>
    /// <seealso cref="Structure"/>
    /// <seealso cref="ValueNode{T}"/>
    public class MapObjectBase : ClassWrapperBase, IComparable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapObjectBase>();

            LocationGetMethod = members.GetSourcePropertyGetterOf(nameof(Location));
            LocationSetMethod = members.GetSourcePropertySetterOf(nameof(Location));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapObjectBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapObjectBase(object src) : base(src)
        {
        }

        private static FastMethod LocationGetMethod;
        private static FastMethod LocationSetMethod;
        /// <summary>
        /// 設置された距離程を取得・設定します。
        /// </summary>
        /// <remarks>
        /// マップ オブジェクトの種類によっては、この数値を変更しても BVE に反映されない場合があります。
        /// </remarks>
        public double Location
        {
            get => (double)LocationGetMethod.Invoke(Src, null);
            set => LocationSetMethod.Invoke(Src, new object[] { value });
        }

        /// <inheritdoc/>
        public int CompareTo(object obj) => obj is MapObjectBase mapObject ? (int)(Src as IComparable).CompareTo(mapObject.Src) : throw new ArgumentException();
    }
}
