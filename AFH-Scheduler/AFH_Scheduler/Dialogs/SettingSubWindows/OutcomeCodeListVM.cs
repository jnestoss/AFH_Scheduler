using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Dialogs.Confirmation;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class OutcomeCodeListVM : ObservableObject
    {
        private ObservableCollection<OutcomeModel> _outcomeList;
        public ObservableCollection<OutcomeModel> OutcomeList
        {
            get { return _outcomeList; }
            set
            {
                if (value != _outcomeList)
                {
                    _outcomeList = value;
                    OnPropertyChanged("OutcomeList");
                }
            }
        }

        #region Add Outcome
        private RelayCommand _outcomeAddCommand;
        public ICommand OutcomeAddCommand
        {
            get
            {
                if (_outcomeAddCommand == null)
                    _outcomeAddCommand = new RelayCommand(AddOutcome);
                return _outcomeAddCommand;
            }
        }

        private async void AddOutcome(object obj)
        {            
            var vm = new OutcomeDialogsVM("","","");
            var view = new NewOutcomeCreator(vm);
            var result = await DialogHost.Show(view, "OutcomeListDialog", ClosingEventHandlerOutcome);
            if (result.Equals("SUBMIT"))
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    db.Inspection_Outcome.Add(new Inspection_Outcome
                    {
                        IOutcome_Code = vm.OutcomeData.Outcome_code,
                        IOutcome_Mintime = vm.OutcomeData.Outcome_mintime,
                        IOutcome_Maxtime = vm.OutcomeData.Outcome_maxtime
                    });
                    db.SaveChanges();
                }
                OutcomeList.Add(vm.OutcomeData);
            }

        }
        #endregion

        #region Edit Outcome
        private RelayCommand _outcomeEditCommand;
        public ICommand OutcomeEditCommand
        {
            get
            {
                if (_outcomeEditCommand == null)
                    _outcomeEditCommand = new RelayCommand(EditOutcome);
                return _outcomeEditCommand;
            }
        }

        private async void EditOutcome(object obj)
        {
            OutcomeModel model = (OutcomeModel)obj;
            var vm = new OutcomeDialogsVM(model.Outcome_code, model.Outcome_mintime, model.Outcome_maxtime);
            var view = new EditOutcome(vm);
            var result = await DialogHost.Show(view, "OutcomeListDialog", ClosingEventHandlerOutcome);
            if (result.Equals("SUBMIT"))
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    var dbOutcome = db.Inspection_Outcome.Where(r => r.IOutcome_Code.Equals(model.Outcome_code)).First();

                    dbOutcome.IOutcome_Mintime = vm.OutcomeData.Outcome_mintime;
                    db.SaveChanges();

                    dbOutcome.IOutcome_Maxtime = vm.OutcomeData.Outcome_maxtime;
                    db.SaveChanges();
                }
                model.Outcome_mintime = vm.OutcomeData.Outcome_mintime;
                model.Outcome_maxtime = vm.OutcomeData.Outcome_maxtime;
            }
        }
        #endregion

        #region Delete Outcome
        private RelayCommand _outcomeDeleteCommand;
        public ICommand OutcomeDeleteCommand
        {
            get
            {
                if (_outcomeDeleteCommand == null)
                    _outcomeDeleteCommand = new RelayCommand(DeleteOutcome);
                return _outcomeDeleteCommand;
            }
        }
        private async void DeleteOutcome(object obj)
        {
            OutcomeModel model = (OutcomeModel)obj;
            var vm = new DeleteVM("Are you sure you want to remove this codeword from the database?", "Codeword:",
                model.Outcome_code);
            var deleteView = new DeleteProviderDialog(vm);

            var deleteResult = await DialogHost.Show(deleteView, "OutcomeListDialog", ClosingEventHandlerOutcome);

            if (deleteResult.Equals("Yes"))
            {
                OutcomeList.Remove(model);
            }
        }
        #endregion

        private void ClosingEventHandlerOutcome(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }
            
        }

        public OutcomeCodeListVM()
        {
            _outcomeList = new ObservableCollection<OutcomeModel>();

            FillOutcomeTable();
        }

        public void FillOutcomeTable()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var outcome = db.Inspection_Outcome.ToList();
                foreach (var codeword in outcome)
                {
                    OutcomeList.Add(
                            new OutcomeModel
                            (
                                codeword.IOutcome_Code,
                                codeword.IOutcome_Mintime,
                                codeword.IOutcome_Maxtime
                            )
                        );
                }
            }
        }


    }
}
