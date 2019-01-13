using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AFH_Scheduler.Schedules;
using AFH_Scheduler.History;
using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Complete;

namespace AFH_Scheduler
{
    public class MainVM : ObservableObject
    {
        public int view_id { get; set; }

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        private Thickness _activeButton;

        public MainVM()
        {
            // Add available pages
            PageViewModels.Add(new SchedulerVM());
            PageViewModels.Add(new HistoryVM());
            PageViewModels.Add(new CompleteInspectionVM());

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
            _activeButton = new Thickness(0, 0, 0, 0);

        }

        #region Properties / Commands


        public ICommand ChangePageCommand {
            get {
                SetThickness(_currentPageViewModel.Name);
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels {
            get {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel {
            get {
                //SetThickness(_currentPageViewModel.Name);
                return _currentPageViewModel;
            }
            set {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    SetThickness(CurrentPageViewModel.Name);
                    OnPropertyChanged("CurrentPageViewModel");
                }
                //SetThickness(_currentPageViewModel.Name);
            }
        }

        public Thickness ActiveButton {
            get {
                return _activeButton;
            }
            set {
                _activeButton = value;
                OnPropertyChanged("ActiveButton");
            }
        }

        #endregion

        #region Methods

        private void SetThickness(string name)
        {
            Console.WriteLine("HI HI HI");
            switch (name)
            {
                case "Schedules":
                    ActiveButton = new Thickness(0, 0, 0, 0);
                    break;
                case "History":
                    ActiveButton = new Thickness(360, 0, 0, 0);
                    break;
                case "Complete Inspection":
                    ActiveButton = new Thickness(720, 0, 0, 0);
                    break;
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);            
        }

        #endregion

    }
}