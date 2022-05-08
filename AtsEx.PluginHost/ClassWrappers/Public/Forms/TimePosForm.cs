﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Automatic9045.AtsEx.PluginHost.BveTypeCollection;

namespace Automatic9045.AtsEx.PluginHost.ClassWrappers
{
    /// <summary>
    /// 「時刻と位置」フォームを表します。
    /// </summary>
    public sealed class TimePosForm : ClassWrapper
    {
        static TimePosForm()
        {
            ClassMemberCollection members = BveTypeCollectionProvider.Instance.GetClassInfoOf<TimePosForm>();

            SetScenarioMethod = members.GetSourceMethodOf(nameof(SetScenario));

            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
        }

        private TimePosForm(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TimePosForm"/> クラスのインスタンス。</returns>
        public static TimePosForm FromSource(object src)
        {
            if (src is null) return null;
            return new TimePosForm(src);
        }

        private static MethodInfo SetScenarioMethod;
        /// <summary>
        /// 指定したシナリオに基づいてダイヤグラムの表示を初期化します。
        /// </summary>
        /// <param name="scenarioProvider">シナリオを表す <see cref="ScenarioProvider"/>。</param>
        public void SetScenario(ScenarioProvider scenarioProvider) => SetScenarioMethod.Invoke(Src, new object[] { scenarioProvider.Src });

        private static MethodInfo DrawMethod;
        /// <summary>
        /// ダイヤグラムを描画します。
        /// </summary>
        public void Draw() => DrawMethod.Invoke(Src, null);
    }
}