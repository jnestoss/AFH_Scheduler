﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AFH_Scheduler.Dialogs;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;

namespace AFH_Scheduler
{
    public class Commands
    {
        private static bool DialogAlreadyOpen = false;

        public static readonly RelayCommand CloseCommand = new RelayCommand(o => ((Window)o).Close());

        public static readonly RelayCommand MinimizeCommand = new RelayCommand(w =>
        {
            var win = (Window)w;
            if (win.WindowState == WindowState.Normal)
            {
                win.WindowState = WindowState.Minimized;
            }
        });

        public static readonly RelayCommand OpenSettingsCommand = new RelayCommand(async w =>
        {
            
            if (!DialogAlreadyOpen)
            {
                DialogAlreadyOpen = true;

                var viewVM = (MainWindow)w;
                DataVM data = (DataVM)w;
                if (viewVM.Name.Equals("Schedules"))
                {
                    data = (DataVM)w;
                    //data.NormalCurve = settings.NormalCurve;
                }

                var settings = new SettingsVM(Convert.ToDouble(data.NormalCurve), data.DesiredAverage);
                var view = new SettingsDialog(settings);
                var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandlerSettings);

                //settings.NormalCurve

                DialogAlreadyOpen = false;
            }
        });

        public static void ClosingEventHandlerSettings(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }
        }
    }
}
