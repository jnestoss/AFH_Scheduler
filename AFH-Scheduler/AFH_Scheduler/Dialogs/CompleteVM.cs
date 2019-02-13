﻿using AFH_Scheduler.Helper_Classes;
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
using AFH_Scheduler.HelperClasses;

namespace AFH_Scheduler.Dialogs
{
    public class CompleteVM : ObservableObject, IPageViewModel       //, ICloseable
    {
        public string Name => "Complete Inspection Dialog";

        public ScheduleModel _selectedSchedule;
        public ScheduleModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
                OnPropertyChanged("SelectedSchedule");
            }
        }

        public static long _homeIDSave;

        public CompleteVM(ScheduleModel scheduleData)
        {
            SelectedSchedule = scheduleData;
        }
    }
}
