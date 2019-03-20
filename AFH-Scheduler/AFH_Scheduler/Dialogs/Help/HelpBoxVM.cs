using AFH_Scheduler.Dialogs.Help.Pages;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.Help
{
    public class HelpBoxVM : ObservableObject, IPageViewModel
    {

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        #region Main Table Tree Descriptions
        private string _mainDescription;
        public string MainDecription {
            get {
                return _mainDescription;
            }
            set {
                if (value != _mainDescription)
                {
                    _mainDescription = value;
                    OnPropertyChanged("MainDecription");
                }
            }
        }

        private string _completeInspectDescription;
        public string CompleteInspectDescription {
            get {
                return _completeInspectDescription;
            }
            set {
                if (value != _completeInspectDescription)
                {
                    _completeInspectDescription = value;
                    OnPropertyChanged("CompleteInspectDescription");
                }
            }
        }

        private string _historyDescription;
        public string HistoryDescription {
            get {
                return _historyDescription;
            }
            set {
                if (value != _historyDescription)
                {
                    _historyDescription = value;
                    OnPropertyChanged("HistoryDescription");
                }
            }
        }

        private string _editDescription;
        public string EditDescription {
            get {
                return _editDescription;
            }
            set {
                if (value != _editDescription)
                {
                    _editDescription = value;
                    OnPropertyChanged("EditDescription");
                }
            }
        }

        private string _inactiveHomeDescription;
        public string InactiveHomeDescription {
            get {
                return _inactiveHomeDescription;
            }
            set {
                if (value != _inactiveHomeDescription)
                {
                    _inactiveHomeDescription = value;
                    OnPropertyChanged("InactiveHomeDescription");
                }
            }
        }

        private string _importDescription;
        public string ImportDescription {
            get {
                return _importDescription;
            }
            set {
                if (value != _importDescription)
                {
                    _importDescription = value;
                    OnPropertyChanged("ImportDescription");
                }
            }
        }

        private string _exportDescription;
        public string ExportDescription {
            get {
                return _exportDescription;
            }
            set {
                if (value != _exportDescription)
                {
                    _exportDescription = value;
                    OnPropertyChanged("ExportDescription");
                }
            }
        }
        #endregion

        #region Settings Tree Descriptions
        private string _selectedPath;
        public string SelectedPath {
            get => _selectedPath;
            set {
                if (_selectedPath == value)
                {
                    return;
                }

                _selectedPath = value;
                Console.WriteLine("Selected path is:           " + _selectedPath);
                OnPropertyChanged("SelectedPath");
            }
        }

        private string _settingsDescription;
        public string SettingsDescription {
            get {
                return _settingsDescription;
            }
            set {
                if (value != _settingsDescription)
                {
                    _settingsDescription = value;
                    OnPropertyChanged("SettingsDescription");
                }
            }
        }

        private string _providersDescription;
        public string ProvidersDescription {
            get {
                return _providersDescription;
            }
            set {
                if (value != _providersDescription)
                {
                    _providersDescription = value;
                    OnPropertyChanged("ProvidersDescription");
                }
            }
        }

        private string _normalValueDescription;
        public string NormalValueDescription {
            get {
                return _normalValueDescription;
            }
            set {
                if (value != _normalValueDescription)
                {
                    _normalValueDescription = value;
                    OnPropertyChanged("NormalValueDescription");
                }
            }
        }

        private string _outcomeDescription;
        public string OutcomeDescription {
            get {
                return _outcomeDescription;
            }
            set {
                if (value != _outcomeDescription)
                {
                    _outcomeDescription = value;
                    OnPropertyChanged("OutcomeDescription");
                }
            }
        }

        #endregion

        public HelpBoxVM()
        {
            // Add available pages
            PageViewModels.Add(new MainHelpVM());

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];

            LoadMainTableTree();
            LoadSettingsTree();
        }

        public ICommand ChangePageCommand {
            get {
                //SetThickness(_currentPageViewModel.Name);
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
                {
                    _pageViewModels = new List<IPageViewModel>();
                }

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
                    //SetThickness(CurrentPageViewModel.Name);
                    OnPropertyChanged("CurrentPageViewModel");
                }
                //SetThickness(_currentPageViewModel.Name);
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
            {
                PageViewModels.Add(viewModel);
            }

            CurrentPageViewModel = PageViewModels.Find(vm => vm == viewModel);
        }

        public void LoadMainTableTree()
        {
            MainDecription = "Description: After logging into the application, you will find a table that lists out " +
                "several licensed Adult Family Homes(AFH) that have upcoming inspections. Here you can look into a home’s history of " +
                "previous inspection, complete an inspection on a home, and edit information on the home. These three options are " +
                "found in the popup icon at the bottom right corner of the table. You must select a row first before selecting an " +
                "option on this popup. To select a row, simply click on the row and it will be highlighted.\n" +
                "You may also filter out the table to find certain homes based on keyword: License Number or name, street address, or upcoming inspections.\n" +
                "There is an option to reactivate homes that are currently on standby and waiting to be reactivated.\n" +
                "The application also support excel spreadsheets(.xlsx and .csv) and allows user to import or export mass amounts of data.";

            CompleteInspectDescription = "When the selected AFH has recently completed an inspection, you can log the results of the " +
                "inspection in this window. The window will calculate a preview of when the next inspection date will based on the " +
                "outcome. Submitting the form will update the home’s inspection on the main table and update its history.";

            HistoryDescription = "The home’s history displays all past inspections that have taken place, along with its outcome.";

            EditDescription = "The edit page will display all the import information on the selected AFH that can be edited. " +
                "Here you can alter the facilities POC, license number and/or facility name, etc. Rescheduling upcoming inspections are " +
                "also performed through this window.";

            InactiveHomeDescription = "This window will list all homes on standby that are not include for RCS’ inspections." +
                " When reactivating a home, you are given an option to reschedule an inspection that has previously been set on the home" +
                " but canceled. After reactivating all the select homes, you can either submit the reactivation or cancel the process.";

            ImportDescription = "Import: The import function providers a data preview on what will be added to the main table." +
                " When loading an excel file into the application, there is a template that must be followed. " +
                "The template requires the following columns in the exact format: " +
                "LicenseNumber, FacilityName, LocationAddress, LocationCity, LocationZipCode, TelephoneNmbr, RCSRegionUnit, FacilityPOC, " +
                "RecentInspection, and NextInspection.\n" +
                "In each row, the telephone number, facility POC, recent and inspection date can be left blank. " +
                "The POC will be labeled as “No Provider”, and the next inspection will be calculated based off the recent inspection. " +
                "In which case, if the recent inspection is left blank, then it will default to the day it was uploaded to the application.";

            ExportDescription = "The export function will copy whatever is listed on the main table and pass it down on the excel spreadsheet." +
                " The excel spreadsheet will also show additional information: \n" +
                "Interval between the most recent inspection to the current one (both in months and days), 17th month drop dead, " +
                "a forecasted outcome to the AFH’s next current inspection, and forecasting the following inspection based on the outcome.";
        }

        public void LoadSettingsTree()
        {
            SettingsDescription = "Description: Here you can alter a few items that will alter the application’s functionality in terms of " +
                "how inspections are calculated and loaded in.";

            ProvidersDescription = "All the providers list are linked to the application’s database. " +
                "Here providers can be added or removed from the application. " +
                "When removing a provider that is currently assigned to one or more homes, you must perform a change of ownership (CHOW)." +
                " An editing window will pop up where you can update the license information on the AFH.";

            NormalValueDescription = "The normal curve value is use to compare the total average of inspections on each active home. " +
                "By default, the normal monthly curve is set to 15.99 average comparison, but can be adjusted in the settings page.\n" +
                "Warning: adjusting the average curve will immediately alter the dialog on the main table page.";

            OutcomeDescription = "Here you can add and edit the definition outcome code words for inspections. " +
                "You can only edit the minimum and maximum value range of months added to randomize the next inspection date " +
                "(Please note that change the values only take affect when the home has recently completed an inspection).";
        }

        public string Name => "Help Page";
    }
}
