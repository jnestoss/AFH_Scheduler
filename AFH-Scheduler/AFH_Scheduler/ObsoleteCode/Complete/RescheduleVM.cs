using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Complete
{
    public class RescheduleVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private string _rescheduledFollowUpDate;
        public string RescheduledFollowUpDate
        {
            get { return _rescheduledFollowUpDate; }
            set
            {
                _rescheduledFollowUpDate = value;

                OnPropertyChanged("RescheduledFollowUpDate");
            }
        }

        private DateTime _datePicked;
        public DateTime DatePicked
        {
            get { return _datePicked; }
            set
            {
                _datePicked = value;
                RescheduledFollowUpDate = _datePicked.ToShortDateString();

                OnPropertyChanged("DatePicked");
            }
        }

        public RescheduleVM(string followUpDate, OpenMessageDialogService messageService)
        {
            DatePicked = alg.ExtractDateTime(followUpDate);
            MessageService = messageService;
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
            set
            {
                _messageService = value;
                OnPropertyChanged("MessageService");
            }
        }

        private RelayCommand _submittingUpdate;
        public ICommand SubmittingUpdateCommand
        {
            get
            {
                if (_submittingUpdate == null)
                    _submittingUpdate = new RelayCommand(SubmittingDialog);
                return _submittingUpdate;
            }
        }

        public void SubmittingDialog(object obj)
        {
            Regex dateFormat = new Regex(@"^[1]?[0-9][/]{1}[1-3]?[0-9][/]{1}[1-2][0-9][0-9][0-9]$");

            if (dateFormat.IsMatch(RescheduledFollowUpDate))
            {
                MessageService.DialogResultIsTrue();
            }
            else
            {
                MessageService.ReleaseMessageBox("Your current rescheduled follow does not match our format." +
                    " Please use this format for writing the date: mm/dd/yyyy, example 1/12/2000");
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
