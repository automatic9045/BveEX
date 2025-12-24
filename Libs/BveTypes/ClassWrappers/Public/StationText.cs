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
    /// 「次駅情報」補助表示を表します。
    /// </summary>
    public class StationText : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<StationText>();

            StationsField = members.GetSourceFieldOf(nameof(Stations));
            LocationField = members.GetSourceFieldOf(nameof(Location));
            NextTextField = members.GetSourceFieldOf(nameof(NextText));
            NextField = members.GetSourceFieldOf(nameof(Next));
            DoorsField = members.GetSourceFieldOf(nameof(Doors));

            UpdateMethod = members.GetSourceMethodOf(nameof(Update));
            UpdateTextMethod = members.GetSourceMethodOf(nameof(UpdateText));
            DrawMethod = members.GetSourceMethodOf(nameof(Draw));
            DisposeMethod = members.GetSourceMethodOf(nameof(Dispose));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="StationText"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected StationText(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="StationText"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new StationText FromSource(object src) => src is null ? null : new StationText(src);

        private static FastField StationsField;
        /// <summary>
        /// 停車場のリストを取得・設定します。
        /// </summary>
        public StationList Stations
        {
            get => StationList.FromSource(StationsField.GetValue(Src));
            set => StationsField.SetValue(Src, value?.Src);
        }

        private static FastField LocationField;
        /// <summary>
        /// 自列車の位置情報を取得・設定します。
        /// </summary>
        public VehicleLocation Location
        {
            get => VehicleLocation.FromSource(LocationField.GetValue(Src));
            set => LocationField.SetValue(Src, value?.Src);
        }

        private static FastField NextTextField;
        /// <summary>
        /// 次駅を表すテキストを取得・設定します。
        /// </summary>
        public string NextText
        {
            get => NextTextField.GetValue(Src) as string;
            set => NextTextField.SetValue(Src, value);
        }

        private static FastField NextField;
        /// <summary>
        /// 次駅を取得・設定します。
        /// </summary>
        public Station Next
        {
            get => Station.FromSource(NextField.GetValue(Src));
            set => NextField.SetValue(Src, value?.Src);
        }

        private static FastField DoorsField;
        /// <summary>
        /// 自列車のドアのセットを取得・設定します。
        /// </summary>
        public DoorSet Doors
        {
            get => DoorSet.FromSource(DoorsField.GetValue(Src));
            set => DoorsField.SetValue(Src, value?.Src);
        }

        private static FastMethod UpdateMethod;
        /// <summary>
        /// 表示するテキストを更新します。
        /// </summary>
        public void Update()
            => UpdateMethod.Invoke(Src, null);

        private static FastMethod UpdateTextMethod;
        /// <summary>
        /// 表示するテキストに、<see cref="NextText"/> プロパティの値および次駅までの残距離を反映します。
        /// </summary>
        public void UpdateText()
            => UpdateTextMethod.Invoke(Src, null);

        private static FastMethod DrawMethod;
        /// <inheritdoc/>
        public override void Draw() => DrawMethod.Invoke(Src, null);

        private static FastMethod DisposeMethod;
        /// <inheritdoc/>
        public override void Dispose() => DisposeMethod.Invoke(Src, null);
    }
}
