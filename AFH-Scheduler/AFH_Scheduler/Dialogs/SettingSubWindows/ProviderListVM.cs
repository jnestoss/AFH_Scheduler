using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
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

        private bool _dialogBool;
        public bool DialogBoolReturn
        {
            get { return _dialogBool; }
            set
            {
                _dialogBool = value;
                OnPropertyChanged("DialogBoolReturn");
            }
        }

        private ProvidersModel _selectedProvider;
        private ProvidersModel SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                if (_selectedProvider == value) return;
                _selectedProvider = value;
            }
        }

        public void DeSelect(ProvidersModel providers)
        {
            if (SelectedProvider != null)
                SelectedProvider.IsProviderSelected = false;
            SelectedProvider = providers;
        }

        private RelayCommand _providerDeleteCommand;
        public ICommand ProviderDeleteCommand
        {
            get
            {
                if (_providerDeleteCommand == null)
                    _providerDeleteCommand = new RelayCommand(DeleteProvider);
                return _providerDeleteCommand;
            }
        }
        private void DeleteProvider(object obj)
        {
            
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

        private async void AddProvider(object obj)
        {
            var vm = new NewProviderCreatorVM();
            var view = new NewProviderCreator(vm);
            var result = await DialogHost.Show(view, "AddProviderDialog", ClosingEventHandlerAddProviders);
            if (DialogBoolReturn)
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    db.Providers.Add(new Provider
                    {
                        Provider_ID = Convert.ToInt64(vm.NewProviderAdded.ProviderID),
                        Provider_Name = vm.NewProviderAdded.ProviderName
                    });
                    db.SaveChanges();

                }

                ProvidersList.Add(
                          new ProvidersModel
                          (
                              this,
                              vm.NewProviderAdded.ProviderID,
                              vm.NewProviderAdded.ProviderName
                          )
                      );
            }
        }

        private void ClosingEventHandlerAddProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogBoolReturn = false;
                return;
            }

            DialogBoolReturn = true;
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
                                this,
                                item.Provider_ID.ToString(),
                                item.Provider_Name
                            )
                        );
                }
            }
        }
    }
}
