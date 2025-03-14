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
    /// BVE 標準機能の時刻表を表します。
    /// </summary>
    public class TimeTable : AssistantText
    {
        [InitializeClassWrapper]
        private static void Initialize(BveTypeSet bveTypes)
        {
            ClassMemberSet members = bveTypes.GetClassInfoOf<TimeTable>();

            ModelField = members.GetSourceFieldOf(nameof(Model));
            NameTextsField = members.GetSourceFieldOf(nameof(NameTexts));
            ArrivalTimeTextsField = members.GetSourceFieldOf(nameof(ArrivalTimeTexts));
            DepartureTimeTextsField = members.GetSourceFieldOf(nameof(DepartureTimeTexts));
            NameTextWidthsField = members.GetSourceFieldOf(nameof(NameTextWidths));
            ArrivalTimeTextWidthsField = members.GetSourceFieldOf(nameof(ArrivalTimeTextWidths));
            DepartureTimeTextWidthsField = members.GetSourceFieldOf(nameof(DepartureTimeTextWidths));

            UpdateMethod = members.GetSourceMethodOf(nameof(Update));
        }

        /// <summary>
        /// オリジナル オブジェクトから <see cref="TimeTable"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        protected TimeTable(object src) : base(src)
        {
        }

        /// <summary>
        /// オリジナル オブジェクトからラッパーのインスタンスを生成します。
        /// </summary>
        /// <param name="src">ラップするオリジナル オブジェクト。</param>
        /// <returns>オリジナル オブジェクトをラップした <see cref="TimeTable"/> クラスのインスタンス。</returns>
        [CreateClassWrapperFromSource]
        public static new TimeTable FromSource(object src) => src is null ? null : new TimeTable(src);

        private static FastField ModelField;
        /// <summary>
        /// 時刻表を表示するための 2D モデルを表す <see cref="ClassWrappers.Model"/> を取得・設定します。
        /// </summary>
        public Model Model
        {
            get => Model.FromSource(ModelField.GetValue(Src));
            set => ModelField.SetValue(Src, value?.Src);
        }

        private static FastField NameTextsField;
        /// <summary>
        /// 表示する停車場名の配列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("停車場名") も含まれます。
        /// </remarks>
        /// <seealso cref="NameTextWidths"/>
        public string[] NameTexts
        {
            get => NameTextsField.GetValue(Src) as string[];
            set => NameTextsField.SetValue(Src, value);
        }

        private static FastField ArrivalTimeTextsField;
        /// <summary>
        /// 表示する到着時刻の配列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("着") も含まれます。
        /// </remarks>
        /// <seealso cref="ArrivalTimeTextWidths"/>
        public string[] ArrivalTimeTexts
        {
            get => ArrivalTimeTextsField.GetValue(Src) as string[];
            set => ArrivalTimeTextsField.SetValue(Src, value);
        }

        private static FastField DepartureTimeTextsField;
        /// <summary>
        /// 表示する発車時刻または通過時刻の配列を取得・設定します。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("発(通)") も含まれます。
        /// </remarks>
        /// <seealso cref="DepartureTimeTextWidths"/>
        public string[] DepartureTimeTexts
        {
            get => DepartureTimeTextsField.GetValue(Src) as string[];
            set => DepartureTimeTextsField.SetValue(Src, value);
        }

        private static FastField NameTextWidthsField;
        /// <summary>
        /// 停車場名の表示幅の配列を取得・設定します。ここで設定した数値を基に、列全体の幅が決定されます。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("停車場名") も含まれます。
        /// </remarks>
        /// <seealso cref="NameTexts"/>
        public int[] NameTextWidths
        {
            get => NameTextWidthsField.GetValue(Src) as int[];
            set => NameTextWidthsField.SetValue(Src, value);
        }

        private static FastField ArrivalTimeTextWidthsField;
        /// <summary>
        /// 到着時刻の表示幅の配列を取得・設定します。ここで設定した数値を基に、列全体の幅が決定されます。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("着") も含まれます。
        /// </remarks>
        /// <seealso cref="ArrivalTimeTexts"/>
        public int[] ArrivalTimeTextWidths
        {
            get => ArrivalTimeTextWidthsField.GetValue(Src) as int[];
            set => ArrivalTimeTextWidthsField.SetValue(Src, value);
        }

        private static FastField DepartureTimeTextWidthsField;
        /// <summary>
        /// 発車時刻または通過時刻の表示幅の配列を取得・設定します。ここで設定した数値を基に、列全体の幅が決定されます。
        /// </summary>
        /// <remarks>
        /// 初めの見出し行 ("発(通)") も含まれます。
        /// </remarks>
        /// <seealso cref="DepartureTimeTexts"/>
        public int[] DepartureTimeTextWidths
        {
            get => DepartureTimeTextWidthsField.GetValue(Src) as int[];
            set => DepartureTimeTextWidthsField.SetValue(Src, value);
        }

        private static FastMethod UpdateMethod;
        /// <summary>
        /// 時刻表の表示を最新の状態に更新します。
        /// </summary>
        public void Update()
        {
            UpdateMethod.Invoke(Src, null);
        }
    }
}
