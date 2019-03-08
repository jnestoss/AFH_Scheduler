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
        private string _homeName;
        private string _inspectiondate;
        private string _inspectionoutcome;

        public HistoryDetailModel(string name, string inspectiondate, string inspectionoutcome)
        {
            IsSelected = false;
            HomeName = name;
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

        public string HomeName
        {
            get { return _homeName; }
            set
            {
                if (_homeName == value) return;
                _homeName = value;
                OnPropertyChanged("ProviderID");
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