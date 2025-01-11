using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// 空気圧縮機を表します。
    /// </summary>
    public class Compressor : ClassWrapperBase, ITickable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<Compressor>();

            UpperPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(UpperPressure));
            UpperPressureSetMethod = members.GetSourcePropertySetterOf(nameof(UpperPressure));

            LowerPressureGetMethod = members.GetSourcePropertyGetterOf(nameof(LowerPressure));
            LowerPressureSetMethod = members.GetSourcePropertySetterOf(nameof(LowerPressure));

            CompressionSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(CompressionSpeed));
            CompressionSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(CompressionSpeed));

            LeakSpeedGetMethod = members.GetSourcePropertyGetterOf(nameof(LeakSpeed));
            LeakSpeedSetMethod = members.GetSourcePropertySetterOf(nameof(LeakSpeed));

            IsWorkingField = members.GetSourceFieldOf(nameof(IsWorking));

            InitializeMethod = members.GetSourceMethodOf(nameof(Initialize));
            TickMethod = members.GetSourceMethodOf(nameof(Tick));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="Compressor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected Compressor(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="Compressor"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static Compressor FromSource(object src) => src is null ? null : new Compressor(src);

        private static FastMethod UpperPressureGetMethod;
        private static FastMethod UpperPressureSetMethod;
        /// <summary>
        /// 空気圧縮機が停止する元空気溜め圧力 [Pa] を取得・設定します。
        /// </summary>
        public double UpperPressure
        {
            get => (double)UpperPressureGetMethod.Invoke(Src, null);
            set => UpperPressureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod LowerPressureGetMethod;
        private static FastMethod LowerPressureSetMethod;
        /// <summary>
        /// 空気圧縮機が始動する元空気溜め圧力 [Pa] を取得・設定します。
        /// </summary>
        public double LowerPressure
        {
            get => (double)LowerPressureGetMethod.Invoke(Src, null);
            set => LowerPressureSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod CompressionSpeedGetMethod;
        private static FastMethod CompressionSpeedSetMethod;
        /// <summary>
        /// 空気圧縮機による元空気溜め圧力上昇速度 [Pa/s] を取得・設定します。
        /// </summary>
        public double CompressionSpeed
        {
            get => (double)CompressionSpeedGetMethod.Invoke(Src, null);
            set => CompressionSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastMethod LeakSpeedGetMethod;
        private static FastMethod LeakSpeedSetMethod;
        /// <summary>
        /// 元空気溜めの空気漏れ速度 [Pa/s] を取得・設定します。
        /// </summary>
        public double LeakSpeed
        {
            get => (double)LeakSpeedGetMethod.Invoke(Src, null);
            set => LeakSpeedSetMethod.Invoke(Src, new object[] { value });
        }

        private static FastField IsWorkingField;
        /// <summary>
        /// 動作中かどうかを取得・設定します。
        /// </summary>
        public bool IsWorking
        {
            get => (bool)IsWorkingField.GetValue(Src);
            set => IsWorkingField.SetValue(Src, value);
        }

        private static FastMethod InitializeMethod;
        /// <inheritdoc/>
        public void Initialize()
            => InitializeMethod.Invoke(Src, null);

        private static FastMethod TickMethod;
        /// <inheritdoc/>
        public void Tick(double elapsedSeconds)
            => TickMethod.Invoke(Src, new object[] { elapsedSeconds });

        /// <summary>
        /// 毎フレーム呼び出されます。
        /// </summary>
        /// <remarks>
        /// このメソッドはオリジナルではないため、<see cref="ClassMemberSet.GetSourceMethodOf(string, Type[])"/> メソッドから参照することはできません。<br/>
        /// このメソッドのオリジナルバージョンは <see cref="Tick(double)"/> です。
        /// </remarks>
        /// <param name="elapsed">前フレームからの経過時間。</param>
        /// <seealso cref="Tick(double)"/>
        public void Tick(TimeSpan elapsed) => Tick(elapsed.TotalSeconds);
    }
}
