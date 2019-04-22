using AFH_Scheduler.Dialogs.SettingSubWindows;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
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

        private double _currentAverage;
        public double CurrentAverage {
            get { return _currentAverage; }
            set {
                if (_currentAverage == value) return;
                _currentAverage = value;
                OnPropertyChanged("CurrentAverage");
            }
        }

        private double _desiredAverage;
        public double DesiredAverage {
            get => _desiredAverage;
            set {
                if (_desiredAverage == value) return;
                _desiredAverage = value;
                OnPropertyChanged("DesiredAverage");
            }
        }

        private string _usr;
        public string UserSignedIn
        {
            get => _usr;
        }

        private RelayCommand _accountManagerCommand;
        public ICommand AccountManagerCommand
        {
            get
            {
                if (_accountManagerCommand == null)
                    _accountManagerCommand = new RelayCommand(ShowAccountManager);
                return _accountManagerCommand;
            }
        }
        private async void ShowAccountManager(object obj)
        {
            var vm = new AccountManagerVM(UserSignedIn);
            var view = new AccountManagerDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerAccounts);
            if (result.Equals("BACKTOLOGIN"))
            {
                DialogHost.CloseDialogCommand.Execute("BACKTOLOGIN", null);
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
            var vm = new ProviderListVM(CurrentAverage, DesiredAverage);
            var view = new ProviderListDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
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

        public void ClosingEventHandlerProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogSettingBoolReturn = false;
                return;
            }
            DialogSettingBoolReturn = true;
        }
        public void ClosingEventHandlerAccounts(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogSettingBoolReturn = false;
                return;
            }
            DialogSettingBoolReturn = true;
        }
        public SettingsVM(string userSignedIn, double normalCurve, double desiredAverage)
        {
            _usr = userSignedIn;
            CurrentAverage = normalCurve;
            DesiredAverage = desiredAverage;

            string text = File.ReadAllText(@"..\..\..\NormalCurve\NormalCurveValue.txt");
            double testCase;
            if (!Double.TryParse(text, out testCase))
            {
                File.WriteAllText(@"..\..\..\NormalCurve\NormalCurveValue.txt", String.Empty);
                File.WriteAllText(@"..\..\..\NormalCurve\NormalCurveValue.txt", "15.99");
                text = "15.99";
                testCase = 15.99;
            }
            DesiredAverage = testCase;
        }
    }
}
