using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class ProviderListVM : ObservableObject
    {
        private ObservableCollection<ProvidersModel> _providersList;
        public ObservableCollection<ProvidersModel> ProvidersList
        {
            get { return _providersList; }
            set
            {
                if (value != _providersList)
                {
                    _providersList = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

        private RelayCommand _providerListCommand;
        public ICommand ProviderListCommand
        {
            get
            {
                if (_providerListCommand == null)
                    _providerListCommand = new RelayCommand(AddProvider);
                return _providerListCommand;
            }
        }

        private void AddProvider(object obj)
        {
            
        }

        public ProviderListVM()
        {
            _providersList = new ObservableCollection<ProvidersModel>();

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                foreach (var item in provs)
                {
                    ProvidersList.Add(
                            new ProvidersModel
                            (
                                item.Provider_ID,
                                item.Provider_Name
                            )
                        );
                }
            }
        }
    }
}
