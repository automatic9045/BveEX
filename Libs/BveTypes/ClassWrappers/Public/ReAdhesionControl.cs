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
    /// 再粘着制御を表します。
    /// </summary>
    public class ReAdhesionControl : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<ReAdhesionControl>();

            ModeGetMethod = members.GetSourcePropertyGetterOf(nameof(Mode));

            IsEnabledGetMethod = members.GetSourcePropertyGetterOf(nameof(IsEnabled));
            IsEnabledSetMethod = members.GetSourcePropertySetterOf(nameof(IsEnabled));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="ReAdhesionControl"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected ReAdhesionControl(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="ReAdhesionControl"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static ReAdhesionControl FromSource(object src) => src is null ? null : new ReAdhesionControl(src);

        private static FastMethod ModeGetMethod;
        /// <summary>
        /// 再粘着制御の動作状態を取得します。
        /// </summary>
        public ReAdhesionControlMode Mode => (ReAdhesionControlMode)ModeGetMethod.Invoke(Src, null);

        private static FastMethod IsEnabledGetMethod;
        private static FastMethod IsEnabledSetMethod;
        /// <summary>
        /// 再粘着制御を使用するかどうかを取得・設定します。
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)IsEnabledGetMethod.Invoke(Src, null);
            set => IsEnabledSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
