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
using AFH_Scheduler.HelperClasses;

namespace AFH_Scheduler.Dialogs
{
    public class EditVM : ObservableObject, IPageViewModel       //, ICloseable
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

        private RelayCommand _deleteProviderCommand;
        public RelayCommand DeleteProviderCommand {
            get {
                if (_deleteProviderCommand == null)
                    _deleteProviderCommand = new RelayCommand(ShowProviderList);
                return _deleteProviderCommand;
            }
        }

        private async void ShowProviderList(object obj)
        {
            var view = new DeleteConfirmationDialog();
            var result = await DialogHost.Show(view, "DeleteConfirmationDialog", ClosingEventHandler);
            //ClosingEventHandler(this, new DialogClosingEventArgs());
            
        }

        public static long _homeIDSave;


        //public event EventHandler<EventArgs> RequestClose;      
        //public RelayCommand CloseCommand { get; private set; }

        public EditVM(ScheduleModel scheduleData)
        {
            SelectedSchedule = scheduleData;
            CurrentProvider = new Tuple<int, string>((int) SelectedSchedule.ProviderID, SelectedSchedule.ProviderName);
            ProviderIDs = new List<Tuple<int,string>>();
            grabProviderInformation();
            _homeIDSave = SelectedSchedule.HomeID;

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

        public void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if((String)eventArgs.Parameter == "YES")
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    //Console.WriteLine("********" + ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext).SelectedSchedule.HomeID);
                    var home = db.Provider_Homes.SingleOrDefault(r => r.PHome_ID == SelectedSchedule.HomeID);

                    if (home != null)
                    {
                        db.Provider_Homes.Remove(home);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
