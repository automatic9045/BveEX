using System;
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
    /// Structure マップ要素、Repeater マップ要素で設置されたストラクチャーの情報を提供します。
    /// </summary>
    public class StructureSet : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<StructureSet>();

            DrawLimitLocationGetMethod = members.GetSourcePropertyGetterOf(nameof(DrawLimitLocation));
            DrawLimitLocationSetMethod = members.GetSourcePropertySetterOf(nameof(DrawLimitLocation));

            RepeatedGetMethod = members.GetSourcePropertyGetterOf(nameof(Repeated));

            PutGetMethod = members.GetSourcePropertyGetterOf(nameof(Put));

            PutBetweenGetMethod = members.GetSourcePropertyGetterOf(nameof(PutBetween));

            SignalsGetMethod = members.GetSourcePropertyGetterOf(nameof(Signals));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StructureSet"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StructureSet(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StructureSet"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static StructureSet FromSource(object src) => src is null ? null : new StructureSet(src);

        private static FastMethod DrawLimitLocationGetMethod;
        private static FastMethod DrawLimitLocationSetMethod;
        /// <summary>
        /// ストラクチャーが設置される限界の距離程 [m] を取得・設定します。通常は最後の駅の 10km 先の位置になります。
        /// </summary>
        /// <remarks>
        /// この数値を変更しても BVE には反映されません。
        /// </remarks>
        public double DrawLimitLocation
        {
            get => (double)DrawLimitLocationGetMethod.Invoke(Src, null);
            set => DrawLimitLocationSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod RepeatedGetMethod;
        /// <summary>
        /// Repeater マップ要素で設置されたストラクチャーを取得します。
        /// </summary>
        /// <value>キーが RepeaterKey で指定した連続ストラクチャーー名、値が設置するストラクチャーのリストを表す <see cref="MapObjectList"/> の <see cref="WrappedSortedList{TKey, TValueWrapper}"/>。</value>
        public WrappedSortedList<string, MapObjectList> Repeated
        {
            get
            {
                IDictionary dictionarySrc = RepeatedGetMethod.Invoke(Src, null) as IDictionary;
                return new WrappedSortedList<string, MapObjectList>(dictionarySrc);
            }
        }

        private static FastMethod PutGetMethod;
        /// <summary>
        /// Structure[].Put ステートメント、Structure[].Put0 ステートメントで設置されたストラクチャーを取得します。
        /// </summary>
        public SingleStructureList Put => SingleStructureList.FromSource(PutGetMethod.Invoke(Src, null));

        private static FastMethod PutBetweenGetMethod;
        /// <summary>
        /// Structure[].PutBetween ステートメントで設置されたストラクチャーを取得します。
        /// </summary>
        public SingleStructureList PutBetween => SingleStructureList.FromSource(PutBetweenGetMethod.Invoke(Src, null));

        private static FastMethod SignalsGetMethod;
        /// <summary>
        /// Signal[].Put ステートメントで設置された信号ストラクチャーを取得します。
        /// </summary>
        public SingleStructureList Signals => SingleStructureList.FromSource(SignalsGetMethod.Invoke(Src, null));
    }
}
