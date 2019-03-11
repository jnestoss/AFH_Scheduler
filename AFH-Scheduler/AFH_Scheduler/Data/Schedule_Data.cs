using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Database;

namespace AFH_Scheduler.Data
{
    public class ScheduleModel : INotifyPropertyChanged
    {
        private ObservableCollection<HistoryDetailModel> _homeshistory;
        private bool _isSelected;
        private bool _hasNoProvider;
        private bool _isActive;
        private long _providerID;
        private long _homeID;
        private string _providerName;
        private long _homeLicense;
        private string _homeName;
        private string _phone;
        private string _address;
        private string _city;
        private string _ZIP;
        private string _recentInspection;
        private string _nextInspection;
        private string _eighteenthMonthDate;
        private string _rcsRegionUnit;

        public ScheduleModel(long providerID,
            long homeID,
            string name,
            long licenseNum,
            string homeName,
            string phone,
            string address,
            string homeCity,
            string homeZIP,
            string recentDate,
            string nextInspection,
            string eighteenthMonthDate,
            bool active,
            string regionUnit
            )
        {
            if (name.Equals("No Provider"))
            {
                HasNoProvider = true;
            }
            else
            {
                HasNoProvider = false;
            }
            ProviderID = providerID;
            HomeID = homeID;
            ProviderName = name;
            HomeLicenseNum = licenseNum;
            HomeName = homeName;
            Phone = phone;
            Address = address;
            City = homeCity;
            ZIP = homeZIP;
            RecentInspection = recentDate;
            NextInspection = nextInspection;
            EighteenthMonthDate = eighteenthMonthDate;
            IsActive = active;
            RcsRegionUnit = regionUnit;
            HomesHistory = new ObservableCollection<HistoryDetailModel>();
        }

        public ScheduleModel()
        {
            HomeID = GenerateHomeID();
            ProviderName = "";
            HomeName = "";
            Phone = "";
            Address = "";
            City = "";
            ZIP = "";
            RecentInspection = "";
            NextInspection = "";
            EighteenthMonthDate = "";
            IsActive = true;
            RcsRegionUnit = "";
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

        public ObservableCollection<HistoryDetailModel> HomesHistory
        {
            get { return _homeshistory; }
            set
            {
                if (value != _homeshistory)
                {
                    _homeshistory = value;
                    OnPropertyChanged("HomesHistory");
                }
            }
        }

        public bool HasNoProvider
        {
            get { return _hasNoProvider; }
            set
            {
                if (_hasNoProvider == value) return;
                _hasNoProvider = value;
                OnPropertyChanged("HasNoProvider");
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public long ProviderID
        {
            get { return _providerID; }
            set
            {
                if (_providerID == value) return;
                _providerID = value;
                OnPropertyChanged("ProviderID");
            }
        }

        public long HomeID
        {
            get { return _homeID; }
            set
            {
                if (_homeID == value) return;
                _homeID = value;
                OnPropertyChanged("HomeID");
            }
        }

        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                if (_providerName == value) return;
                _providerName = value;

                if (_providerName.Equals("No Provider"))
                {
                    HasNoProvider = true;
                }
                OnPropertyChanged("ProviderName");
            }
        }

        public long HomeLicenseNum
        {
            get { return _homeLicense; }
            set
            {
                if (_homeLicense == value) return;
                _homeLicense = value;
                OnPropertyChanged("HomeLicenseNum");
            }
        }

        public string HomeName
        {
            get { return _homeName; }
            set
            {
                if (_homeName == value) return;
                _homeName = value;
                OnPropertyChanged("HomeName");
            }
        }


        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone == value) return;
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) return;
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (_city == value) return;
                _city = value;
                OnPropertyChanged("City");
            }
        }

        public string ZIP
        {
            get { return _ZIP; }
            set
            {
                if (_ZIP == value) return;
                _ZIP = value;
                OnPropertyChanged("ZIP");
            }
        }

        public string RecentInspection
        {
            get { return _recentInspection; }
            set
            {
                if (_recentInspection == value) return;
                _recentInspection = value;
                OnPropertyChanged("RecentInspection");
            }
        }

        public string NextInspection
        {
            get { return _nextInspection; }
            set
            {
                if (_nextInspection == value) return;
                _nextInspection = value;
                OnPropertyChanged("NextInspection");
            }
        }

        public void SetSelectedToFalse()
        {
            _isSelected = false;
        }
        public string EighteenthMonthDate
        {
            get { return _eighteenthMonthDate; }
            set
            {
                if (_eighteenthMonthDate == value) return;
                _eighteenthMonthDate = value;
                OnPropertyChanged("EighteenthMonthDate");
            }
        }
        public string RcsRegionUnit
        {
            get { return _rcsRegionUnit; }
            set
            {
                if (_rcsRegionUnit == value) return;
                _rcsRegionUnit = value;
                OnPropertyChanged("RcsRegionUnit");
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