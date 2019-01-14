using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class HistoryDetailModel: INotifyPropertyChanged
    {
        private bool _isSelected;
        private long _providerID;
        private long _homeID;
        private string _inspectiondate;
        private string _inspectionoutcome;

        public HistoryDetailModel(long proID, long phomeID, string inspectiondate, string inspectionoutcome)
        {
            IsSelected = false;
            ProviderID = proID;
            HomeID = phomeID;
            _inspectiondate = inspectiondate;
            _inspectionoutcome = inspectionoutcome;
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

        public string InspectionDate
        {
            get { return _inspectiondate; }
            set
            {
                if (_inspectiondate == value) return;
                _inspectiondate = value;
                OnPropertyChanged("InspectionDate");
            }
        }
        public string InspectionOutcome
        {
            get { return _inspectionoutcome; }
            set
            {
                if (_inspectionoutcome == value) return;
                _inspectionoutcome = value;
                OnPropertyChanged("InspectionOutcome");
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