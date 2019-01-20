using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AFH_Scheduler.Complete
{
    public class CompleteInspectionVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        private ObservableCollection<CompleteSchedule_Model> _providers;
        public ObservableCollection<CompleteSchedule_Model> Providers
        {
            get { return _providers; }
            set
            {
                if (value != _providers)
                {
                    _providers = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

        private RelayCommand _submittingForm;
        public ICommand SubmittingFormCommand
        {
            get
            {
                if (_submittingForm == null)
                    _submittingForm = new RelayCommand(SubmitTheForm);
                return _submittingForm;
            }
        }

        private OpenMessageDialogService _messageService;
        public OpenMessageDialogService MessageService
        {
            get
            {
                if (_messageService == null)
                    _messageService = new CompleteOpenMessageDialogs();
                return _messageService;
            }
        }

        private RelayCommand _openFollowUpDialog;
        public ICommand OpenFollowUpDialogCommand
        {
            get
            {
                if (_openFollowUpDialog == null)
                    _openFollowUpDialog = new RelayCommand(FollowUpDialogOpen);
                return _openFollowUpDialog;
            }
        }

        public CompleteInspectionVM()
        {
            _providers = new ObservableCollection<CompleteSchedule_Model>();

            GenData();
        }

        private void SubmitTheForm(object obj)
        {
            CompleteSchedule_Model model = (CompleteSchedule_Model)obj;
            if (model.SelectedOutcome is null)
            {
                MessageService.ReleaseMessageBox("Your row's deficiency is not selected. Please select the deficiency for the selected row.");
                return;
            }
            
            if (MessageService.MessageConfirmation("Are you sure you want to submit? The " +
                "next inspection will be set up.", "Submitting"))
            {
                Providers.Remove(model);
                //Remove Item from Scheduled Inspections database and update it
                //Add Home_History new PHome's inspection date and outcome code
                
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    Scheduled_Inspections deletingSchedule = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == model.HomeID && r.SInspections_Date.Equals(model.InspectionDate)).First();
                    long newScheduleID = deletingSchedule.SInspections_Id;
                    db.Scheduled_Inspections.Remove(deletingSchedule);
                    db.SaveChanges();

                    Random randomiz = new Random();
                    int id = randomiz.Next(111, 8000+1);
                    while (true)
                    {
                        try
                        {
                            Home_History scheduled = db.Home_History.Where(r => r.HHistory_ID != id).First();
                            break;
                        }
                        catch (InvalidOperationException e)
                        {
                            id = randomiz.Next(111, 8000 + 1);
                        }
                    }
                    db.Home_History.Add(new Home_History { HHistory_ID = id, HHistory_Date = model.InspectionDate, FK_PHome_ID = model.HomeID, FK_IOutcome_Code = model.SelectedOutcome});
                    db.SaveChanges();

                    db.Scheduled_Inspections.Add(new Scheduled_Inspections { SInspections_Id = newScheduleID, SInspections_Date = alg.SchedulingNextDate(Convert.ToInt32(model.HomeID)), FK_PHome_ID = model.HomeID });
                    db.SaveChanges();
                }
                MessageService.ReleaseMessageBox("Your form is Submitted.");
            }
        }
        private void FollowUpDialogOpen(object obj)
        {
            CompleteSchedule_Model model = (CompleteSchedule_Model)obj;
            RescheduleVM rescheduleVM = new RescheduleVM(model.FollowUpDate, MessageService);
            var updateOrNot = MessageService.ShowDialog(rescheduleVM);
            if (updateOrNot == true)
            {
                model.FollowUpDate = rescheduleVM.RescheduledFollowUpDate;
            }
        }

        public string Name {
            get {
                return "Complete Inspection";
            }
        }
        public void GenData()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var outcome = db.Inspection_Outcome.ToList();
                List<string> codeWordList = new List<string>();
                foreach (var codeWord in outcome)
                {
                    codeWordList.Add(codeWord.IOutcome_Code);
                }

                var insp = db.Scheduled_Inspections.ToList();
                List<long?> provList = new List<long?>();

                foreach (var inspection in insp)
                {
                    if (DateTime.Compare(alg.ExtractDateTime(inspection.SInspections_Date), DateTime.Today) <= 0)
                    {
                        var providerHome = db.Provider_Homes.Where(r => r.PHome_ID == inspection.FK_PHome_ID).First();
                        var name = db.Providers.Where(r => r.Provider_ID == providerHome.FK_Provider_ID).First().Provider_Name;
                        Providers.Add(
                        new CompleteSchedule_Model
                        (
                           providerHome.FK_Provider_ID,
                           name,
                           providerHome,
                           inspection.SInspections_Date,
                           codeWordList
                          )
                        );
                    }
                }
            }
        }
    }
}