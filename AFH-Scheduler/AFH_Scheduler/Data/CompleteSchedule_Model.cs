using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class CompleteSchedule_Model : INotifyPropertyChanged
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        private bool _isSelected;
        private bool _followUpSelected;
        private long? _providerID;
        private string _providerName;
        private long _homeID;
        private string _address;
        private string _city;
        private string _zipcode;
        private string _inspectionDate;
        private string _followUpDate;
        
        private List<string> _deficiences;
        private string _selectedOutcome;

        public CompleteSchedule_Model(long? provider, string name, Provider_Homes house, string inspection, List<string> deficiences)
        {
            IsSelected = false;
            FollowUpSelected = false;
            ProviderID = provider;
            ProviderName = name;
            HomeID = house.PHome_ID;
            Address = house.PHome_Address;
            City = house.PHome_City;
            ZipCode = house.PHome_Zipcode;
            InspectionDate = inspection;
            FollowUpDate = "";
            Deficiences = deficiences;
            //Deficiences = new List<string>();
            //foreach (var outcom in deficiences)
            //{
            //    Deficiences.Add(outcom);
            //}
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        public bool FollowUpSelected
        {
            get { return _followUpSelected; }
            set
            {
                if (_followUpSelected == value) return;
                _followUpSelected = value;
                if (_followUpSelected)
                {
                    FollowUpDate = SchedulingAlgorithm.SettingFollowUps(InspectionDate);
                }
                else
                {
                    FollowUpDate = "";
                }
                OnPropertyChanged("FollowUpSelected");
            }
        }
        public long? ProviderID
        {
            get { return _providerID; }
            set
            {
                if (_providerID == value) return;
                _providerID = value;
                OnPropertyChanged("ProviderID");
            }
        }
        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                if (_providerName == value) return;
                _providerName = value;
                OnPropertyChanged("ProviderName");
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

        public string ZipCode
        {
            get { return _zipcode; }
            set
            {
                if (_zipcode == value) return;
                _zipcode = value;
                OnPropertyChanged("ZipCode");
            }
        }
        public string InspectionDate
        {
            get { return _inspectionDate; }
            set
            {
                if (_inspectionDate == value) return;
                _inspectionDate = value;
                OnPropertyChanged("InspectionDate");
            }
        }
        public string FollowUpDate
        {
            get { return _followUpDate; }
            set
            {
                if (_followUpDate == value) return;
                _followUpDate = value;
                OnPropertyChanged("FollowUpDate");
            }
        }
        public List<string> Deficiences
        {
            get { return _deficiences; }
            set
            {
                if (_deficiences == value) return;
                _deficiences = value;
                OnPropertyChanged("Deficiences");
            }
        }
        public string SelectedOutcome
        {
            get { return _selectedOutcome; }
            set
            {
                if (_selectedOutcome == value) return;
                _selectedOutcome = value;
                OnPropertyChanged("SelectedOutcome");
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
