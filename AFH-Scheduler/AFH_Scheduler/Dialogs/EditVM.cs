using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;

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

        private List<Tuple<int,string>> _providerIDs;
        public List<Tuple<int, string>> ProviderIDs {
            get { return _providerIDs; }
            set {
                if (_providerIDs == value) return;
                _providerIDs = value;
                OnPropertyChanged("ProviderIDs");
            }
        }

        private Tuple<int, string> _curProvider;
        public Tuple<int, string> CurrentProvider {
            get { return _curProvider; }
            set {
                if (_curProvider == value) return;
                _curProvider = value;
                OnPropertyChanged("CurrentProvider");
            }
        }

        public EditVM(ScheduleModel scheduleData)
        {
            SelectedSchedule = scheduleData;
            CurrentProvider = new Tuple<int, string>((int) SelectedSchedule.ProviderID, SelectedSchedule.ProviderName);
            ProviderIDs = new List<Tuple<int,string>>();
            grabProviderInformation();
        }

        private void grabProviderInformation()
        {
            using(HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();

                foreach(Provider prov in provs)
                {
                    ProviderIDs.Add(new Tuple<int, string>((int) prov.Provider_ID, prov.Provider_Name));
                    Console.WriteLine(prov.Provider_ID);
                }
            }
        }
    }
}
