using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using BveTypes.ClassWrappers.Extensions;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ランダムに 1 つの項目が選ばれる <see cref="BveFile"/> のリストを表します。
    /// </summary>
    public class RandomFileList : WrappedList<BveFile>
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<RandomFileList>();

            SelectedFileGetMethod = members.GetSourcePropertyGetterOf(nameof(SelectedFile));
            SelectedFileSetMethod = members.GetSourcePropertySetterOf(nameof(SelectedFile));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="RandomFileList"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected RandomFileList(IList src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="RandomFileList"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static RandomFileList FromSource(object src) => src is null ? null : new RandomFileList((IList)src);

        private static FastMethod SelectedFileGetMethod;
        private static FastMethod SelectedFileSetMethod;
        /// <summary>
        /// ランダムに選ばれた項目を取得・設定します。
        /// </summary>
        public BveFile SelectedFile
        {
            get => BveFile.FromSource(SelectedFileGetMethod.Invoke(Src, null));
            set => SelectedFileGetMethod.Invoke(Src, new object[] { value?.Src });
        }
    }
}
