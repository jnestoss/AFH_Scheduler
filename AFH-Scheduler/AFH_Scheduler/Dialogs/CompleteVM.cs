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
using AFH_Scheduler.Algorithm;

namespace AFH_Scheduler.Dialogs
{
    public class CompleteVM : ObservableObject, IPageViewModel       //, ICloseable
    {
        public string Name => "Complete Inspection Dialog";

        public HomeModel _selectedHome;
        public HomeModel SelectedHome {
            get { return _selectedHome; }
            set {
                if (_selectedHome == value) return;
                _selectedHome = value;
                OnPropertyChanged("SelectedHome");
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

        private List<String> _outcomeCodes;
        public List<String> OutcomeCodes {
            get {
                return _outcomeCodes;
            }
            set {
                if (!(_outcomeCodes == value)) _outcomeCodes = value;
            }
        }

        private Inspection_Outcome _selectedCode;
        public Inspection_Outcome SelectedCode {
            get { return _selectedCode; }
            set {
                if (_selectedCode == value) return;
                _selectedCode = value;
            }
        }

        private RelayCommand _calcDate;
        public RelayCommand CalcDate {
            get {
                if (_calcDate == null) _calcDate = new RelayCommand(CalcNextInspectionDate);
                return _calcDate;
            }
        }

        public static long _homeIDSave;

        public CompleteVM(HomeModel scheduleData)
        {
            SelectedHome = scheduleData;
            PreviousInspection = SelectedHome.NextInspection;
            GrabOutcomeCodes();
            SelectedCode = GetMostRecentOutcome();
        }

        private Inspection_Outcome GetMostRecentOutcome()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                List<Home_History> history = db.Home_History.ToList();
                //searches history database for the most recent inspection outcome
                Home_History mostRecentInspectionOutcome = history.Where(x => x.FK_PHome_ID == SelectedHome.HomeID).FirstOrDefault();

                List<Inspection_Outcome> outcomes = db.Inspection_Outcome.ToList();

                if (mostRecentInspectionOutcome == null)
                {
                    //initiate next inspection to most recent inspection outcome
                    try
                    {
                        return outcomes.Where(x => x.IOutcome_Code == mostRecentInspectionOutcome.Inspection_Outcome.IOutcome_Code).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        return outcomes[0];
                    }
                }
                else
                {
                    //Grab first outcome from database
                    return outcomes.FirstOrDefault();
                }
            }
        }

        private void CalcNextInspectionDate(object o)
        {
            string date = SchedulingAlgorithm.NextScheduledDate(SelectedCode, SelectedHome.NextInspection);
            SelectedHome.NextInspection = date;
        }

        private void GrabOutcomeCodes()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                List<Inspection_Outcome> outcomes = db.Inspection_Outcome.ToList();
                OutcomeCodes = outcomes.Select(x => x.IOutcome_Code).ToList();
            }
        }
    }
}
