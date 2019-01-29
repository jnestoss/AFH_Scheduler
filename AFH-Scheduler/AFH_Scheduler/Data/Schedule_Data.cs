using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AFH_Scheduler.Schedules;

namespace AFH_Scheduler.Data
{
    public class ScheduleModel : INotifyPropertyChanged
    {
        public bool _isSelected;
        private long _providerID;
        private long _homeID;
        private string _providerName;
        private string _phone;
        private string _address;
        private string _city;
        private string _ZIP;
        private string _recentInspection;
        private string _nextInspection;
        private readonly SchedulerVM _schedulerVM;

        public ScheduleModel(long providerID,
            long homeID,
            string name,
            string phone,
            string address,
            string homeCity,
            string homeZIP,
            string recentDate,
            string nextInspection,
            SchedulerVM schedulerVM)
        {
            _schedulerVM = schedulerVM;
            IsSelected = false;
            ProviderID = providerID;
            HomeID = homeID;
            ProviderName = name;
            Phone = phone;
            Address = address;
            RecentInspection = recentDate;
            NextInspection = nextInspection;
        }



        public bool IsSelected {
            get { return _isSelected; }
            set {
                //if (value == true) _schedulerVM.ClearSelected();
                if (value == true) _schedulerVM.ClearSelected2(this);
                else _isSelected = value;
                OnPropertyChanged("IsSelected");
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
