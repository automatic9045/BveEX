using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;
using UnembeddedResources;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 停車場のリストを表します。
    /// </summary>
    public class StationList : MapObjectList
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<StationList>(@"PluginHost\ClassWrappers");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> SameLocation { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        static StationList()
        {
#if DEBUG
            _ = Resources.Value;
#endif
        }

        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<StationList>();

            InsertMethod = members.GetSourceMethodOf(nameof(Insert));
            GetStandardTimeMethod = members.GetSourceMethodOf(nameof(GetStandardTime));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StationList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StationList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StationList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new StationList FromSource(object src) => src is null ? null : new StationList((IList)src);

        /// <summary>
        /// <see cref="StationList"/> に項目を追加します。
        /// </summary>
        /// <remarks>
        /// このメソッドは非推奨です。<see cref="Insert(Station)"/> メソッドを使用してください。
        /// </remarks>
        /// <param name="item"><see cref="StationList"/> に追加するオブジェクト。</param>
        /// <exception cref="NotSupportedException">BVE がダイヤグラムを正常に描画できなくなるため、最初の駅と最後の駅の距離程を同一にすることはできません。</exception>
        [Obsolete(nameof(Insert) + "(" + nameof(Station) + ") メソッドを使用してください。")]
#pragma warning disable CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
        public override void Add(MapObjectBase item)
#pragma warning restore CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
        {
            if (Count > 0 && this[0].Location == item.Location)
            {
                throw new NotSupportedException(Resources.Value.SameLocation.Value);
            }

            base.Add(item);
        }

        private static FastMethod InsertMethod;
        /// <summary>
        /// <see cref="StationList"/> に項目を追加します。
        /// </summary>
        /// <param name="item"><see cref="StationList"/> に追加するオブジェクト。</param>
        /// <exception cref="NotSupportedException">BVE がダイヤグラムを正常に描画できなくなるため、最初の駅と最後の駅の距離程を同一にすることはできません。</exception>
        public void Insert(Station item)
        {
            if (Count > 0 && this[0].Location == item.Location)
            {
                throw new NotSupportedException(Resources.Value.SameLocation.Value);
            }
            
            InsertMethod.Invoke(Src, new object[] { item?.Src });
        }

        private static FastMethod GetStandardTimeMethod;
        /// <summary>
        /// 指定した距離程における標準通過時刻を停車場の発着時刻から計算します。
        /// </summary>
        /// <param name="location">指定する距離程 [m]。</param>
        /// <returns>0 時丁度から現在時刻までに経過したミリ秒数 [ms]。</returns>
        public int GetStandardTime(double location) => (int)GetStandardTimeMethod.Invoke(Src, new object[] { location });
    }
}
