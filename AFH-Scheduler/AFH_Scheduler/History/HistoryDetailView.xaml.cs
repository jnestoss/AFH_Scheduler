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

namespace AFH_Scheduler.History
{
    /// <summary>
    /// Interaction logic for HistoryDetailView.xaml
    /// </summary>
    public partial class HistoryDetailView : Window
    {
        public HistoryDetailView(HistoryDetailViewVM historyDetailViewVM)
        {
            InitializeComponent();
            DataContext = historyDetailViewVM;
        }
    }
}
