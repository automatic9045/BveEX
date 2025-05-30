﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// <see cref="MapObjectBase"/> のリストを表します。
    /// </summary>
    /// <seealso cref="SingleStructureList"/>
    /// <seealso cref="StationList"/>
    public class MapObjectList : WrappedList<MapObjectBase>
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<MapObjectList>();

            Constructor = members.GetSourceConstructor();

            CurrentIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentIndex));
            CurrentIndexSetMethod = members.GetSourcePropertySetterOf(nameof(CurrentIndex));

            ObjectPassedEvent = new WrapperEvent<ObjectPassedEventHandler>(
                members.GetSourceEventOf(nameof(ObjectPassed)),
                x => (sender, e) => x?.Invoke(FromSource(sender), ObjectPassedEventArgs.FromSource(e)));

            InsertMethod = members.GetSourceMethodOf(nameof(Insert), new Type[] { typeof(MapObjectBase) });
            GetNextMethod = members.GetSourceMethodOf(nameof(GetNext));
            GoToAndGetNextMethod = members.GetSourceMethodOf(nameof(GoToAndGetNext));
            GetCurrentMethod = members.GetSourceMethodOf(nameof(GetCurrent));
            GoToAndGetCurrentMethod = members.GetSourceMethodOf(nameof(GoToAndGetCurrent));
            GoToMethod1 = members.GetSourceMethodOf(nameof(GoTo), new Type[] { typeof(double) });
            GoToMethod2 = members.GetSourceMethodOf(nameof(GoTo), new Type[] { typeof(double), typeof(double) });
            GoToByIndexMethod = members.GetSourceMethodOf(nameof(GoToByIndex));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="MapObjectList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected MapObjectList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="MapObjectList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static MapObjectList FromSource(object src) => src is null ? null : new MapObjectList((IList)src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="MapObjectList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MapObjectList() : base(Constructor.Invoke(null) as IList)
        {
        }

        private static FastMethod CurrentIndexGetMethod;
        private static FastMethod CurrentIndexSetMethod;
        /// <summary>
        /// 現在の距離程に対応するコレクションの要素のインデックスを取得・設定します。
        /// </summary>
        public int CurrentIndex
        {
            get => (int)CurrentIndexGetMethod.Invoke(Src, null);
            set => CurrentIndexSetMethod.Invoke(Src, new object[] { value });
        }

        private static WrapperEvent<ObjectPassedEventHandler> ObjectPassedEvent;
        /// <summary>
        /// <see cref="CurrentIndex"/> の値が変更されたときに発生します。
        /// </summary>
        public event ObjectPassedEventHandler ObjectPassed
        {
            add => ObjectPassedEvent.Add(Src, value);
            remove => ObjectPassedEvent.Remove(Src, value);
        }
        /// <summary>
        /// <see cref="ObjectPassed"/> イベントを実行します。
        /// </summary>
        /// <param name="args">自列車が通過したマップ オブジェクト。</param>
        public void ObjectPassed_Invoke(ObjectPassedEventArgs args) => ObjectPassedEvent.Invoke(Src, args);

        private static FastMethod GetNextMethod;
        /// <summary>
        /// 現在の距離程に対応するコレクションの要素の次を取得します。
        /// </summary>
        public MapObjectBase GetNext()
        {
            object src = GetNextMethod.Invoke(Src, null);
            return CreateFromSource(src) as MapObjectBase;
        }

        private static FastMethod GoToAndGetNextMethod;
        /// <summary>
        /// 指定した距離程へ移動し、その距離程に対応するコレクションの要素の次を取得します。
        /// </summary>
        /// <param name="location">移動先の距離程 [m]。</param>
        public MapObjectBase GoToAndGetNext(int location)
        {
            object src = GoToAndGetNextMethod.Invoke(Src, new object[] { location });
            return CreateFromSource(src) as MapObjectBase;
        }

        private static FastMethod GetCurrentMethod;
        /// <summary>
        /// 現在の距離程に対応するコレクションの要素を取得します。
        /// </summary>
        public MapObjectBase GetCurrent()
        {
            object src = GetCurrentMethod.Invoke(Src, null);
            return CreateFromSource(src) as MapObjectBase;
        }

        private static FastMethod GoToAndGetCurrentMethod;
        /// <summary>
        /// 指定した距離程へ移動し、その距離程に対応するコレクションの要素を取得します。
        /// </summary>
        /// <param name="location">移動先の距離程 [m]。</param>
        public MapObjectBase GoToAndGetCurrent(double location)
        {
            object src = GoToAndGetCurrentMethod.Invoke(Src, new object[] { location });
            return CreateFromSource(src) as MapObjectBase;
        }

        private static FastMethod InsertMethod;
        /// <summary>
        /// 距離程順に新しい項目を追加します。
        /// </summary>
        /// <param name="item">追加するマップ オブジェクト。</param>
        public void Insert(MapObjectBase item) => InsertMethod.Invoke(Src, new object[] { item?.Src });

        private static FastMethod GoToMethod1;
        /// <summary>
        /// 指定した距離程へ移動します。
        /// </summary>
        /// <param name="newLocation">自列車の新たな距離程 [m]。</param>
        public void GoTo(double newLocation) => GoToMethod1.Invoke(Src, new object[] { newLocation });

        private static FastMethod GoToMethod2;
        /// <summary>
        /// 指定した距離程へ移動します。
        /// </summary>
        /// <param name="newLocation">自列車の新たな距離程 [m]。</param>
        /// <param name="displacement">自列車の移動した距離 [m]。</param>
        public void GoTo(double newLocation, double displacement) => GoToMethod2.Invoke(Src, new object[] { newLocation, displacement });

        private static FastMethod GoToByIndexMethod;
        /// <summary>
        /// 指定したインデックスへ移動します。
        /// </summary>
        /// <param name="index">移動先のインデックス。</param>
        public void GoToByIndex(int index) => GoToByIndexMethod.Invoke(Src, new object[] { index });


        /// <summary>
        /// 自列車がマップ オブジェクトを通過したときに発生するイベントを処理するメソッドを表します。
        /// </summary>
        /// <param name="sender">イベントのソース。</param>
        /// <param name="e">イベントデータを格納している <see cref="ObjectPassedEventArgs"/>。</param>
        public delegate void ObjectPassedEventHandler(object sender, ObjectPassedEventArgs e);
    }
}
