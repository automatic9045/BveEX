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
    /// 運転台のハンドルを表します。
    /// </summary>
    public class CabBase : ClassWrapperBase, IDisposable
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<CabBase>();

            HandlesGetMethod = members.GetSourcePropertyGetterOf(nameof(Handles));

            GetDescriptionTextMethod = members.GetSourceMethodOf(nameof(GetDescriptionText));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="CabBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected CabBase(object src) : base(src)
        {
        }

        private static FastMethod HandlesGetMethod;
        /// <summary>
        /// 操作可能なハンドルのセットを表す <see cref="HandleSet"/> を取得します。
        /// </summary>
        public HandleSet Handles => HandleSet.FromSource(HandlesGetMethod.Invoke(Src, null));

        private static FastMethod GetDescriptionTextMethod;
        /// <summary>
        /// 現在の状態を説明するテキストを取得します。
        /// </summary>
        public string GetDescriptionText() => GetDescriptionTextMethod.Invoke(Src, null) as string;

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
