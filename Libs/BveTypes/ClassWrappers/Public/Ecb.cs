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
    /// 電気指令式ブレーキを表します。
    /// </summary>
    public class Ecb : BrakeControllerBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Ecb>();

            TickMethod = members.GetSourceMethodOf(nameof(Tick));
            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Ecb"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Ecb(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Ecb"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Ecb FromSource(object src) => src is null ? null : new Ecb(src);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public override void Tick(double elapsedSeconds) => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public override void Initialize() => InitializeMethod.Invoke(Src, null);
    }
}
