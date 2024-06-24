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
    /// 自列車の運転台のデジタル数字計器要素を表します。
    /// </summary>
    public class DigitalNumber : VehiclePanelElement
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<DigitalNumber>();

            Constructor = members.GetSourceConstructor();

            IntervalGetMethod = members.GetSourcePropertyGetterOf(nameof(Interval));
            IntervalSetMethod = members.GetSourcePropertySetterOf(nameof(Interval));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="DigitalNumber"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected DigitalNumber(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="DigitalNumber"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new DigitalNumber FromSource(object src) => src is null ? null : new DigitalNumber(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="DigitalNumber"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DigitalNumber() : this(Constructor.Invoke(null))
        {
        }

        private static FastMethod IntervalGetMethod;
        private static FastMethod IntervalSetMethod;
        /// <summary>
        /// 数字配列画像の 1 コマの高さを取得・設定します。
        /// </summary>
        public int Interval
        {
            get => IntervalGetMethod.Invoke(Src, null);
            set => IntervalSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
