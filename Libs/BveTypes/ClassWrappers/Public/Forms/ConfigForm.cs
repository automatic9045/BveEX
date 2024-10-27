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
    /// 「設定」フォームを表します。
    /// </summary>
    public class ConfigForm : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ConfigForm>();

            Constructor = members.GetSourceConstructor();

            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ConfigForm"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ConfigForm(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ConfigForm"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ConfigForm FromSource(object src) => src is null ? null : new ConfigForm(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="ConfigForm"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="mainForm">メインのフォーム。</param>
        public ConfigForm(MainForm mainForm) : this(Constructor.Invoke(new object[] { mainForm?.Src }))
        {
        }

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(this, null);
    }
}
