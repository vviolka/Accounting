using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Common;
using ReportPages;
using ReportPagesViewModels;

namespace MenuPagesViewModels
{
    public class ReportsPageViewModel : BaseViewModel
    {

        public ReportsPageViewModel()
        {
            this.Items = new ObservableCollection<TabItemViewModel>();
            openGenerateReportWindowCommand = new RelayCommand(OpenGenerateReportWindow);
        }

        #region OpenGenerateReportWindow

        private ICommand openGenerateReportWindowCommand;

        public ICommand OpenGenerateReportWindowCommand
        {
            get => openGenerateReportWindowCommand;
            set => openGenerateReportWindowCommand = value;
        }

        private void OpenGenerateReportWindow()
        {
            var window = new GenerateReportVM();
            DateTime date = DateTime.Today;
            window.Show(out string account, out date);
            if (account == null || date == null)
                return;
            var dc = new Report10_1VM(date, account);
            var page = new Report101Page(dc);
            page.DataContext = dc;
            AddItem($"материальный отчет за {Helpers.Monthes[new DateTime(date.Year, date.Month, date.Day).AddMonths(-1).Month]} {date.Year} по счёту {account}",
                page);
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
