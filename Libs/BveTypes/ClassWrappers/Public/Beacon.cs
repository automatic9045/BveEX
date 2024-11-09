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
    /// Beacon.Put ステートメントで設置された地上子を表します。
    /// </summary>
    public class Beacon : MapObjectBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Beacon>();

            Constructor = members.GetSourceConstructor(new Type[] { typeof(double), typeof(int), typeof(int), typeof(int) });

            TargetSectionIndexGetMethod = members.GetSourcePropertyGetterOf(nameof(TargetSectionIndex));
            TargetSectionIndexSetMethod = members.GetSourcePropertySetterOf(nameof(TargetSectionIndex));

            TypeGetMethod = members.GetSourcePropertyGetterOf(nameof(Type));
            TypeSetMethod = members.GetSourcePropertySetterOf(nameof(Type));

            SendDataGetMethod = members.GetSourcePropertyGetterOf(nameof(SendData));
            SendDataSetMethod = members.GetSourcePropertySetterOf(nameof(SendData));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Beacon"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Beacon(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Beacon"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Beacon FromSource(object src) => src is null ? null : new Beacon(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="Beacon"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// <paramref name="targetSectionIndex"/> に指定するのは関連づける閉塞の絶対インデックスです。設置位置から見た相対インデックスではありませんので注意してください。
        /// </remarks>
        /// <param name="location">設置する距離程 [m]。</param>
        /// <param name="type">保安装置に送る地上子種別。</param>
        /// <param name="targetSectionIndex">関連づける閉塞の絶対インデックス。</param>
        /// <param name="sendData">保安装置に送る値。</param>
        public Beacon(double location, int type, int targetSectionIndex, int sendData)
            : this(Constructor.Invoke(new object[] { location, type, targetSectionIndex, sendData }))
        {
        }

        private static FastMethod TargetSectionIndexGetMethod;
        private static FastMethod TargetSectionIndexSetMethod;
        /// <summary>
        /// 関連づける閉塞の絶対インデックスを取得・設定します。
        /// </summary>
        public int TargetSectionIndex
        {
            get => (int)TargetSectionIndexGetMethod.Invoke(Src, null);
            set => TargetSectionIndexSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod TypeGetMethod;
        private static FastMethod TypeSetMethod;
        /// <summary>
        /// 保安装置に送る地上子種別を取得・設定します。
        /// </summary>
        public int Type
        {
            get => (int)TypeGetMethod.Invoke(Src, null);
            set => TypeSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod SendDataGetMethod;
        private static FastMethod SendDataSetMethod;
        /// <summary>
        /// 保安装置に送る値を取得・設定します。
        /// </summary>
        public int SendData
        {
            get => (int)SendDataGetMethod.Invoke(Src, null);
            set => SendDataSetMethod.Invoke(Src, new object[] { value });
        }
    }
}
