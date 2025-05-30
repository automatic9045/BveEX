﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FastMember;
using TypeWrapping;

namespace BveTypes.ClassWrappers
{
    /// <summary>
    /// ワンハンドル式の運転台を表します。
    /// </summary>
    public class OneLeverCab : CabBase
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<OneLeverCab>();

            Constructor = members.GetSourceConstructor();
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="OneLeverCab"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected OneLeverCab(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="OneLeverCab"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static OneLeverCab FromSource(object src) => src is null ? null : new OneLeverCab(src);

        private static FastConstructor Constructor;
        /// <summary>
        /// <see cref="OneLeverCab"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="inputManager">キー入力の情報。</param>
        /// <param name="handles">ハンドル入力。</param>
        public OneLeverCab(InputManager inputManager, HandleSet handles) : this(Constructor.Invoke(new object[] { inputManager?.Src, handles?.Src }))
        {
        }
    }
}
