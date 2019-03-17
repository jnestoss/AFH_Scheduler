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
    /// Interaction logic for AccountManagerDialog.xaml
    /// </summary>
    public partial class AccountManagerDialog : UserControl
    {
        public AccountManagerDialog(AccountManagerVM accountManagerVM)
        {
            InitializeComponent();
            DataContext = accountManagerVM;
        }
    }
}
