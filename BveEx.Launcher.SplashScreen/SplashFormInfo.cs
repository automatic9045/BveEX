using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Launcher.SplashScreen
{
    public class SplashFormInfo : MarshalByRefObject
    {
        private readonly SplashForm TargetForm;

        private DateTime LastUpdated = DateTime.Now;

        public string ProgressText
        {
            get => TargetForm.ProgressText;
            set
            {
                TargetForm.ProgressText = value;
                LastUpdated = DateTime.Now;
            }
        }

        internal SplashFormInfo(SplashForm targetForm)
        {
            TargetForm = targetForm;

            Task.Run(async () =>
            {
                while (true)
                {
                    if (LastUpdated + TimeSpan.FromSeconds(6) <= DateTime.Now) TargetForm.ProgressText = "起動に通常よりも時間がかかっています...";
                    if (LastUpdated + TimeSpan.FromSeconds(15) <= DateTime.Now) Environment.Exit(0);
                    await Task.Delay(1000);
                }
            });
        }
    }
}
