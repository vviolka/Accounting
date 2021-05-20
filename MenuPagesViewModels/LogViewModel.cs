using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using LogPages;
using LogPagesViewModels;

namespace MenuPagesViewModels
{
    class LogViewModel: BaseViewModel
    {
        public LogViewModel()
        {
            this.Items = new ObservableCollection<TabItemViewModel>();
            openGenerateLogWindowCommand = new RelayCommand(OpenGenerateLogWindow);
        }

        #region OpenGenerateLogWindow

        private ICommand openGenerateLogWindowCommand;

        public ICommand OpenGenerateLogWindowCommand
        {
            get => openGenerateLogWindowCommand;
            set => openGenerateLogWindowCommand = value;
        }

        private void OpenGenerateLogWindow()
        {
            var window = new GenerateLogVM();
            window.Show(out string account, out DateTime date);
            if (account == null || date == null)
                return;
            var dc = new Log60_1VM(date, account);
            var page = new Log60_1Page();
            page.DataContext = dc;
            AddItem($"журнал-ордер за {Helpers.Monthes[date.Month]} {date.Year} по счёту 60/1",
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
