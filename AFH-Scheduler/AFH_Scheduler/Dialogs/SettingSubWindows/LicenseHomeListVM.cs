using AFH_Scheduler.Data;
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
    public class LicenseHomeListVM : ObservableObject
    {
        private ObservableCollection<LicensedHomeModel> _licensedHomesList;
        public ObservableCollection<LicensedHomeModel> LicensedHomesList
        {
            get { return _licensedHomesList; }
            set
            {
                if (value != _licensedHomesList)
                {
                    _licensedHomesList = value;
                    OnPropertyChanged("LicensedHomesList");
                }
            }
        }


        #region Edit Outcome
        private RelayCommand _licenseEditCommand;
        public ICommand LicenseEditCommand
        {
            get
            {
                if (_licenseEditCommand == null)
                    _licenseEditCommand = new RelayCommand(EditLicensedHome);
                return _licenseEditCommand;
            }
        }

        private /*async*/ void EditLicensedHome(object obj)
        {
            /*LicensedHomeModel model = (LicensedHomeModel)obj;
            var vm = new EditLicensedHomeVM(model.HomeLicense, model.HomeName);
            var view = new EditLicensedHome(vm);
            var result = await DialogHost.Show(view, "LicensedHomeDialog", ClosingEventHandlerOutcome);
            if (result.Equals("SUBMIT"))
            {
                model.HomeLicense = vm.LicensedHomes.HomeLicense;
                model.HomeName = vm.LicensedHomes.HomeName;
            }*/
        }
        #endregion



        public LicenseHomeListVM()
        {
            _licensedHomesList = new ObservableCollection<LicensedHomeModel>();

            LicensedHomesList.Add(new LicensedHomeModel("235456", "Stan Bed & Breakfast", "245 Southern Dr", "Washington", "12345", ""));
        }
    }
}
