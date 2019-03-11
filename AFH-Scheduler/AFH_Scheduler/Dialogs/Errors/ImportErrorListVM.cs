using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.Errors
{
    public class ImportErrorListVM : ObservableObject
    {
        private ObservableCollection<ImportErrorModel> _errorList;
        public ObservableCollection<ImportErrorModel> ErrorList
        {
            get { return _errorList; }
            set
            {
                if (value != _errorList)
                {
                    _errorList = value;
                    OnPropertyChanged("ErrorList");
                }
            }
        }

        public ImportErrorListVM(List<ImportErrorModel> errorlist)
        {
            _errorList = new ObservableCollection<ImportErrorModel>();

            foreach (var error in errorlist)
            {
                _errorList.Add(error);
            }
        }
    }
}
