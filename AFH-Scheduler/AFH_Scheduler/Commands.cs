using System;
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
            var settings = new SettingsVM();
            var view = new SettingsDialog(settings);
            var result = await DialogHost.Show(view, "WindowDialogs", SettingsClosingEventHandler);
        });

        //public static RelayCommand OpenSettingsCommand => new RelayCommand(ExecuteSettingsDialog);

        //private static async void ExecuteSettingsDialog(object o)
        //{
        //    if(App.Current.Windows.Count == 2)
        //    {
        //        var settings = new SettingsVM();
        //        var view = new SettingsDialog(settings);
        //        var result = await DialogHost.Show(view, "WindowDialogs", SettingsClosingEventHandler);
        //    }
        //}

        public static void SettingsClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }
        }
    }
}
