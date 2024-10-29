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
    /// 「乗客の揺れ」補助表示を表します。
    /// </summary>
    public class SwayGauge : AssistantBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SwayGauge>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SwayGauge"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SwayGauge(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SwayGauge"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static SwayGauge FromSource(object src) => src is null ? null : new SwayGauge(src);
    }
}
