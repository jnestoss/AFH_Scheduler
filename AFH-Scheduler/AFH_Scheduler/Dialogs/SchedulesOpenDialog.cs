using AFH_Scheduler.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AFH_Scheduler.Dialogs
{
    public interface IOpenMessageDialogService
    {
        void ReleaseMessageBox(string message);
        bool MessageConfirmation(string message, string caption);
        string ExcelSaveDialog();
    }
    class SchedulesOpenDialog : IOpenMessageDialogService
    {
        public void ReleaseMessageBox(string message)
        {
            MessageBox.Show(message);
        }
        public bool MessageConfirmation(string message, string caption)
        {
            MessageBoxResult choice = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return choice.Equals(MessageBoxResult.Yes);
        }

        public string ExcelSaveDialog()
        {
            SaveFileDialog excelSaveFile = new SaveFileDialog();
            excelSaveFile.DefaultExt = ".xlsx";
            excelSaveFile.Filter = "Excel Worksheets|*.xlsx";
            excelSaveFile.OverwritePrompt = true;

            if(excelSaveFile.ShowDialog() == true)
            {
                return excelSaveFile.FileName;
            }

            return null;
        }
    }
}
