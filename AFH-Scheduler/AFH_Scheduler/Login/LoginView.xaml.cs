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

namespace AFH_Scheduler.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView :UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        //Based on: https://stackoverflow.com/a/25001115
        private void UpdateViewModelPassword(object sender, RoutedEventArgs e)
        {
            ((LoginViewVM)DataContext).Password = password.Password;
        }
    }
}
