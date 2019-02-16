using AFH_Scheduler.Data;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class EditLicensedHomeVM : ObservableObject, IPageViewModel
    {
        private LicensedHomeModel _licensedHomes;
        public LicensedHomeModel LicensedHomes
        {
            get { return _licensedHomes; }
            set
            {
                _licensedHomes = value;
                OnPropertyChanged("LicensedHomes");
            }
        }

        public EditLicensedHomeVM(string licenseNumber, string name)
        {
            LicensedHomes = new LicensedHomeModel(licenseNumber, name, "", "", "", "");
        }

        public string Name
        {
            get
            {
                return "Edit Licensed Home";
            }
        }
    }
}
