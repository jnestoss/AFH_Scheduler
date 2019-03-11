using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.Errors
{
    public class ImportErrorModel : INotifyPropertyChanged
    {
        private int rowNumber;
        private string errorCase;

        public ImportErrorModel(int row, string error)
        {
            RowNumber = row;
            ErrorCase = error;
        }

        public int RowNumber
        {
            get { return rowNumber; }
            set
            {
                if (rowNumber == value) return;
                rowNumber = value;
                OnPropertyChanged("RowNumber");
            }
        }

        public string ErrorCase
        {
            get { return errorCase; }
            set
            {
                if (errorCase == value) return;
                errorCase = value;
                OnPropertyChanged("ErrorCase");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
