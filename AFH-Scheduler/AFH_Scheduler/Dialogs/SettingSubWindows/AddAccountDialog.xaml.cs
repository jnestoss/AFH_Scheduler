using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    /// <summary>
    /// Interaction logic for AddAccountDialog.xaml
    /// </summary>
    public partial class AddAccountDialog : UserControl
    {
        public AddAccountDialog(AddAccountVM addAccountVM)
        {
            InitializeComponent();
            DataContext = addAccountVM;
        }
    }
}
