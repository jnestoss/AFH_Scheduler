using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AFH_Scheduler
{

    public partial class DataView : UserControl
    {
        public DataView()
        {
            InitializeComponent();
        }

        private void FilterTextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (FilterTextBox.IsEnabled)
            {
                
                FilterTextBox.Visibility = Visibility.Visible;
                DatePickerStart.IsEnabled = false;
                DatePickerStart.Visibility = Visibility.Hidden;
                DatePickerEnd.IsEnabled = false;
                DatePickerEnd.Visibility = Visibility.Hidden;
                //ToTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                FilterTextBox.Visibility = Visibility.Hidden;
                DatePickerStart.IsEnabled = true;
                DatePickerStart.Visibility = Visibility.Visible;
                DatePickerEnd.IsEnabled = true;
                DatePickerEnd.Visibility = Visibility.Visible;
                //ToTextBlock.Visibility = Visibility.Visible;
            }
        }

        /*private void NoProviderImage_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (NoProviderImage.IsEnabled)
            {
                NoProviderImage.Visibility = Visibility.Visible;
            }
            else
            {
                NoProviderImage.Visibility = Visibility.Hidden;
            }
        }*/
    }
}