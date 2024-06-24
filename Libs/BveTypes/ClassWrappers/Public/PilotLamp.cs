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
    /// 自列車の運転台の表示灯計器要素を表します。
    /// </summary>
    public class PilotLamp : VehiclePanelElement
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<PilotLamp>();

            Constructor = members.GetSourceConstructor();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="PilotLamp"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected PilotLamp(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="PilotLamp"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new PilotLamp FromSource(object src) => src is null ? null : new PilotLamp(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="PilotLamp"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PilotLamp() : this(Constructor.Invoke(null))
        {
        }
    }
}
