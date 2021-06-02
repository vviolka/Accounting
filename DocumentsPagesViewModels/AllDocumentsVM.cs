using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DocumentsPages;
using Model;
using Model.DBStructure;

namespace DocumentsPagesViewModels
{
    public class AllDocumentsVM: BindableBase
    {
        private AllDocuments window;
        private readonly BillOfLadingDB dataBase;
        private string type;
        private int id;

        #region constructor

        public AllDocumentsVM()
        {
            dataBase = new BillOfLadingDB();
            documentsDate = DateTime.Today;
            loadAllDocumentsCommand = new DelegateCommand(LoadAllDocuments);
            addNewDocumentCommand = new DelegateCommand(AddNewDocument);
            documentsRowClickCommand = new DelegateCommand(DocumentsRowClick);
            

        }

        #endregion
        #region Fields
        #region Types

        private List<string> types = (List<string>) Lists.GetDocumentTypes();

        public List<string> Types
        {
            get => types;
            set => types = value;
        }


        #endregion

        #region SelectedTypeCreate

        private int selectedTypeCreate;

        public int SelectedTypeCreate
        {
            get => selectedTypeCreate;
            set => selectedTypeCreate = value;
        }

        #endregion

        #region DocumentsDate

        private DateTime documentsDate;
        public DateTime DocumentsDate
        {
            get => documentsDate;
            set  {
            documentsDate = value;
            Search();
            }
        }


        #endregion

        #region SelectedTypeSearch

        private int? selectedTypeSearch;
        public int? SelectedTypeSearch
        {
            get => selectedTypeSearch;
            set {
                selectedTypeSearch = value;
                Search();
        }
        }

        #endregion

        #region DocumentsNumber


        private string documentsNumber;
        public string DocumentsNumber
        {
            get => documentsNumber;
            set  {
            documentsNumber = value;
            Search();
        }
        }

        #endregion

        #region Partners

        private List<Partner> partners = new PartnerDB().GetList();

        public List<Partner> Partners
        {
            get => partners;
            set => partners = value;
        }

        #endregion

        #region SelectedPartner

        private int selectedPartner = 0;

        public int SelectedPartner
        {
            get => selectedPartner;
            set  {
            selectedPartner = value;
            Search();
        }
        }

        #endregion

        #region Documents
        private List<AdaptedDocument> documents;

        public List<AdaptedDocument> Documents
        {
            get => documents;
            set => documents = value;
        }

        #endregion

        #region AddNewBillOfLading

        private ICommand addNewDocumentCommand;

        public ICommand AddNewDocumentCommand
        {
            get => addNewDocumentCommand;
            set => addNewDocumentCommand = value;
        }

        private void AddNewDocument()
        {
            id = -1;
            type = types[selectedTypeCreate];
            window.Close();
            
        }

        #endregion

        #region SelectedDocument

        private AdaptedDocument selectedDocument;

        public AdaptedDocument SelectedDocument
        {
            get => selectedDocument;
            set => selectedDocument = value;
        }

        #endregion
        #region OpenDocument

        private ICommand documentsRowClickCommand;
        public ICommand DocumentsRowClickCommand
        {
            get => documentsRowClickCommand;
            set => documentsRowClickCommand = value;
        }

        private void DocumentsRowClick()
        {
            id = selectedDocument.Id;
            window.Close();
        }

        #endregion
        #endregion
        private void Search()
        {
            if (selectedPartner == -1)
                return;
            documents = dataBase.Search(documentsDate,
                selectedTypeSearch == 0 ? "" : Types[(int) selectedTypeSearch],
                documentsNumber == null ? "" : documentsNumber.Replace(" ", string.Empty),
                selectedPartner == 0 ? "" : Partners[selectedPartner].UNP);
            RaisePropertyChanged(nameof(documents));
        }
        public void Show(out string? docType, out int docId)
        {
            window = new AllDocuments(){DataContext = this};
            window.ShowDialog();
            docType = this.type;
            docId = this.id;
            
        }

        #region LoadDocuments

        private ICommand loadAllDocumentsCommand;
        public ICommand LoadAllDocumentsCommand
        {
            get => loadAllDocumentsCommand;
            set => loadAllDocumentsCommand = value;
        }
        public void LoadAllDocuments()
        {
            documents = dataBase.GetJoinedList();
            RaisePropertyChanged(nameof(documents));
        }

        #endregion

    }
}
