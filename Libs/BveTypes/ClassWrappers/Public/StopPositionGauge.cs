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
    /// 「停止位置」補助表示を表します。
    /// </summary>
    public class StopPositionGauge : AssistantBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<StopPositionGauge>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StopPositionGauge"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StopPositionGauge(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StopPositionGauge"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static StopPositionGauge FromSource(object src) => src is null ? null : new StopPositionGauge(src);
    }
}
