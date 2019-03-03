using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.Confirmation
{
    public class DeleteVM : ObservableObject, IPageViewModel
    {
        private string _name;
        public string SelectedName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("SelectedName");
            }
        }

        private string _deleteMessage;
        public string DeleteMessage
        {
            get { return _deleteMessage; }
            set
            {
                _deleteMessage = value;
                OnPropertyChanged("DeleteMessage");
            }
        }

        private string _itemType;
        public string ItemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                OnPropertyChanged("ItemType");
            }
        }

        public DeleteVM(string msg, string item, string nameAssigned)
        {
            DeleteMessage = msg;
            ItemType = item;
            SelectedName = nameAssigned;
        }

        public string Name
        {
            get
            {
                return "Delete Selected Item";
            }
        }
    }
}
