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
    /// 閉塞を表します。
    /// </summary>
    public class Section : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Section>();

            SectionIndexesTrainOnGetMethod = members.GetSourcePropertyGetterOf(nameof(SectionIndexesTrainOn));
            SectionIndexesTrainOnSetMethod = members.GetSourcePropertySetterOf(nameof(SectionIndexesTrainOn));

            CurrentSignalIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(CurrentSignalIndex));

            SignalIndexesGetMethod = members.GetSourcePropertyGetterOf(nameof(SignalIndexes));
            SignalIndexesSetMethod = members.GetSourcePropertySetterOf(nameof(SignalIndexes));

            SectionCountGetMethod = members.GetSourcePropertyGetterOf(nameof(SectionCount));
            SectionCountSetMethod = members.GetSourcePropertySetterOf(nameof(SectionCount));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Section"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Section(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Section"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Section FromSource(object src) => src is null ? null : new Section(src);

        private static FastMethod SectionIndexesTrainOnGetMethod;
        private static FastMethod SectionIndexesTrainOnSetMethod;
        /// <summary>
        /// 列車 (自列車・先行列車) が走行している閉塞のインデックスの一覧を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 要素は昇順にソートされています。
        /// </remarks>
        public List<int> SectionIndexesTrainOn
        {
            get => SectionIndexesTrainOnGetMethod.Invoke(Src, null) as List<int>;
            set => SectionIndexesTrainOnSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CurrentSignalIndexGetMethod;
        /// <summary>
        /// 現在の信号現示のインデックスを取得します。
        /// </summary>
        public int CurrentSignalIndex => (int)CurrentSignalIndexGetMethod.Invoke(Src, null);

        private static FastMethod SignalIndexesGetMethod;
        private static FastMethod SignalIndexesSetMethod;
        /// <summary>
        /// 信号現示インデックスの一覧を取得・設定します。
        /// </summary>
        public int[] SignalIndexes
        {
            get => SignalIndexesGetMethod.Invoke(Src, null) as int[];
            set => SignalIndexesSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SectionCountGetMethod;
        private static FastMethod SectionCountSetMethod;
        /// <summary>
        /// 閉塞の総数を取得・設定します。
        /// </summary>
        public int SectionCount
        {
            get => (int)SectionCountGetMethod.Invoke(Src, null);
            set => SectionCountSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
