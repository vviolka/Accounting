using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;

namespace SalaryPagesViewModels
{
   public class SalariesVM:BindableBase
    {
        public SalariesVM()
        {
            this.Items = new ObservableCollection<TabItemViewModel>();
            openAddSalaryWindowCommand = new RelayCommand(OpenGenerateReportWindow);
        }

        #region OpenGenerateReportWindow

        private ICommand openAddSalaryWindowCommand;

        public ICommand OpenAddSalaryWindowCommand
        {
            get => openAddSalaryWindowCommand;
            set => openAddSalaryWindowCommand = value;
        }

        private void OpenGenerateReportWindow()
        {
            var window = new AddReportVM();
            window.Show(out DateTime date);
            if (date == null)
                return;
            //add tab and add page depends on selected account (dictionary)
            var page = new SalaryPages.SalaryPage();
            page.DataContext = new SalaryVM(date);
            AddItem($"зарплата за {Helpers.Monthes[new DateTime(date.Year, date.Month, date.Day).AddMonths(-1).Month].ToLower()} {date.Year}", page);
        }

        #endregion

        public ObservableCollection<TabItemViewModel> Items { get; private set; }

        public TabItemViewModel SelectedItem { get; set; }

        public RelayCommand AddCommand { get; set; }






        public void AddItem(string header, object content)
        {
            var nextIndex = this.Items.Count + 1;
            var newItem = new TabItemViewModel(header, content, this.CloseItem);
            this.Items.Add(newItem);
            this.SelectedItem = newItem;
            RaisePropertyChanged("SelectedItem");
        }

        private void CloseItem(TabItemViewModel vm)
        {
            this.Items.Remove(vm);
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
