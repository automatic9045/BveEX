﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

using Automatic9045.AtsEx.PluginHost.BveTypes;

namespace Automatic9045.AtsEx.PluginHost.ClassWrappers
{
    /// <summary>
    /// 自列車のブレーキシステム全体を表します。
    /// </summary>
    public class BrakeSystem : ClassWrapperBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<BrakeSystem>();

            MotorCarBcGetMethod = members.GetSourcePropertyGetterOf(nameof(MotorCarBc));

            TrailerCarBcGetMethod = members.GetSourcePropertyGetterOf(nameof(TrailerCarBc));

            FirstCarBcGetMethod = members.GetSourcePropertyGetterOf(nameof(FirstCarBc));
            FirstCarBcSetMethod = members.GetSourcePropertySetterOf(nameof(FirstCarBc));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="AssistantText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected BrakeSystem(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="BrakeSystem"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static BrakeSystem FromSource(object src) => src is null ? null : new BrakeSystem(src);

        private static FastMethod MotorCarBcGetMethod;
        /// <summary>
        /// 動力車のブレーキシリンダーの圧力調整弁を表す <see cref="CarBc"/> を取得します。
        /// </summary>
        public CarBc MotorCarBc => CarBc.FromSource(MotorCarBcGetMethod.Invoke(Src, null));

        private static FastMethod TrailerCarBcGetMethod;
        /// <summary>
        /// 付随車のブレーキシリンダーの圧力調整弁を表す <see cref="CarBc"/> を取得します。
        /// </summary>
        public CarBc TrailerCarBc => CarBc.FromSource(TrailerCarBcGetMethod.Invoke(Src, null));

        private static FastMethod FirstCarBcGetMethod;
        private static FastMethod FirstCarBcSetMethod;
        /// <summary>
        /// 先頭車両のブレーキシリンダーの圧力調整弁を表す <see cref="CarBc"/> を取得・設定します。
        /// </summary>
        public CarBc FirstCarBc
        {
            get => CarBc.FromSource(FirstCarBcGetMethod.Invoke(Src, null));
            set => FirstCarBcSetMethod.Invoke(Src, value.Src);
        }
    }
}