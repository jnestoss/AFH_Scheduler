using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AFH_Scheduler.Data
{
    public class ScheduleModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private long _providerID;
        private string _providerName;
        private string _phone;
        private string _address;
        private string _recentInspection;
        private string _nextInspection;
        private string _food;

        public ScheduleModel(long id,
            string name,
            string phone,
            string address,
            string recentDate,
            string nextInspection,
            string food)
        {
            IsSelected = false;
            ProviderID = id;
            ProviderName = name;
            Phone = phone;
            Address = address;
            RecentInspection = recentDate;
            NextInspection = nextInspection;
            Food = food;
        }



        public bool IsSelected {
            get { return _isSelected; }
            set {
                ObservableProviders.ClearSelected();
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

        public string Food {
            get { return _food; }
            set {
                if (_food == value) return;
                _food = value;
                OnPropertyChanged("Food");
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
