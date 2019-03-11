using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class NewHomeDialogVM : ObservableObject, IPageViewModel
    {
        private List<ProvidersModel> _providers;
        public List<ProvidersModel> Providers
        {
            get { return _providers; }
            set
            {
                if (_providers == value) return;
                _providers = value;
                OnPropertyChanged("Providers");
            }
        }
        public ICollectionView ComboBoxProviderItems { get; }

        private string _TextSearch;
        public string TextSearch
        {
            get
            {
                return _TextSearch;
            }
            set
            {
                if (_TextSearch != value)
                {
                    _TextSearch = value;
                    ComboBoxProviderItems.Refresh();
                    OnPropertyChanged("TextSearch");
                }
            }
        }

        private HomeModel _newHomeCreated;
        public HomeModel NewHomeCreated
        {
            get { return _newHomeCreated; }
            set
            {
                _newHomeCreated = value;
                OnPropertyChanged("NewHomeCreated");
            }
        }

        private DateTime _datePicked;
        public DateTime DatePicked
        {
            get { return _datePicked; }
            set
            {
                _datePicked = value;
                NewHomeCreated.NextInspection = _datePicked.ToShortDateString();
                OnPropertyChanged("DatePicked");
            }
        }
        private bool _isProviderSelected;
        public bool IsProviderSelected
        {
            get { return _isProviderSelected; }
            set
            {
                _isProviderSelected = value;
                OnPropertyChanged("IsProviderSelected");
            }
        }

        private ProvidersModel _selectedProviderName;
        public ProvidersModel SelectedProviderName
        {
            get { return _selectedProviderName; }
            set
            {
                if (_selectedProviderName == value) return;
                _selectedProviderName = value;
                if (_selectedProviderName == null || _selectedProviderName.ProviderName.Equals(""))
                {
                    IsProviderSelected = false;
                }
                else
                {
                    IsProviderSelected = true;
                }
                OnPropertyChanged("SelectedProviderName");
            }
        }

        public NewHomeDialogVM()
        {
            IsProviderSelected = false;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                Providers = new List<ProvidersModel>();
                Providers.Add(new ProvidersModel("-1", "No Provider"));
                string listItem = "";
                foreach (var item in provs)
                {
                    listItem = item.Provider_ID + "-" + item.Provider_Name;
                    Providers.Add(new ProvidersModel(item.Provider_ID.ToString(), item.Provider_Name));
                }
                var lv = (ListCollectionView)CollectionViewSource.GetDefaultView(Providers);

                ComboBoxProviderItems = lv;
                lv.CustomSort = Comparer<ProvidersModel>.Create(ProviderSort);

                NewHomeCreated = new HomeModel();
                DatePicked = DateTime.Today;
            }
        }
        private int ProviderSort(ProvidersModel x, ProvidersModel y)
        {
            return GetDistance(x.ProviderName).CompareTo(GetDistance(y.ProviderName));
        }
        private int GetDistance(string provider)
        {
            if (string.IsNullOrWhiteSpace(TextSearch))
            {
                return 0;
            }

            string[] splitName = provider.Split(' ');

            string first = splitName[0];
            string last = splitName[splitName.Length - 1];

            first = first.Substring(0, Math.Min(first.Length, TextSearch.Length));
            last = last.Substring(0, Math.Min(last.Length, TextSearch.Length));

            return Math.Min(GetDistance(first, TextSearch), GetDistance(last, TextSearch));
        }

        //Taken from: https://github.com/dotnet/command-line-api/blob/master/src/System.CommandLine/Invocation/TypoCorrection.cs
        private static int GetDistance(string first, string second)
        {

            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            int n = first.Length;
            int m = second.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            int curRow = 0, nextRow = 1;
            int[][] rows = { new int[m + 1], new int[m + 1] };

            for (int j = 0; j <= m; ++j)
            {
                rows[curRow][j] = j;
            }

            for (int i = 1; i <= n; ++i)
            {
                rows[nextRow][0] = i;
                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] + (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            return rows[curRow][m];
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
