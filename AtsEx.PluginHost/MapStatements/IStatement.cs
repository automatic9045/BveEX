using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveTypes.ClassWrappers;

namespace AtsEx.PluginHost.MapStatements
{
    /// <summary>
    /// Repeater ステートメントを使用して独自に定義する構文を表します。
    /// </summary>
    /// <remarks>
    /// 互換性のために残されている古い機能です。AtsEx.CoreExtensions のマップステートメント機能をご利用ください。<br/>
    /// <see cref="IHeader"/> と異なり、シナリオの読込が完了して <see cref="IBveHacker.ScenarioCreated"/> イベントが発生するまでは取得できませんが、
    /// マップ内の変数を埋め込んだり、定義した距離程と関連付けたりすることが可能です。
    /// </remarks>
    [Obsolete]
    public interface IStatement
    {
        /// <summary>
        /// このステートメントの名前を表す識別子を取得します。
        /// </summary>
        /// <remarks>
        /// 使用する 1 番目のストラクチャーのキー (structureKey1) が該当します。
        /// </remarks>
        Identifier Name { get; }

        /// <summary>
        /// このステートメントの宣言の付加情報を表す識別子の一覧を取得します。
        /// </summary>
        /// <remarks>
        /// 使用する 2 番目以降のストラクチャーのキー (structureKey2、structureKey3、…、structureKeyN) が該当します。
        /// </remarks>
        Identifier[] AdditionalDeclaration { get; }

        /// <summary>
        /// このヘッダーの引数のテキストを取得します。
        /// </summary>
        /// <remarks>
        /// 連続ストラクチャー名が該当します。
        /// </remarks>
        string Argument { get; }

        /// <summary>
        /// このステートメントが設置されている範囲の始端の距離程 [m] を取得します。
        /// </summary>
        double From { get; }

        /// <summary>
        /// このステートメントが設置されている範囲の終端の距離程 [m] を取得します。
        /// </summary>
        double To { get; }

        /// <summary>
        /// このステートメントの定義に使用されている連続ストラクチャーを取得します。
        /// </summary>
        RepeatedStructure DefinedStructure { get; }

        /// <summary>
        /// <see cref="ObserveTrain(Train)"/> メソッド、あるいは <see cref="ObserveTrains(IEnumerable{Train})"/> メソッドによって登録された、動作を監視している配置済み他列車のコレクションを取得します。
        /// </summary>
        IReadOnlyCollection<Train> TrainsObserving { get; }

        /// <summary>
        /// 指定した配置済み他列車の動作の監視を開始します。
        /// </summary>
        /// <remarks>
        /// <c>Train.Add</c> ステートメント、あるいは <c>Train[trainKey].Load</c> ステートメントによってマップへ配置されている他列車を表す <see cref="Train"/> のみ使用可能です。<br />
        /// AtsEX プラグインから独自に定義した <see cref="Train"/> を使用することはできません。
        /// </remarks>
        /// <param name="train">動作を監視する配置済み他列車。</param>
        void ObserveTrain(Train train);

        /// <summary>
        /// 指定した複数の配置済み他列車の動作の監視を開始します。
        /// </summary>
        /// <remarks>
        /// <c>Train.Add</c> ステートメント、あるいは <c>Train[trainKey].Load</c> ステートメントによってマップへ配置されている他列車を表す <see cref="Train"/> のみ使用可能です。<br />
        /// AtsEX プラグインから独自に定義した <see cref="Train"/> を使用することはできません。
        /// </remarks>
        /// <param name="trains">動作を監視する配置済み他列車のコレクション。</param>
        void ObserveTrains(IEnumerable<Train> trains);

        /// <summary>
        /// このステートメント上へ自列車が進入した瞬間に発生します。
        /// </summary>
        event EventHandler<PassedEventArgs> Entered;

        /// <summary>
        /// このステートメント上へ先行列車が進入した瞬間に発生します。
        /// </summary>
        event EventHandler<PassedEventArgs> PreTrainEntered;

        /// <summary>
        /// このステートメント上へ他列車が進入した瞬間に発生します。
        /// </summary>
        event EventHandler<TrainPassedEventArgs> TrainEntered;

        /// <summary>
        /// このステートメント上から自列車が退出した瞬間に発生します。
        /// </summary>
        event EventHandler<PassedEventArgs> Exited;

        /// <summary>
        /// このステートメント上から先行列車が退出した瞬間に発生します。
        /// </summary>
        event EventHandler<PassedEventArgs> PreTrainExited;

        /// <summary>
        /// このステートメント上から他列車が退出した瞬間に発生します。
        /// </summary>
        event EventHandler<TrainPassedEventArgs> TrainExited;

    }
}
