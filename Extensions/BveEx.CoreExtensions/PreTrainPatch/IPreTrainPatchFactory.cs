using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.PreTrainPatch
{
    /// <summary>
    /// <see cref="PreTrainPatch"/> クラスの新しいインスタンスを初期化するたのファクトリを表します。
    /// </summary>
    public interface IPreTrainPatchFactory : IExtension
    {
        /// <summary>
        /// 先行列車の走行位置を自由に変更できるようにするパッチを適用します。
        /// </summary>
        /// <param name="name">パッチの名前。</param>
        /// <param name="sectionManager">先行列車の走行位置の変更に使用する <see cref="SectionManager"/>。</param>
        /// <param name="converter">本来の走行位置から、実際に使用する走行位置に変換するためのコンバーター。</param>
        /// <returns>パッチを表す <see cref="PreTrainPatch"/>。</returns>
        PreTrainPatch Patch(string name, SectionManager sectionManager, IPreTrainLocationConverter converter);
    }
}
