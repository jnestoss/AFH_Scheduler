using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class NewHomeDialogVM : ObservableObject, IPageViewModel
    {
        private List<ProvidersModel> _providers;
        public List<ProvidersModel> Providers
        {
            get { return _providers; }
            set
            {
                if (_providers == value) return;
                _providers = value;
                OnPropertyChanged("Providers");
            }
        }

        private ScheduleModel _newHomeCreated;
        public ScheduleModel NewHomeCreated
        {
            get { return _newHomeCreated; }
            set
            {
                _newHomeCreated = value;
                OnPropertyChanged("NewHomeCreated");
            }
        }

        private DateTime _datePicked;
        public DateTime DatePicked
        {
            get { return _datePicked; }
            set
            {
                _datePicked = value;
                NewHomeCreated.NextInspection = _datePicked.ToShortDateString();
                OnPropertyChanged("DatePicked");
            }
        }
        private bool _isProviderSelected;
        public bool IsProviderSelected
        {
            get { return _isProviderSelected; }
            set
            {
                _isProviderSelected = value;
                OnPropertyChanged("IsProviderSelected");
            }
        }

        private ProvidersModel _selectedProviderName;
        public ProvidersModel SelectedProviderName
        {
            get { return _selectedProviderName; }
            set
            {
                if (_selectedProviderName == value) return;
                _selectedProviderName = value;
                IsProviderSelected = true;
                OnPropertyChanged("SelectedProviderName");
            }
        }

        public NewHomeDialogVM()
        {
            IsProviderSelected = false;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                Providers = new List<ProvidersModel>();
                Providers.Add(new ProvidersModel("-1", "No Provider"));
                string listItem = "";
                foreach (var item in provs)
                {
                    listItem = item.Provider_ID + "-" + item.Provider_Name;
                    Providers.Add(new ProvidersModel(item.Provider_ID.ToString(), item.Provider_Name));
                }
                NewHomeCreated = new ScheduleModel();
            }
        }

        public string Name
        {
            get
            {
                return "Create New Home";
            }
        }
    }
}
