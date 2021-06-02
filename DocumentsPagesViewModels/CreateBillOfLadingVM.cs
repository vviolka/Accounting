using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DocumentsPages;
using Model;
using Model.DBStructure;

namespace DocumentsPagesViewModels
{
    public class CreateBillOfLadingVM:BindableBase
    {
        private CreateTTN window;
        private BillOfLading billOfLading;
        public CreateBillOfLadingVM()
        {
            createCommand = new DelegateCommand(Create);
            createVisibility = true;
            documentsDate = DateTime.Today;
        }
        public CreateBillOfLadingVM(BillOfLading billOfLading)
        {
            this.billOfLading = billOfLading;
            documentsDate = this.billOfLading.Date;
            documentsNumber = billOfLading.Number;
            selectedPartner = partners.IndexOf(partners.First(p => p.Id == billOfLading.PartnerId));

            deleteCommand = new DelegateCommand(Delete);
            editCommand = new DelegateCommand(Edit);
            editVisibility = true;
            RaisePropertyChanged(nameof(documentsDate));
            RaisePropertyChanged(nameof(documentsNumber));
            RaisePropertyChanged(nameof(selectedPartner));
            documentsDate = DateTime.Today;

        }
        #region DocumentsDate

        private DateTime documentsDate;
        public DateTime DocumentsDate
        {
            get => documentsDate.Date;
            set => documentsDate = value;
        }

        #endregion
        #region DocumentsName

        private string documentsName = "ТТН";
        public string DocumentsName
        {
            get => documentsName;
            set => documentsName = value;
        }

        #endregion

        #region DocumentsNumber


        private string documentsNumber;
        public string DocumentsNumber
        {
            get => documentsNumber;
            set => documentsNumber = value;
        }

        #endregion

        #region Partners

        private List<Partner> partners = new PartnerDB().GetList();

        public List<string> Partners
        {
            get => partners.Select(p => p.Name).ToList();
        }

        #endregion

        #region SelectedPartner

        private int selectedPartner;

        public int SelectedPartner
        {
            get => selectedPartner;
            set => selectedPartner = value;
        }

        #endregion

        #region VatsDeduction

        private bool vatsDeduction;
        public bool VatsDeduction
        {
            get => vatsDeduction;
            set
            {
                vatsDeduction = value;
                deductionsVisibility = vatsDeduction;
                RaisePropertyChanged(nameof(deductionsVisibility));
            }
        }

        #endregion

        #region DeductionsDate

        private DateTime deductionsDate;

        public DateTime DeductionsDate
        {
            get => deductionsDate;
            set => deductionsDate = value;
        }

        #endregion

        #region DeductionsVisibility

        private bool deductionsVisibility;
        public bool DeductionsVisibility
        {
            get => deductionsVisibility;
            set => deductionsVisibility = value;
        }

        #endregion

        #region CreateVisibility

        private bool createVisibility;
        public bool CreateVisibility
        {
            get => createVisibility;
            set => createVisibility = value;
        }

        #endregion

        #region EditVisibility

        private bool editVisibility;
        public bool EditVisibility
        {
            get => editVisibility;
            set => editVisibility = value;
        }

        #endregion

        #region CreateDocument

        private ICommand createCommand;

        public ICommand CreateCommand
        {
            get => createCommand;
            set => createCommand = value;
        }
        private void Create()
        {
            int partnerId = partners[selectedPartner].Id;
            billOfLading = new BillOfLading()
                {Date = documentsDate.Date, Number = documentsNumber, PartnerId = partnerId, Type = documentsName};
            window.Close();
        }

        #endregion

        #region Delete

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get => deleteCommand;
            set => deleteCommand = value;
        }

        private void Delete()
        {
            billOfLading = null;
            window.Close();
        }

        #endregion

        #region Edit

        private ICommand editCommand;

        public ICommand EditCommand
        {
            get => editCommand;
            set => editCommand = value;
        }

        private void Edit()
        {
            billOfLading.Date = documentsDate;
            billOfLading.Number = documentsNumber;
            billOfLading.PartnerId = partners[selectedPartner].Id;
            window.Close();
        }

        #endregion
        public BillOfLading Show()
        {
            window = new CreateTTN(){DataContext = this};
            window.ShowDialog();
            return billOfLading;

        }
    }
}
