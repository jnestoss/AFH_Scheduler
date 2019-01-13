using AFH_Scheduler.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class CompletedHomes
    {
        long? _providerID;
        long _homeID;
        string _address;
        string _city;
        string _zipcode;
        //string _phoneNum;
        string _inspectionDate;
        public CompletedHomes(Provider_Homes house, string inspection)
        {
            ProviderID = house.FK_Provider_ID;
            HomeID = house.PHome_ID;
            Address = house.PHome_Address;
            City = house.PHome_City;
            ZipCode = house.PHome_Zipcode;
            //PhoneNumber = house.PHome_Phonenumber;
            InspectionDate = inspection;
        }

        public long? ProviderID
        {
            get { return _providerID; }
            set
            {
                if (_providerID == value) return;
                _providerID = value;
            }
        }

        public long HomeID
        {
            get { return _homeID; }
            set
            {
                if (_homeID == value) return;
                _homeID = value;
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value) return;
                _address = value;
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (_city == value) return;
                _city = value;
            }
        }

        public string ZipCode
        {
            get { return _zipcode; }
            set
            {
                if (_zipcode == value) return;
                _zipcode = value;
            }
        }

        /*public string PhoneNumber
        {
            get { return _phoneNum; }
            set
            {
                if (_phoneNum == value) return;
                _phoneNum = value;
            }
        }*/

        public string InspectionDate
        {
            get { return _inspectionDate; }
            set
            {
                if (_inspectionDate == value) return;
                _inspectionDate = value;
            }
        }
    }
}
