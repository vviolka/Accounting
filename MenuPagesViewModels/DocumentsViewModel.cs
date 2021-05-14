﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Common;
using DocumentsPages;
using DocumentsPagesViewModels;
using Model;

namespace MenuPagesViewModels
{
    public class DocumentsViewModel:BaseViewModel
    {
        private int billOfLadingId;
        public DocumentsViewModel(IShowWindow window)
        {
            this.Items = new ObservableCollection<TabItemViewModel>();
            
            OpenAllDocsCommand = new RelayCommand(OpenAllDocs);
            OnDelete += () =>
            {
                CloseItem(SelectedItem);
            };
            OnEdit += () =>
            {
                CloseItem(SelectedItem);
                var billOfLading = new BillOfLadingDB().GetBillOfLading(billOfLadingId);

                AddItem(billOfLading.Type + " № " + billOfLading.Number
                        + "от" + billOfLading.Date.Date, new TTN() { DataContext = new TTNVM(billOfLading.Id, OnDelete, OnEdit) });
                RaisePropertyChanged(nameof(SelectedItem.Header));
            };

        }

        public ObservableCollection<TabItemViewModel> Items { get; private set; }

        public TabItemViewModel SelectedItem { get; set; }

        public RelayCommand AddCommand { get; set; }

        #region OpenAllDocs

        private ICommand openAllDocsCommand;

        public ICommand OpenAllDocsCommand
        {
            get => openAllDocsCommand;
            set => openAllDocsCommand = value;
        }

        private void OpenAllDocs()
        {
            string type;
            int id = -1;
            var allDocuments = new AllDocumentsVM();
            allDocuments.Show(out type, out id);
            if (type == string.Empty)
                return;
            if (id == -1)
                CreateNewDocument();
            else
                OpenDocument(id);

        }
        private void OpenDocument(int id)
        {
            var billOfLading = new BillOfLadingDB().GetBillOfLading(id);

            if (billOfLading == null)
                return;
            billOfLadingId = billOfLading.Id;
            AddItem(billOfLading.Type + " № " + billOfLading.Number
                    + "от" + billOfLading.Date.Date, new TTN() { DataContext = new TTNVM(billOfLading.Id, OnDelete, OnEdit) });

        }
        private void CreateNewDocument()
        {
            var window = new CreateBillOfLadingVM();
            var billOfLading = window.Show();
            if (billOfLading == null)
                return;

            billOfLadingId = new BillOfLadingDB().Add(billOfLading);

            AddItem(billOfLading.Type + " № " + billOfLading.Number
                    + "от" + billOfLading.Date.Date, new TTN() { DataContext = new TTNVM(billOfLading.Id, OnDelete, OnEdit) });

        }
        #endregion
        public event TTNVM.DeleteHandler OnDelete;
        public event TTNVM.DeleteHandler OnEdit;
      
        
        
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

        // public event PropertyChangedEventHandler PropertyChanged;

    }

 
}
