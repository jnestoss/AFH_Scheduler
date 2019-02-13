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
        private string _providerID;
        private string _providerName;
        private bool _isProviderSelected;
        private readonly ProviderListVM _providerVM;

        public ProvidersModel(string id, string name)
        {
            IsProviderSelected = false;
            ProviderID = id;
            ProviderName = name;
        }
        public ProvidersModel(ProviderListVM vm, string id, string name)
        {
            _providerVM = vm;
            IsProviderSelected = false;
            ProviderID = id;
            ProviderName = name;
        }
        public string ProviderID
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
        public bool IsProviderSelected
        {
            get { return _isProviderSelected; }
            set
            {
                if (value == true) _providerVM.DeSelect(this);
                if (_isProviderSelected == value) return;
                _isProviderSelected = value;
                OnPropertyChanged("IsProviderSelected");
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
