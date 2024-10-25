using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

using AtsEx.PluginHost;
using AtsEx.PluginHost.Plugins.Extensions;

namespace AtsEx.Extensions.ConductorPatch
{
    /// <summary>
    /// <see cref="ConductorPatch"/> クラスの新しいインスタンスを初期化するたのファクトリを表します。
    /// </summary>
    public interface IConductorPatchFactory : IExtension
    {
        /// <summary>
        /// BVE 本体にて <see cref="Conductor"/> が初期化された時に車掌の動作を上書きするパッチを適用するよう登録します。
        /// </summary>
        /// <remarks>
        /// <see cref="Conductor"/> が初期化されるのは <see cref="IBveHacker.ScenarioCreated"/> イベントより前です。
        /// このメソッドを実行するタイミングによっては、パッチが適用されない場合もあるため注意してください。
        /// </remarks>
        /// <param name="conductorFactory">上書きする車掌の動作を定義した <see cref="ConductorBase"/> の派生クラスのインスタンスのファクトリデリゲート。</param>
        /// <param name="priority">この宣言の優先度。</param>
        /// <param name="patchedCallback">この宣言の優先度。</param>
        void BeginPatch(Func<Conductor, ConductorBase> conductorFactory, DeclarationPriority priority, Action<ConductorPatch> patchedCallback);

        /// <summary>
        /// 車掌の動作を上書きするパッチを適用します。
        /// </summary>
        /// <param name="conductor">上書きする車掌の動作を定義した <see cref="ConductorBase"/> の派生クラスのインスタンス。</param>
        /// <param name="priority">この宣言の優先度。</param>
        ConductorPatch Patch(ConductorBase conductor, DeclarationPriority priority = DeclarationPriority.Sequentially);

        /// <summary>
        /// 指定したパッチを削除します。
        /// </summary>
        /// <param name="patch">削除するパッチ。</param>
        void Unpatch(ConductorPatch patch);
    }
}
