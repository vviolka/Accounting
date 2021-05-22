using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Common;
using DevExpress.Mvvm;
using ReportCard = Model.ReportCard;

namespace SalaryPagesViewModels
{
    public class SalaryViewVM : BindableBase
    {
        public SalaryViewVM()
        {
            this.Items = new ObservableCollection<TabItemViewModel>();
            openAddCardRepordWindowCommand = new RelayCommand(OpenGenerateReportWindow);
        }

        #region OpenGenerateReportWindow

        private ICommand openAddCardRepordWindowCommand;

        public ICommand OpenAddCardRepordWindowCommand
        {
            get => openAddCardRepordWindowCommand;
            set => openAddCardRepordWindowCommand = value;
        }

        private void OpenGenerateReportWindow()
        {
            var window = new AddReportVM();
            window.Show(out DateTime date);
            if (date == null)
                return;
            //add tab and add page depends on selected account (dictionary)
            var page = new SalaryPages.ReportCard();
            page.DataContext = new ReportCardVM(date);
            AddItem("материальный отчет за \0" + date.Month + " " + date.Year + "\0 по счёту \0",
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