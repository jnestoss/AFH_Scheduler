using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AFH_Scheduler.Algorithm;

namespace AFH_Scheduler.Complete
{
    /// <summary>
    /// Interaction logic for Rescheduling_Follow_Up.xaml
    /// </summary>
    public partial class Rescheduling_Follow_Up : Window
    {
        public Rescheduling_Follow_Up(RescheduleVM rescheduleVM)
        {
            InitializeComponent();
            DataContext = rescheduleVM;
        }
    }
}
