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
    /// 「制限速度」補助表示を表します。
    /// </summary>
    public class SpeedLimitText : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<SpeedLimitText>();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="SpeedLimitText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected SpeedLimitText(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="SpeedLimitText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new SpeedLimitText FromSource(object src) => src is null ? null : new SpeedLimitText(src);
    }
}
