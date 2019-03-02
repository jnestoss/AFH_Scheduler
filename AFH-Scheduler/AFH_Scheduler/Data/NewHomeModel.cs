using AFH_Scheduler.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class NewHomeModel : INotifyPropertyChanged
    {
        private string _providerID;
        private string _providerName;
        private List<ProvidersModel> _providers;
        private ProvidersModel _selectedProviderName;
        private long _homeID;
        private string _homeLicense;
        private string _homeName;
        private string _homePhone;
        private string _address;
        private string _city;
        private string _zip;
        private DateTime _inspectionDate;
        private string _rcsRegion;
        private string _rcsUnit;

        private bool _isProviderSelected;

        public NewHomeModel(List<ProvidersModel> provs)
        {
            HomeID = GenerateHomeID();
            Address = "";
            City = "";
            Zipcode = "";
            IsProviderSelected = false;
            RcsRegion = "";
            RcsUnit = "";

             Providers = new List<ProvidersModel>();
            foreach (var item in provs)
            {
                Providers.Add(item);
            }
            InspectionDate = DateTime.Now;
        }

        /*public NewHomeModel(long pHome_ID, 
            string pHome_Address, 
            string pHome_City, 
            string pHome_Zipcode, 
            DateTime insp)
        {
            HomeID = pHome_ID.ToString();
            Address = pHome_Address;
            City = pHome_City;
            Zipcode = pHome_Zipcode;
            IsProviderSelected = false;
            InspectionDate = insp;
        }*/

        public string ProviderID
        {
            get { return _providerID; }
            set
            {
                _providerID = value;
                OnPropertyChanged("ProviderID");
            }
        }

        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                _providerName = value;
                OnPropertyChanged("ProviderName");
            }
        }
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

        public long HomeID
        {
            get { return _homeID; }
            set
            {
                _homeID = value;
                OnPropertyChanged("HomeID");
            }
        }

        public long GenerateHomeID()
        {
            long newID;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                try
                {
                    var recentHomeID = db.Provider_Homes.OrderByDescending(r => r.PHome_ID).FirstOrDefault();
                    if (recentHomeID.PHome_ID == Int64.MaxValue)
                    {
                        newID = 0;
                        while (true)
                        {
                            var isUniqueID = db.Provider_Homes.Where(r => r.PHome_ID == newID).ToList();
                            if (isUniqueID.Count == 0)
                            {
                                return newID;
                            }
                            newID++;
                        }
                    }
                    else
                        newID = recentHomeID.PHome_ID + 1;
                    return newID;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public string HomeLicenseNum
        {
            get { return _homeLicense; }
            set
            {
                _homeLicense = value;
                OnPropertyChanged("HomeLicenseNum");
            }
        }

        public string HomeLicensedName
        {
            get { return _homeName; }
            set
            {
                _homeName = value;
                OnPropertyChanged("HomeLicensedName");
            }
        }

        public string HomePhoneNumber
        {
            get { return _homePhone; }
            set
            {
                _homePhone = value;
                OnPropertyChanged("HomePhoneNumber");
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        public string Zipcode
        {
            get { return _zip; }
            set
            {
                _zip = value;
                OnPropertyChanged("Zipcode");
            }
        }

        public DateTime InspectionDate
        {
            get { return _inspectionDate; }
            set
            {
                _inspectionDate = value;
                OnPropertyChanged("InspectionDate");
            }
        }
        public string RcsRegion
        {
            get { return _rcsRegion; }
            set
            {
                if (_rcsRegion == value) return;
                _rcsRegion = value;
                OnPropertyChanged("RcsRegion");
            }
        }
        public string RcsUnit
        {
            get { return _rcsUnit; }
            set
            {
                if (_rcsUnit == value) return;
                _rcsUnit = value;
                OnPropertyChanged("RcsUnit");
            }
        }

        public bool IsProviderSelected
        {
            get { return _isProviderSelected; }
            set
            {
                _isProviderSelected = value;
                OnPropertyChanged("IsProviderSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
