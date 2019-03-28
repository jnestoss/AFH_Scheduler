using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Login;
using AFH_Scheduler.Database.LoginDB;

namespace AFH_Scheduler
{
    public class MainVM : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;
        private bool _settingsEnabled;

        public MainVM()
        {
            CurrentPageViewModel = new LoginViewVM(this);
            _settingsEnabled = false;
        }


        #region Properties / Commands

        public IPageViewModel CurrentPageViewModel {
            get {
                return _currentPageViewModel;
            }
            set {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }
        public bool SettingsEnabled
        {
            get
            {
                return _settingsEnabled;
            }
            set
            {
                if (CurrentPageViewModel.GetType() == typeof(DataVM))
                {
                    _settingsEnabled = value;
                    OnPropertyChanged("SettingsEnabled");
                }
                else
                {
                    _settingsEnabled = false;
                    OnPropertyChanged("SettingsEnabled");
                }
            }
        }
        public void LoggedIn(User user)
        {
            if (user != null)
            {
                CurrentPageViewModel = new DataVM(user);
                if (user.Admin)
                    SettingsEnabled = true;
                else
                    SettingsEnabled = false;
            }
        }
       
        #endregion

    }
}