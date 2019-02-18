using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class LicensedHomeModel : INotifyPropertyChanged
    {
        private string _homeLicense;
        private string _homeName;
        private string _address;
        private string _city;
        private string _zip;
        private string _telephone;

        public LicensedHomeModel(string licenseNumber, string name, string address, string city, string zipcode, string phone)
        {
            HomeLicense = licenseNumber;
            HomeName = name;
            Address = address;
            City = city;
            Zipcode = zipcode;
        }

        public string HomeLicense
        {
            get { return _homeLicense; }
            set
            {
                _homeLicense = value;
                OnPropertyChanged("HomeLicense");
            }
        }

        public string HomeName
        {
            get { return _homeName; }
            set
            {
                _homeName = value;
                OnPropertyChanged("HomeName");
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

        public string Telephone
        {
            get { return _telephone; }
            set
            {
                _telephone = value;
                OnPropertyChanged("Telephone");
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
