﻿using System;
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
        private List<string> _providers;
        private string _selectedProviderName;
        private string _homeID;
        private string _address;
        private string _city;
        private string _zip;
        private DateTime _inspectionDate;

        public NewHomeModel(List<string> provs)
        {
            Providers = new List<string>();
            foreach (var item in provs)
            {
                Providers.Add(item);
            }
            InspectionDate = DateTime.Now;
        }

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
        public List<string> Providers
        {
            get { return _providers; }
            set
            {
                if (_providers == value) return;
                _providers = value;
                OnPropertyChanged("Providers");
            }
        }
        public string SelectedProviderName
        {
            get { return _selectedProviderName; }
            set
            {
                if (_selectedProviderName == value) return;
                _selectedProviderName = value;
                String[] idAndName = _selectedProviderName.Split('-');
                ProviderID = idAndName[0];
                ProviderName = idAndName[1];
                OnPropertyChanged("SelectedProviderName");
            }
        }

        public string HomeID
        {
            get { return _homeID; }
            set
            {
                _homeID = value;
                OnPropertyChanged("HomeID");
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}