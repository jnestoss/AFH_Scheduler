using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Dialogs.Errors;
using AFH_Scheduler.Dialogs.Confirmation;
using MaterialDesignThemes.Wpf;

namespace AFH_Scheduler.Dialogs
{
    public class CompleteVM : ObservableObject, IPageViewModel       //, ICloseable
    {
        public string Name => "Complete Inspection Dialog";

        public HomeModel _selectedSchedule;
        public HomeModel SelectedHome {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
                OnPropertyChanged("SelectedSchedule");
            }
        }

        private string _previousInspection;
        public string PreviousInspection {
            get => _previousInspection;
            set {
                if (_previousInspection == value) return;
                _previousInspection = value;
            }
        }

        public static long _homeIDSave;

        public CompleteVM(HomeModel scheduleData)
        {
            SelectedHome = scheduleData;
            PreviousInspection = SelectedHome.NextInspection;
        }
    }
}
