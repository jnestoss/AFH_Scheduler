using AFH_Scheduler.Helper_Classes;

namespace AFH_Scheduler
{
    public class MainVM : ObservableObject
    {
        private IPageViewModel _currentPageViewModel;

        public MainVM()
        {
            CurrentPageViewModel = new DataVM();

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

        #endregion

    }
}