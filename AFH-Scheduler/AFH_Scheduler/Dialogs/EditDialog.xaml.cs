using System;
using System.Windows.Controls;
using AFH_Scheduler.Data;

namespace AFH_Scheduler.Dialogs
{
    public partial class EditDialog : UserControl
    {
        public EditDialog()
        {
            InitializeComponent();
            
        }

        public void setDataContext(ScheduleModel schedule)
        {
            this.DataContext = new EditVM(schedule);
        }
    }
}