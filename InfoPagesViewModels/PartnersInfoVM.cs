using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;
using Timer = System.Timers.Timer;


namespace InfoPagesViewModels
{
    public class PartnersInfoVM : BindableBase
    {
        private readonly PartnerDB dataBase;
        private readonly IErrorAlert errorAlert;

        #region selectedTabIndex

        private int selectedTabIndex;

        public int SelectedTabIndex
        {
            get
            {
                editTabVisible = selectedTabIndex == 2;
                RaisePropertyChanged(nameof(editTabVisible));
                return selectedTabIndex;

            }
            set => selectedTabIndex = value;
        }

        #endregion

        #region editTabVisible

        private bool editTabVisible;

        public bool EditTabVisible

        {
            get { return editTabVisible; }
            set { editTabVisible = value; }

        }

        #endregion


        #region AddPartner

        #region AddName

        private string addName = string.Empty;

        public string AddName
        {
            set
            {
                addName = value;
            }
            get
            {
                return addName;
            }
        }
        #endregion

        #region AddUNP

        private string addUNP;
        public string AddUNP
        {
            set => addUNP = value;
            get => addUNP;
        }


        #endregion


        #endregion


        #region SearchPartner

        #region SearchName

        private string searchName = string.Empty;

        public string SearchName
        {
            get { return searchName; }
            set
            {
                nameTimer.Stop();
                nameTimer.Start();
                searchName = value;
            }
        }

        #endregion
        #region SearchUNP

        private string searchUNP;
        public string SearchUNP
        {
            set
            {
                searchUNP = value;
                Search();
            }
            get
            {
                return searchUNP;
            }
        }
        #endregion
        #endregion


        private readonly Timer nameTimer = new Timer() { Interval = 2000 };

        private async void SearchByName(object sender, EventArgs e)
        {
            nameTimer.Stop();
            partners = dataBase.GetList(); //why?
            if (searchName != string.Empty)
                await Task.Run(() => partners = (List<Partner>)dataBase.SearchByName(searchName));
            RaisePropertyChanged(nameof(partners));
        }

        //todo: make parallel

        #region SearchCancel


        private ICommand searchCancelCommand;

        public ICommand SearchCancelCommand
        {
            get => searchCancelCommand;
            set => searchCancelCommand = value;
        }

        private void SearchCancel()
        {
            searchName = string.Empty;
            searchUNP = string.Empty;
            RaisePropertyChanged(nameof(searchName));
            RaisePropertyChanged(nameof(searchUNP));
        }

        #endregion
        #region AddNew

        private ICommand addNewCommand;

        public ICommand AddNewCommand
        {
            get => addNewCommand;
            set => addNewCommand = value;
        }

        private void AddNew()
        {
            if (addName != string.Empty && addUNP.Replace(" ", string.Empty).Length == 9)
            {
                var partner = new Partner() { Name = addName, UNP = addUNP};
                dataBase.Add(partner);
                partners = dataBase.GetList();
                AddCancel();
                RaisePropertyChanged(nameof(partners));
            }
            else 
                errorAlert.ErrorAlert("Форма заполнена некорректно");
        }

        #endregion

        #region LoadingIndicator

        private bool isActive;
        public bool IsActive
        {
            get
            {
                RaisePropertyChanged(nameof(IsEnabled));
                return isActive;

            }
            set => isActive = value;
        }

        public bool IsEnabled => !isActive;

        #endregion

        #region LoadPartnersCommand

        public ICommand LoadPartnersCommand
        {
            get => loadPartnersCommand;
            set => loadPartnersCommand = value;
        }
        private ICommand loadPartnersCommand;

        public async void LoadPartners()
        {
            await Task.Run(() => Partners = dataBase.GetList());

            RaisePropertyChanged(nameof(Partners));
            isActive = false;
            RaisePropertyChanged(nameof(isActive));
        }
        #endregion

        #region SelectedPartner

        private Partner selectedPartner;

        public Partner SelectedPartner
        {
            get => selectedPartner;
            set
            {
                selectedPartner = value;
                RaisePropertyChanged(nameof(selectedPartner));
            }
        }

        #endregion

        #region AddCancel

        public ICommand AddCancelCommand { get; set; }

        private void AddCancel()
        {
            addName = string.Empty;
            addUNP = string.Empty;
            selectedPartner = null;
            RaisePropertyChanged(nameof(addName));
            RaisePropertyChanged(nameof(addUNP));
        }

        #endregion

        #region EditPartner

        #region EditName

        private string editName = string.Empty;

        public string EditName
        {
            get => editName;
            set => editName = value;
        }

        #endregion

        #region EditUNP

        private string editUNP;
        public string EditUNP
        {
            set
            {
                editUNP = value;
            }
            get
            {
                return editUNP;
            }
        }
      
        #region SaveChanges

        private ICommand saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get { return saveChangesCommand; }
            set { saveChangesCommand = value; }
        }

        private void SaveChanges()
        {

            if (editName != string.Empty && editUNP.Replace(" ", string.Empty).Length == 9)
            {
                var partner = new Partner() { Name = editName, UNP = editUNP};
                dataBase.Edit(selectedPartner.Id, partner);
                partners = dataBase.GetList();
                RaisePropertyChanged(nameof(partners));
                EditCancel();
            }
            else
            {
                errorAlert.ErrorAlert("Введите корректные значения для изменения");
            }
        }

        #endregion

        #region EditCancel

        private ICommand editCancelCommand;

        public ICommand EditCancelCommand
        {
            get { return editCancelCommand; }
            set { editCancelCommand = value; }
        }

        private void EditCancel()
        {
            editName = string.Empty;
            editUNP = String.Empty;

            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editUNP));

        }

        #endregion

        #region SaveAsNew

        private ICommand saveAsNewCommand;

        public ICommand SaveAsNewCommand
        {
            get { return saveAsNewCommand; }
            set { saveAsNewCommand = value; }
        }

        private void SaveAsNew()
        {
            if (editName != string.Empty && editUNP.Replace(" ", string.Empty).Length != 9)
            {
                var partner = new Partner()
                {
                    Name = editName,
                    UNP = editUNP,
                };
                dataBase.Add(partner);
                partners = dataBase.GetList();
                RaisePropertyChanged(nameof(partners));
                EditCancel();
            }
            else
            {
                errorAlert.ErrorAlert("Введите корректные значения");
            }
        }
        #endregion

        #region Delete

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set { deleteCommand = value; }
        }

        private void Delete()
        {
            dataBase.Delete(selectedPartner);
            partners = dataBase.GetList();
            RaisePropertyChanged(nameof(partners));
            EditCancel();
        }

        #endregion

        #endregion

        #endregion


        #region RowClick

        private void Search()
        {
            partners = dataBase.Search(searchName, searchUNP.Replace(" ", string.Empty));
            RaisePropertyChanged(nameof(partners));
        }

        public ICommand PartnersRowClickCommand { get; set; }

        private void OnRowClick()
        {
            if (selectedPartner == null) return;
            selectedTabIndex = 2;
            var partner = selectedPartner;
            editName = partner.Name;
            editUNP = partner.UNP;

            RaisePropertyChanged(nameof(selectedPartner));
            RaisePropertyChanged(nameof(selectedTabIndex));
            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editUNP));
        }
        #endregion

        #region construcor
        public PartnersInfoVM(IErrorAlert errorAlert)
        {
            saveChangesCommand = new DelegateCommand(SaveChanges);
            addNewCommand = new DelegateCommand(AddNew);
            PartnersRowClickCommand = new DelegateCommand(OnRowClick);
            AddCancelCommand = new DelegateCommand(AddCancel);
            loadPartnersCommand = new DelegateCommand(LoadPartners);
            nameTimer.Elapsed += SearchByName;
            editCancelCommand = new DelegateCommand(EditCancel);
            deleteCommand = new DelegateCommand(Delete);
            saveAsNewCommand = new DelegateCommand(SaveAsNew);
            searchCancelCommand = new DelegateCommand(SearchCancel);

            //partner = new MaterialsM(); 
            dataBase = new PartnerDB();
            IsActive = true;
            this.errorAlert = errorAlert;
        }
        #endregion

        private List<Partner> partners;

        public List<Partner> Partners
        {
            get => partners;
            set => partners = value;
        }
    }
}
