using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Login;
using AFH_Scheduler.Database.LoginDB;
using AFH_Scheduler.Schedules;

namespace AFH_Scheduler
{
    public class MainVM : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;

        public MainVM()
        {
            CurrentPageViewModel = new LoginViewVM(this);

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
        public void LoggedIn(User user)
        {
            if (user != null)
            {
                CurrentPageViewModel = new DataVM(user);
            }
        }
        public void Logout(object obj)
        {
            ((DataVM)CurrentPageViewModel).ClearUser();
            CurrentPageViewModel = new LoginViewVM(this);
        }

        #endregion

    }
}