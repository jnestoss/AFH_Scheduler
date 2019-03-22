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

namespace AFH_Scheduler.Data
{
    public class HomeModel : INotifyPropertyChanged
    {
        private ObservableCollection<HistoryDetailModel> _homeshistory;
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
        private string _seventeenMonthDate;
        private string _forecastedDate;
        private string _rcsRegion;
        private string _rcsUnit;
        private string _rcsRegionUnit;
        private int _homeCount;

        public HomeModel()
        {
            HomesHistory = new ObservableCollection<HistoryDetailModel>();
        }

        public void HasProvider()
        {
            if (ProviderName.Equals("No Provider"))
            {
                HasNoProvider = true;
            }
            else
            {
                HasNoProvider = false;
            }
        }

        public ObservableCollection<HistoryDetailModel> HomesHistory {
            get { return _homeshistory; }
            set {
                if (value != _homeshistory)
                {
                    _homeshistory = value;
                    OnPropertyChanged("HomesHistory");
                }
            }
        }

        public bool HasNoProvider {
            get { return _hasNoProvider; }
            set {
                if (_hasNoProvider == value) return;
                _hasNoProvider = value;
                OnPropertyChanged("HasNoProvider");
            }
        }

        public bool IsActive {
            get { return _isActive; }
            set {
                if (_isActive == value) return;
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public long ProviderID {
            get { return _providerID; }
            set {
                if (_providerID == value) return;
                _providerID = value;
                OnPropertyChanged("ProviderID");
            }
        }

        public long HomeID {
            get { return _homeID; }
            set {
                if (_homeID == value) return;
                _homeID = value;
                OnPropertyChanged("HomeID");
            }
        }

        public string ProviderName {
            get { return _providerName; }
            set {
                if (_providerName == value) return;
                _providerName = value;
                OnPropertyChanged("ProviderName");
            }
        }

        public long HomeLicenseNum {
            get { return _homeLicense; }
            set {
                if (_homeLicense == value) return;
                _homeLicense = value;
                OnPropertyChanged("HomeLicenseNum");
            }
        }

        public string HomeName {
            get { return _homeName; }
            set {
                if (_homeName == value) return;
                _homeName = value;
                OnPropertyChanged("HomeName");
            }
        }


        public string Phone {
            get { return _phone; }
            set {
                if (_phone == value) return;
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Address {
            get { return _address; }
            set {
                if (_address == value) return;
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public string City {
            get { return _city; }
            set {
                if (_city == value) return;
                _city = value;
                OnPropertyChanged("City");
            }
        }

        public string ZIP {
            get { return _ZIP; }
            set {
                if (_ZIP == value) return;
                _ZIP = value;
                OnPropertyChanged("ZIP");
            }
        }

        public string RecentInspection {
            get { return _recentInspection; }
            set {
                if (_recentInspection == value) return;
                _recentInspection = value;
                OnPropertyChanged("RecentInspection");
            }
        }

        public string NextInspection {
            get { return _nextInspection; }
            set {
                if (_nextInspection == value) return;
                _nextInspection = value;
                OnPropertyChanged("NextInspection");
            }
        }

        public string EighteenthMonthDate {
            get { return _eighteenthMonthDate; }
            set {
                if (_eighteenthMonthDate == value) return;
                _eighteenthMonthDate = value;
                OnPropertyChanged("EighteenthMonthDate");
            }
        }

        public string SeventeenMonthDate {
            get { return _seventeenMonthDate;  }
            set {
                if (_seventeenMonthDate == value) return;
                _seventeenMonthDate = value;
                OnPropertyChanged("SeventeenMonthDate");
            }
        }

        public string ForecastedDate {
            get => _forecastedDate;
            set {
                if(_forecastedDate != value)
                {
                    _forecastedDate = value;
                    OnPropertyChanged("ForecastedDate");
                }
            }
        }

        public string RcsRegion {
            get => _rcsRegion;
            set {
                if (_rcsRegion == value) return;
                _rcsRegion = value;
                OnPropertyChanged("RcsRegion");
            }
        }

        public string RcsUnit {
            get { return _rcsUnit; }
            set {
                if (_rcsUnit == value) return;
                _rcsUnit = value;
                OnPropertyChanged("RcsUnit");
            }
        }

        public string RcsRegionUnit {
            get { return $"{RcsRegion}{RcsUnit}"; }
            set {
                if (_rcsRegionUnit == value) return;
                _rcsRegionUnit = value;
                _rcsRegion = value.Substring(0,1);
                _rcsUnit = value.Substring(1, 1);
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