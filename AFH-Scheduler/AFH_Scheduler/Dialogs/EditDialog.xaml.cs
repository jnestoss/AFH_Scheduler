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

        public void setDataContext(HomeModel schedule)
        {
            this.DataContext = new EditVM(schedule);


            //trying to shut down this dialog after confirmation of deletion

            //this.Loaded += (s, e) =>
            //{
            //    if (DataContext is ICloseable)
            //    {
            //        (DataContext as ICloseable).RequestClose += (_, __) => this.;
            //    }
            //};
        }
    }
}