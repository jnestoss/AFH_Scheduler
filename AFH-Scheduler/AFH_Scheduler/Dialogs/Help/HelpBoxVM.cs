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

        public bool _subButtonsOn;
        public bool SubButtonsOn
        {
            get { return _subButtonsOn; }
            set
            {
                _subButtonsOn = value;
                OnPropertyChanged("SubButtonsOn");
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
            SubButtonsOn = false;
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
                SubButtonsOn = true;
            }
            else if (keyword.Equals("Complete Inspection"))
            {
                DialogMessage = "Complete Inspection button";
            }
            else if (keyword.Equals("Home History"))
            {
                DialogMessage = "Home History button";
            }
            else if (keyword.Equals("Add"))
            {
                DialogMessage = "Add New Home button";
                HomeButtonsOn = false;
                BackButtonOn = true;
            }
            else if (keyword.Equals("Edit"))
            {
                DialogMessage = "Edit Home Button button";
                HomeButtonsOn = false;
                BackButtonOn = true;
            }
            else if (keyword.Equals("Settings"))
            {
                DialogMessage = "Settings button";
                HomeButtonsOn = false;
                BackButtonOn = true;
            }
            else if (keyword.Equals("Import_Export"))
            {
                DialogMessage = "Import/Export button";
                HomeButtonsOn = false;
                BackButtonOn = true;
            }
            else if (keyword.Equals("BackButton"))
            {
                DialogMessage = "You have reached the help page, select one of the buttons on the side to obtain a description of said functionality";
                HomeButtonsOn = true;
                BackButtonOn = false;
                SubButtonsOn = false;
            }
        }

        #endregion

        public string Name => "Help Page";
    }
}
