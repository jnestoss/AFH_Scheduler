using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.Help
{
    public class HelpBoxVM : ObservableObject, IPageViewModel
    {
        private string dialogMessage;
        public string DialogMessage
        {
            get { return dialogMessage; }
            set
            {
                dialogMessage = value;
                OnPropertyChanged("DialogMessage");
            }
        }

        public bool _homeButtonsOn;
        public bool HomeButtonsOn
        {
            get { return _homeButtonsOn; }
            set
            {
                _homeButtonsOn = value;
                OnPropertyChanged("HomeButtonsOn");
            }
        }

        public bool _subButtonsMainOn;
        public bool SubButtonsMainOn
        {
            get { return _subButtonsMainOn; }
            set
            {
                _subButtonsMainOn = value;
                OnPropertyChanged("SubButtonsMainOn");
            }
        }


        public bool _subButtonsAddOn;
        public bool SubButtonsAddOn
        {
            get { return _subButtonsAddOn; }
            set
            {
                _subButtonsAddOn = value;
                OnPropertyChanged("SubButtonsAddOn");
            }
        }

        public bool _subButtonsEditOn;
        public bool SubButtonsEditOn
        {
            get { return _subButtonsEditOn; }
            set
            {
                _subButtonsEditOn = value;
                OnPropertyChanged("SubButtonsEditOn");
            }
        }

        public bool _subButtonsSettingOn;
        public bool SubButtonsSettingOn
        {
            get { return _subButtonsSettingOn; }
            set
            {
                _subButtonsSettingOn = value;
                OnPropertyChanged("SubButtonsSettingOn");
            }
        }

        public bool _subButtonsImportOn;
        public bool SubButtonsImportOn
        {
            get { return _subButtonsImportOn; }
            set
            {
                _subButtonsImportOn = value;
                OnPropertyChanged("SubButtonsImportOn");
            }
        }

        public bool _backButtonOn;
        public bool BackButtonOn
        {
            get { return _backButtonOn; }
            set
            {
                _backButtonOn = value;
                OnPropertyChanged("BackButtonOn");
            }
        }

        public HelpBoxVM()
        {
            DialogMessage = "You have reached the help page, select one of the buttons on the side to obtain a description of said functionality";
            HomeButtonsOn = true;
            BackButtonOn = false;
            SubButtonsMainOn = false;
            SubButtonsAddOn = false;
            SubButtonsEditOn = false;
            SubButtonsSettingOn = false;
            SubButtonsImportOn = false;
        }

        #region commands
        private RelayCommand _explainKeyCommand;
        public ICommand ExplainKeyCommand
        {
            get
            {
                if (_explainKeyCommand == null)
                    _explainKeyCommand = new RelayCommand(ExplainKeyFunction);
                return _explainKeyCommand;
            }
        }

        private void ExplainKeyFunction(object obj)
        {
            string keyword = (string)obj;
            if (keyword.Equals("Main"))
            {
                DialogMessage = "After logging in your username and password, you will come upon a data table listing several items.\n" +
                    "Each item represents an home from the database that is scheduled ";
                HomeButtonsOn = false;
                BackButtonOn = true;
                SubButtonsMainOn = true;
            }
            else if (keyword.Equals("Add"))
            {
                DialogMessage = "Add New Home button";
                HomeButtonsOn = false;
                BackButtonOn = true;
                SubButtonsAddOn = true;
            }
            else if (keyword.Equals("Edit"))
            {
                DialogMessage = "Edit Home Button button";
                HomeButtonsOn = false;
                BackButtonOn = true;
                SubButtonsEditOn = true;
            }
            else if (keyword.Equals("Settings"))
            {
                DialogMessage = "Settings button";
                HomeButtonsOn = false;
                BackButtonOn = true;
                SubButtonsSettingOn = true;
            }
            else if (keyword.Equals("Import_Export"))
            {
                DialogMessage = "Import/Export button";
                HomeButtonsOn = false;
                BackButtonOn = true;
                SubButtonsImportOn = true;
            }
            else if (keyword.Equals("BackButton"))
            {
                DialogMessage = "You have reached the help page, select one of the buttons on the side to obtain a description of said functionality";
                HomeButtonsOn = true;
                BackButtonOn = false;
                SubButtonsMainOn = false;
                SubButtonsAddOn = false;
                SubButtonsEditOn = false;
                SubButtonsSettingOn = false;
                SubButtonsImportOn = false;
            }
        }

        private RelayCommand _explainSubKeyCommand;
        public ICommand ExplainSubKeyCommand
        {
            get
            {
                if (_explainSubKeyCommand == null)
                    _explainSubKeyCommand = new RelayCommand(ExplainSubKeyFunction);
                return _explainSubKeyCommand;
            }
        }

        private void ExplainSubKeyFunction(object obj)
        {
            string keyword = (string)obj;
            //Main Table
            if (keyword.Equals("Complete Inspection"))
            {
                DialogMessage = "Complete Inspection button";
            }
            else if (keyword.Equals("Home History"))
            {
                DialogMessage = "Home History button";
            }
            //Edit Page
            else if (keyword.Equals("Active Homes"))
            {
                DialogMessage = "Active Homes button";
            }
            else if (keyword.Equals("Delete"))
            {
                DialogMessage = "Delete button";
            }
            //Settings
            else if (keyword.Equals("Provider List"))
            {
                DialogMessage = "Provider List button";
            }
            else if (keyword.Equals("Normal Curve"))
            {
                DialogMessage = "Normal Curve button";
            }
            //Import_Export
            else if (keyword.Equals("Import"))
            {
                DialogMessage = "Import button";
            }
            else if (keyword.Equals("Export"))
            {
                DialogMessage = "Export button";
            }
        }

        #endregion

        public string Name => "Help Page";
    }
}
