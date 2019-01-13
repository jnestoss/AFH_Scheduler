using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AFH_Scheduler.Helper_Classes;

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
    }
}
