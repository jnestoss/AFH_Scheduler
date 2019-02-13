using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AFH_Scheduler.History
{
    public interface OpenMessageDialogService
    {
        Window DialogBox { get; set; }
        bool? ShowDialog(HistoryDetailViewVM historyDetailViewVM);
        void ReleaseMessageBox(string message);
        bool MessageConfirmation(string message, string caption);
        void DialogResultIsTrue();
    }

    public class openHistoryDetailDialog: OpenMessageDialogService
    {
        private Window _dialogBox;
        public Window DialogBox
        {
            get { return _dialogBox; }
            set
            {
                _dialogBox = value;
            }
        }

        public bool MessageConfirmation(string message, string caption)
        {
            MessageBoxResult choice = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return choice.Equals(MessageBoxResult.Yes);
        }

        public void ReleaseMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public bool? ShowDialog(HistoryDetailViewVM historyDetailViewVM)
        {
            //Rescheduling_Follow_Up follow = new Rescheduling_Follow_Up(rescheduleVM);
            DialogBox = new HistoryDetailView(historyDetailViewVM);
            return DialogBox.ShowDialog();
        }

        public void DialogResultIsTrue()
        {
            DialogBox.DialogResult = true;
        }
    }
}
