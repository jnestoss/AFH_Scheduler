using AFH_Scheduler.Dialogs.SettingSubWindows;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class SettingsVM : ObservableObject
    {
        private string _normalCurveValue;
        public string NormalCurve
        {
            get { return _normalCurveValue; }
            set
            {
                if (_normalCurveValue == value) return;
                _normalCurveValue = value;
                OnPropertyChanged("NormalCurve");
            }
        }

        private bool _dialogSettingBool;
        public bool DialogSettingBoolReturn
        {
            get { return _dialogSettingBool; }
            set
            {
                _dialogSettingBool = value;
                OnPropertyChanged("DialogSettingBoolReturn");
            }
        }

        private RelayCommand _providerListCommand;
        public ICommand ProviderListCommand
        {
            get
            {
                if (_providerListCommand == null)
                    _providerListCommand = new RelayCommand(ShowProviderList);
                return _providerListCommand;
            }
        }

        private async void ShowProviderList(object obj)
        {
            var vm = new ProviderListVM();
            var view = new ProviderListDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
        }

        private RelayCommand _normalCurveCommand;
        public ICommand NormalCurveCommand
        {
            get
            {
                if (_normalCurveCommand == null)
                    _normalCurveCommand = new RelayCommand(AdjustNormalCurve);
                return _normalCurveCommand;
            }
        }

        private async void AdjustNormalCurve(object obj)
        {
            var vm = new NormalCurveAdjusterVM(NormalCurve);
            var view = new NormalCurveAdjuster(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
            if (DialogSettingBoolReturn)
            {
                NormalCurve = vm.CurveNumber;
            }
        }

        private RelayCommand _outcomeListCommand;
        public ICommand OutcomeListCommand
        {
            get
            {
                if (_outcomeListCommand == null)
                    _outcomeListCommand = new RelayCommand(ShowOutcomeCodes);
                return _outcomeListCommand;
            }
        }
        private async void ShowOutcomeCodes(object obj)
        {
            var vm = new OutcomeCodeListVM();
            var view = new OutcomeCodeListDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
        }

        private RelayCommand _licenseListCommand;
        public ICommand LicenseListCommand
        {
            get
            {
                if (_licenseListCommand == null)
                    _licenseListCommand = new RelayCommand(ShowLicensedHomes);
                return _licenseListCommand;
            }
        }
        private async void ShowLicensedHomes(object obj)
        {
            var vm = new LicenseHomeListVM();
            var view = new LicenseHomeListDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
        }

        public void ClosingEventHandlerProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogSettingBoolReturn = false;
                return;
            }
            DialogSettingBoolReturn = true;
        }

        public SettingsVM()
        {
            NormalCurve = "15.99";
        }
    }
}
