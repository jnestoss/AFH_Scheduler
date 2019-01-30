using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;

namespace AFH_Scheduler.Dialogs
{
    public class EditVM : ObservableObject, IPageViewModel
    {
        public string Name => "Edit Page";

        public ScheduleModel _selectedSchedule;
        public ScheduleModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
                OnPropertyChanged("SelectedSchedule");
            }
        }

        public EditVM(ScheduleModel scheduleData)
        {
            SelectedSchedule = scheduleData;
        }
    }
}
