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
    /// 応荷重制御装置を表します。
    /// </summary>
    public class LoadCompensator : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<LoadCompensator>();

            IsEnabledGetMethod = members.GetSourcePropertyGetterOf(nameof(IsEnabled));
            IsEnabledSetMethod = members.GetSourcePropertySetterOf(nameof(IsEnabled));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="LoadCompensator"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected LoadCompensator(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="LoadCompensator"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static LoadCompensator FromSource(object src) => src is null ? null : new LoadCompensator(src);

        private static FastMethod IsEnabledGetMethod;
        private static FastMethod IsEnabledSetMethod;
        /// <summary>
        /// 応荷重制御が有効かどうかを取得・設定します。
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)IsEnabledGetMethod.Invoke(Src, null);
            set => IsEnabledSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
