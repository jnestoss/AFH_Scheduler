using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database.LoginDB;
using AFH_Scheduler.Dialogs.SettingSubWindows;
using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.HelperClasses;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class AccountManagerVM : ObservableObject
    {
        public AccountManagerVM()
        {
            _accountsList = new ObservableCollection<AccountModel>();

            FillAccountTable();
        }

        private ObservableCollection<AccountModel> _accountsList;
        public ObservableCollection<AccountModel> AccountsList
        {
            get { return _accountsList; }
            set
            {
                if (value != _accountsList)
                {
                    _accountsList = value;
                    OnPropertyChanged("AccountsList");
                }
            }
        }

        private bool _dialogBool;
        public bool DialogBoolReturn
        {
            get { return _dialogBool; }
            set
            {
                _dialogBool = value;
                OnPropertyChanged("DialogBoolReturn");
            }
        }

        #region Delete Account Command
        private RelayCommand _accountDeleteCommand;
        public ICommand AccountDeleteCommand
        {
            get
            {
                if (_accountDeleteCommand == null)
                    _accountDeleteCommand = new RelayCommand(DeleteAccount);
                return _accountDeleteCommand;
            }
        }
        private async void DeleteAccount(object obj)
        {

        }
        #endregion

        #region Add Account Command
        private RelayCommand _addAccountCommand;
        public ICommand AddAccountCommand
        {
            get
            {
                if (_addAccountCommand == null)
                    _addAccountCommand = new RelayCommand(AddAccount);
                return _addAccountCommand;
            }
        }
        private async void AddAccount(object obj)
        {
            var account = (AccountModel)obj;
            var vm = new AddAccountVM();
            var view = new AddAccountDialog(vm);
            var result = await DialogHost.Show(view, "AccountsDialog", ClosingEventHandlerAddAccount);
            if (DialogBoolReturn)
            {
                using (UserLoginEntities db = new UserLoginEntities())
                {
                    CryptSharp.BlowfishCrypter crypt = new CryptSharp.BlowfishCrypter();
                    string salt = crypt.GenerateSalt();
                    Database.LoginDB.Login newUser = new Database.LoginDB.Login(vm.NewUsername, crypt.Crypt(vm.NewPassword, salt), salt, vm.NewAdministrator);
                    db.Logins.Add(newUser);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region Edit Account
        private RelayCommand _accountEditCommand;
        public ICommand AccountEditCommand
        {
            get
            {
                if (_accountEditCommand == null)
                    _accountEditCommand = new RelayCommand(EditAccount);
                return _accountEditCommand;
            }
        }
        private async void EditAccount(object obj)
        {
            var account = (AccountModel)obj;
            var vm = new EditAccountVM(account.Username, account.Password, account.Administrator);
            var view = new EditAccount(vm);
            var result = await DialogHost.Show(view, "AccountsDialog", ClosingEventHandlerEditAccount);
            if (DialogBoolReturn)
            {
                using (UserLoginEntities db = new UserLoginEntities())
                {
                    CryptSharp.BlowfishCrypter crypt = new CryptSharp.BlowfishCrypter();
                    string salt = crypt.GenerateSalt();
                    Database.LoginDB.Login editUser = db.Logins.Single(x => x.Username == vm.EditUsername);
                    editUser.Password = crypt.Crypt(vm.EditPassword, salt);
                    editUser.Salt = salt;
                    //db.Logins.Add(newUser);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        private void ClosingEventHandlerEditAccount(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogBoolReturn = false;
                return;
            }
            DialogBoolReturn = true;
            FillAccountTable();
        }
        public void ClosingEventHandlerAddAccount(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogBoolReturn = false;
                return;
            }
            DialogBoolReturn = true;
            FillAccountTable();
        }
        public void FillAccountTable()
        {
            LoginDBhelper dbhelper = new LoginDBhelper();
            AccountsList.Clear();
            foreach (AccountModel login in dbhelper.LoadAccounts())
            {
                AccountsList.Add(login);
            }
        }
    }
}
