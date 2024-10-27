using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 「時刻と位置」フォームを表します。
    /// </summary>
    public class TimePosForm : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TimePosForm>();

            SetScenarioMethod = members.GetSourceMethodOf(nameof(SetScenario));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TimePosForm"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TimePosForm(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TimePosForm"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static TimePosForm FromSource(object src) => src is null ? null : new TimePosForm(src);

        private static FastMethod SetScenarioMethod;
        /// <summary>
        /// 指定したシナリオに基づいてダイヤグラムの表示を初期化します。
        /// </summary>
        /// <param name="scenario">シナリオを表す <see cref="Scenario"/>。</param>
        public void SetScenario(Scenario scenario) => SetScenarioMethod.Invoke(Src, new object[] { scenario?.Src });

        private static FastMethod DrawMethod;
        /// <summary>
        /// ダイヤグラムを描画します。
        /// </summary>
        public void Draw() => DrawMethod.Invoke(Src, null);
    }
}
