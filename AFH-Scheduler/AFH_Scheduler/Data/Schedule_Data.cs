using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Helper_Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace AFH_Scheduler.Data
{
    public class ScheduleModel : ObservableObject, INotifyPropertyChanged
    {
        private ObservableCollection<HistoryDetailModel> _homeshistory;
        private bool _isSelected;
        private long _providerID;
        private long _homeID;
        private string _providerName;
        private string _phone;
        private string _address;
        private string _recentInspection;
        private string _nextInspection;
        private string _eighteenthMonthDate;

        public ScheduleModel(long id,
            string name,
            long homeID,
            string phone,
            string address,
            string recentDate,
            string nextInspection,
            string eighteenthMonthDate
            )
        {
            IsSelected = false;
            ProviderID = id;
            ProviderName = name;
            HomeID = homeID;
            Phone = phone;
            Address = address;
            RecentInspection = recentDate;
            NextInspection = nextInspection;
            EighteenthMonthDate = eighteenthMonthDate;
            HomesHistory = new ObservableCollection<HistoryDetailModel>();
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

        public bool IsSelected {
            get { return _isSelected; }
            set {
                if (_isSelected == value) return;
                _isSelected = value;
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

        public string EighteenthMonthDate
        {
            get { return _eighteenthMonthDate; }
            set {
                if (_eighteenthMonthDate == value) return;
                _eighteenthMonthDate = value;
                OnPropertyChanged("EighteenthMonthDate");
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
