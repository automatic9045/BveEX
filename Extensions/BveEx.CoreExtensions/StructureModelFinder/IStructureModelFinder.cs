using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.StructureModelFinder
{
    /// <summary>
    /// ストラクチャーの 3D モデルを簡単に検索するための機能を提供します。
    /// </summary>
    public interface IStructureModelFinder : IExtension
    {
        /// <summary>
        /// ストラクチャーキーから 3D モデルを検索します。
        /// </summary>
        /// <param name="structureKey">ストラクチャーキー。</param>
        /// <returns><paramref name="structureKey"/> に対応する 3D モデル。</returns>
        Model GetModel(string structureKey);

        /// <summary>
        /// 3D モデルからストラクチャーキーを検索します。
        /// </summary>
        /// <param name="model">3D モデル。</param>
        /// <returns><paramref name="model"/> に対応するストラクチャーキー。</returns>
        string GetStructureKey(Model model);
    }
}
