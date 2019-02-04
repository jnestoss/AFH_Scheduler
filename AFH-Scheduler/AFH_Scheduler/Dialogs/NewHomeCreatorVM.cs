using AFH_Scheduler.Complete;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Schedules;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class NewHomeCreatorVM : ObservableObject, IPageViewModel
    {
        private IOpenMessageDialogService _messageService;
        public IOpenMessageDialogService MessageService
        {
            get
            {
                if (_messageService == null)
                    _messageService = new SchedulesOpenDialog();
                return _messageService;
            }
            set
            {
                _messageService = value;
                OnPropertyChanged("MessageService");
            }
        }

        private NewHomeModel _newHomeCreated;
        public NewHomeModel NewHomeCreated
        {
            get { return _newHomeCreated; }
            set
            {
                _newHomeCreated = value;
                OnPropertyChanged("NewHomeCreated");
            }
        }

        private RelayCommand _submittingNewHome;
        public ICommand SubmittingNewHomeCommand
        {
            get
            {
                if (_submittingNewHome == null)
                    _submittingNewHome = new RelayCommand(SubmitNewHome);
                return _submittingNewHome;
            }
        }

        private void SubmitNewHome(object obj)
        {
            if (DoesRegexMatch())
            {
                if (MessageService.MessageConfirmation("You are about to add a new home to the database, are you sure?", "Creating Home"))
                {
                    //
                }
            }
        }

        private bool DoesRegexMatch()
        {
            Regex check = new Regex(@"^[0-9]+$");
            String message = "Provider ID is not in correct format. Be sure to use only whole numbers.";
            if (check.IsMatch(NewHomeCreated.ProviderID))//Check ProviderID
            {
                message = "Provider Name is not in correct format. Use only First and Last name, (hyphens can be included if needed).";
                check = new Regex(@"^[A-Z]([a-z][-']?)* [A-Z]([a-z][-']?)*$");
                if (check.IsMatch(NewHomeCreated.ProviderName))//Check ProviderName
                {
                    message = "Home ID is not in correct format. Be sure to use only whole numbers.";
                    check = new Regex(@"^[0-9]+$");
                    if (check.IsMatch(NewHomeCreated.HomeID))//Check HomeID
                    {
                        message = "Address is not in correct format.";
                        check = new Regex(@"^[0-9]+ [a-zA-z0-9]+( [a-zA-z0-9]+)?$");
                        if (check.IsMatch(NewHomeCreated.Address))//Check Address
                        {
                            message = "City is not in correct format.";
                            check = new Regex(@"^[A-Z][a-z]+([ -][A-Z][a-z]+)*$");
                            if (check.IsMatch(NewHomeCreated.City))//Check City
                            {
                                message = "Zipcode is not in correct format.";
                                check = new Regex(@"^[0-9]{5}(-[0-9]{4})?$");//0-9]{5}|[0-9]{5}[-][0-9]{4}
                                if (check.IsMatch(NewHomeCreated.Zipcode))//Check Zipccde
                                {
                                    message = "Inspection Date is not in correct format.";//0-9]{5}|[0-9]{5}[-][0-9]{4}
                                    if (NewHomeCreated.InspectionDate != null)//Check Zipccde
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            MessageService.ReleaseMessageBox(message);
            return false;
        }

        public bool CheckID()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                /*var provs = db.Providers.Where(x => x.Provider_ID == Convert.ToInt64(NewHomeCreated.ProviderID)).ToList();
                if (provs.Count == 0)//The Provider ID typed in doesn't exist
                {

                }*/
            }
                return false;
        }

        public NewHomeCreatorVM(IOpenMessageDialogService messageService)
        {
            MessageService = messageService;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                List<string> providers = new List<string>();
                string listItem = "";
                foreach(var item in provs)
                {
                    listItem = item.Provider_ID + "-" + item.Provider_Name;
                    providers.Add(listItem);
                }
                NewHomeCreated = new NewHomeModel(providers);
            }
        }

        public string Name
        {
            get
            {
                return "Reschedule FollowUp";
            }
        }
    }
}
