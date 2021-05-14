﻿using System;
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
            window.Show(out string account, out DateTime date);
            if (account == null || date == null)
                return;
            //add tab and add page depends on selected account (dictionary)
            var page = new Report101Page();
            page.DataContext = new Report10_1VM(page, date);
            AddItem("материальный отчет за \0" + date.Month + "\0"+ date.Year + "\0 по счёту \0" + account,
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
