using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AFH_Scheduler.Complete
{
    public interface OpenMessageDialogService
    {
        Window DialogBox { get; set; }
        bool? ShowDialog(RescheduleVM rescheduleVM);
        void ReleaseMessageBox(string message);
        bool MessageConfirmation(string message, string caption);
        void DialogResultIsTrue();
    }
    public class OpenMessageDialogs : OpenMessageDialogService
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

        public bool? ShowDialog(RescheduleVM rescheduleVM)
        {
            //Rescheduling_Follow_Up follow = new Rescheduling_Follow_Up(rescheduleVM);
            DialogBox = new Rescheduling_Follow_Up(rescheduleVM);
            return DialogBox.ShowDialog();
        }

        public void DialogResultIsTrue()
        {
            DialogBox.DialogResult = true;
        }
    }
}
