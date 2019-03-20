using AFH_Scheduler.Dialogs.SettingSubWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class ProvidersModel : INotifyPropertyChanged
    {
        private long _providerID;
        private string _providerName;

        public ProvidersModel(long providerId, string name)
        {
            ProviderName = name;
            ProviderID = providerId;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
