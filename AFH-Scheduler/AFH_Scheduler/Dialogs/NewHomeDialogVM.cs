﻿using AFH_Scheduler.Complete;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class NewHomeDialogVM : ObservableObject, IPageViewModel
    {
        private NewHomeModel _newHomeCreated;
        public NewHomeModel NewHomeCreated
        {
            get { return _newHomeCreated; }
            set
            {
                _newHomeCreated = value;
                OnPropertyChanged("NewHomeCreated");
            }
        }

        public NewHomeDialogVM()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                List<ProvidersModel> providers = new List<ProvidersModel>();
                providers.Add(new ProvidersModel("-1", "No Provider"));
                string listItem = "";
                foreach (var item in provs)
                {
                    listItem = item.Provider_ID + "-" + item.Provider_Name;
                    providers.Add(new ProvidersModel(item.Provider_ID.ToString(), item.Provider_Name));
                }
                NewHomeCreated = new NewHomeModel(providers);
            }
        }

        public string Name
        {
            get
            {
                return "Create New Home";
            }
        }
    }
}
