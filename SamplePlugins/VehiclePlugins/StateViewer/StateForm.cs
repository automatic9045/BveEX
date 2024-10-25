using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

namespace BveEx.Samples.VehiclePlugins.StateViewer
{
    public partial class StateForm : Form
    {
        public StateForm()
        {
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Scenario scenario = InstanceStore.Instance.BveHacker.Scenario;

            if (e.KeyCode == Keys.Enter)
            {
                if (sender is TextBox textBox)
                {
                    switch (textBox.Name)
                    {
                        case nameof(TimeValue):
                            TimeSpan time;
                            if (TimeSpan.TryParse(textBox.Text, out time))
                            {
                                scenario.TimeManager.SetTime((int)time.TotalMilliseconds);
                            }
                            break;
                            
                        case nameof(LocationValue):
                            double location;
                            if (double.TryParse(textBox.Text, out location))
                            {
                                scenario.LocationManager.SetLocation(location, true);
                            }
                            break;

                        case nameof(SpeedValue):
                            double speed;
                            if (double.TryParse(textBox.Text, out speed))
                            {
                                scenario.LocationManager.SetSpeed(speed / 3.6);
                            }
                            break;
                    }
                }
            }
        }

        public void Tick()
        {
            Scenario scenario = InstanceStore.Instance.BveHacker.Scenario;

            if (!TimeValue.Focused) TimeValue.Text = TimeSpan.FromMilliseconds(scenario.TimeManager.TimeMilliseconds).ToString(@"hh\:mm\:ss");
            if (!LocationValue.Focused) LocationValue.Text = scenario.LocationManager.Location.ToString("F");
            if (!SpeedValue.Focused) SpeedValue.Text = (scenario.LocationManager.SpeedMeterPerSecond * 3.6).ToString("F");
        }
    }
}
