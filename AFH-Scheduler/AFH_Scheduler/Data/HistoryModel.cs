﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class HistoryModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private long _providerID;
        private string _providerName;
        private long _homeID;
        private string _address;
        private string _zipcode;
        private string _recentInspection;

        public HistoryModel(long proID, long phomeID,string providerName,string address,string zipcode)
        {
            IsSelected = false;
            ProviderID = proID;
            HomeID = phomeID;
            Address = address;
            ProviderName = providerName;
            ZipCode = zipcode;
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
                
    }
}
