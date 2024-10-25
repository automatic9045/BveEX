using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlimDX;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.TrainDrawPatch
{
    /// <summary>
    /// <see cref="TrainDrawPatch"/> クラスの新しいインスタンスを初期化するたのファクトリを表します。
    /// </summary>
    public interface ITrainDrawPatchFactory : IExtension
    {
        /// <summary>
        /// 指定した他列車に、自由に移動・回転できるようにするパッチを適用します。
        /// </summary>
        /// <param name="name">パッチの名前。</param>
        /// <param name="target">パッチを適用する他列車。</param>
        /// <param name="worldMatrixConverter">本来の配置先に設置するワールド変換行列から、実際に使用する行列に変換するためのコンバーター。</param>
        /// <returns>パッチを表す <see cref="TrainDrawPatch"/>。</returns>
        TrainDrawPatch Patch(string name, Train target, IMatrixConverter worldMatrixConverter);

        /// <summary>
        /// 指定した他列車に、自由に移動・回転できるようにするパッチを適用します。
        /// </summary>
        /// <param name="name">パッチの名前。</param>
        /// <param name="target">パッチを適用する他列車。</param>
        /// <param name="worldMatrixConverter">本来の配置先に設置するワールド変換行列から、実際に使用する行列に変換するためのコンバーター。</param>
        /// <param name="viewMatrixConverter">本来の配置先に設置するビュー変換行列から、実際に使用する行列に変換するためのコンバーター。</param>
        /// <returns>パッチを表す <see cref="TrainDrawPatch"/>。</returns>
        TrainDrawPatch Patch(string name, Train target, IMatrixConverter worldMatrixConverter, IMatrixConverter viewMatrixConverter);

        /// <summary>
        /// 指定した他列車に、自由に移動・回転できるようにするパッチを適用します。
        /// </summary>
        /// <param name="name">パッチの名前。</param>
        /// <param name="target">パッチを適用する他列車。</param>
        /// <param name="overrideAction"><see cref="Train.DrawCars(Direct3DProvider, Matrix)"/> メソッドの処理をオーバーライドするデリゲート。</param>
        /// <returns>パッチを表す <see cref="TrainDrawPatch"/>。</returns>
        TrainDrawPatch Patch(string name, Train target, Action<Direct3DProvider, Matrix> overrideAction);
    }
}
