using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BveEx.Samples.MapPlugins.StationController
{
    public partial class ControllerForm : Form
    {
        protected Label NameKey;
        protected TextBox NameValue;

        protected Label LocationKey;
        protected TextBox LocationValue;

        protected Label ArrivalTimeKey;
        protected TextBox ArrivalTimeValue;

        protected CheckBox Pass;

        protected Label DepartureTimeKey;
        protected TextBox DepartureTimeValue;

        protected CheckBox IsTerminal;

        protected Button AddButton;
        protected Button RemoveButton;

        protected void InitializeComponent()
        {
            MaximizeBox = false;
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(480, 112);
            Font = new Font("Yu Gothic UI", 9);
            Text = "BveEX マッププラグイン 停車場リスト編集サンプル";


            NameKey = new Label()
            {
                Left = 16,
                Top = 16,
                Width = 32,
                Text = "駅名",
            };
            Controls.Add(NameKey);

            NameValue = new TextBox()
            {
                Name = nameof(NameValue),
                Left = 48,
                Top = 16 - 2,
                Width = 64,
                Text = "東京",
            };
            Controls.Add(NameValue);


            LocationKey = new Label()
            {
                Left = 16,
                Top = 48,
                Width = 32,
                Text = "位置",
            };
            Controls.Add(LocationKey);

            LocationValue = new TextBox()
            {
                Name = nameof(LocationValue),
                Left = 48,
                Top = 48 - 2,
                Width = 64,
            };
            Controls.Add(LocationValue);


            ArrivalTimeKey = new Label()
            {
                Left = 144,
                Top = 16,
                Width = 32,
                Text = "到着",
            };
            Controls.Add(ArrivalTimeKey);

            ArrivalTimeValue = new TextBox()
            {
                Name = nameof(ArrivalTimeValue),
                Left = 176,
                Top = 16 - 2,
                Width = 64,
            };
            Controls.Add(ArrivalTimeValue);


            Pass = new CheckBox()
            {
                Name = nameof(Pass),
                Left = 176,
                Top = 40,
                Width = 64,
                Text = "通過",
            };
            Controls.Add(Pass);


            DepartureTimeKey = new Label()
            {
                Left = 272,
                Top = 16,
                Width = 32,
                Text = "出発",
            };
            Controls.Add(DepartureTimeKey);

            DepartureTimeValue = new TextBox()
            {
                Name = nameof(DepartureTimeValue),
                Left = 304,
                Top = 16 - 2,
                Width = 64,
            };
            Controls.Add(DepartureTimeValue);


            IsTerminal = new CheckBox()
            {
                Name = nameof(IsTerminal),
                Left = 304,
                Top = 40,
                Width = 64,
                Text = "終点",
            };
            Controls.Add(IsTerminal);


            AddButton = new Button()
            {
                Left = 384,
                Top = 16,
                Width = 80,
                Text = "追加",
            };
            AddButton.Click += AddButtonClicked;
            Controls.Add(AddButton);

            RemoveButton = new Button()
            {
                Left = 384,
                Top = 80,
                Width = 80,
                Text = "最後を削除",
            };
            RemoveButton.Click += RemoveButtonClicked;
            Controls.Add(RemoveButton);
        }
    }
}
