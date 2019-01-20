using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AFH_Scheduler.Schedules
{
    public interface OpenMessageDialogService
    {
        void ReleaseMessageBox(string message);
    }
    class SchedulesOpenDialog : OpenMessageDialogService
    {
        public void ReleaseMessageBox(string message)
        {
            MessageBox.Show(message);
        }
    }
}
