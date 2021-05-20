using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;

namespace DocumentsPagesViewModels
{
    public class TTNVM: BindableBase
    {
        private readonly int billOfLadingId;
        public delegate void DeleteHandler();
        public delegate void UpdateHandler(string header);

        private MaterialsInfoDB materialsInfoDb;
        private IncomeDB incomeDb;
        private DeleteHandler notifier;
        private DeleteHandler updater;
        public TTNVM(int billOfLadingId, DeleteHandler notifier, DeleteHandler updater)
        {
            this.billOfLadingId = billOfLadingId;
            isActive = true;
            materialsInfoDb = new MaterialsInfoDB();
            incomeDb = new IncomeDB();
            loadManterialsCommand = new DelegateCommand(LoadMaterials);
            addNewCommand = new DelegateCommand(Add);
            addCancelCommand = new DelegateCommand(AddCancel);
            rowDoubleClickCommand = new DelegateCommand(RowDoubleClick);
            saveChangesCommand = new DelegateCommand(SaveChanges);
            deleteCommand = new DelegateCommand(Delete);
            editCancelCommand = new DelegateCommand(EditCancel);
            backEditCommand = new DelegateCommand(BackEdit);
            this.notifier = notifier;
            this.updater = updater;
        }
      
        #region TabControl

        #region IsEnabled

        public bool IsEnabled => !isActive;

        #endregion

        #region SelectedTabIndex

        private int selectedTabIndex;

        public int SelectedTabIndex
        {
            get
            {
                editTabVisible = selectedTabIndex == 1;
                RaisePropertyChanged(nameof(editTabVisible));
                return selectedTabIndex;
            }
            set => selectedTabIndex = value;
        }

        #endregion
        #endregion

        #region Lists

        #region Units
        public List<string> Units
        {
            get => (List<string>)Lists.GetUnits();
            set => Units = value;

        }
        #endregion

        #region Names

        private IEnumerable materials;

        
        public IEnumerable Materials
        {
            get => materials;
            set => materials = value;
        }

        #endregion

        #region Accounts
        private List<string> accounts = (List<string>)Lists.GetMaterialsAccounts();

        public List<string> Accounts
        {
            get => accounts;
        }

        #endregion
        #endregion
        
        #region Result

        #region CostWithVat

        private float resultCost;

        public float ResultCost
        {
            get => resultCost;
            set => resultCost = value;
        }

        #endregion

        #region ResultVat

        private float resultVat;

        public float ResultVat
        {
            get => resultVat;
            set => resultVat = value;
        }

        #endregion

        #region ResultCostWithVat

        private float resultCostWithVat;

        public float ResultCostWithVat
        {
            get => resultCostWithVat;
            set => resultCostWithVat = value;
        }

        #endregion

        private void UpdateResultValues()
        {
            var billOfLading = new BillOfLadingDB().GetBillOfLading(billOfLadingId);
            resultCost = billOfLading.Cost;
            resultVat = billOfLading.Vat;
            resultCostWithVat = billOfLading.CostWithVat;
            RaisePropertyChanged(nameof(resultCost));
            RaisePropertyChanged(nameof(resultVat));
            RaisePropertyChanged(nameof(resultCostWithVat));
        }
        #endregion
        
        #region AddMaterial
        #region AddName

        private string addName;

        public string AddName
        {
            get => addName;
            set => addName = value;
        }

        #endregion
        
        #region AddUnity

        private int addUnity;
        public int AddUnity
        {
            set => addUnity = value;
            get => addUnity;
        }


        #endregion
        
        #region AddCount

        private float? addCount;

        public float? AddCount
        {
            get => addCount;
            set => addCount = value;
        }

        #endregion
        
        #region AddCost

        private float? addCost;

        public float? AddCost
        {
            get => addCost;
            set => addCost = value;
        }

        #endregion
        
        #region AddVatRate

        private string addVatRate;

        public string AddVatRate
        {
            get => addVatRate;
            set => addVatRate = value;
        }

        #endregion
        
        #region AddVat

        private float? addVat;

        public float? AddVat
        {
            get => addVat;
            set => addVat = value;
        }

        #endregion
        
        #region AddWeight

        private float? addWeight;

        public float? AddWeight
        {
            get => addWeight;
            set => addWeight = value;
        }

        #endregion

        #region AddAcccount

        private int addAccount;

        public int AddAccount
        {
            get => addAccount;
            set => addAccount = value;
        }

        #endregion
       
        #region Add

        private ICommand addNewCommand;

        public ICommand AddNewCommand
        {
            get => addNewCommand;
            set => addNewCommand = value;
        }

        private void Add()
        {
            if (addName == string.Empty|| addCost == null || addCount == null ||
                addVatRate == string.Empty || addVat == null ) 
                return;
            var materialsInfo = new MaterialsInfo()
            {
                Name = addName, Unity = Units[addUnity], Account = accounts[addAccount]
            };
            int materialsInfoId = materialsInfoDb.Add(materialsInfo);
            var income = new Income()
            {
                MaterialId = materialsInfoId,
                Count = (float)addCount, Cost = (float)addCost,
                VatRate = addVatRate, Vat = (float)addVat,
                BillId = billOfLadingId, Weight = addWeight
            };
            incomeDb.Add(income);
            
            AddCancel();
            UpdateResultValues();
            GetJoinedList();
        }
        #endregion

        #region AddCancel

        private ICommand addCancelCommand;

        public ICommand AddCancelCommand
        {
            get => addCancelCommand;
            set => addCancelCommand = value;
        }

        private void AddCancel()
        {
            addName = String.Empty;
            addUnity = 0;
            addCount = null;
            addCost = null;
            addVatRate = string.Empty;
            addVat = null;
            addWeight = null;
            addAccount = 0;
            
            
            RaisePropertyChanged(nameof(addName));
            RaisePropertyChanged(nameof(addUnity));
            RaisePropertyChanged(nameof(addCount));
            RaisePropertyChanged(nameof(addCost));
            RaisePropertyChanged(nameof(addVatRate));
            RaisePropertyChanged(nameof(addVat));
            RaisePropertyChanged(nameof(addWeight));
            RaisePropertyChanged(nameof(addAccount));
        }

        #endregion
        #endregion

        /*#region EditSelectedMaterial

        private int editSelectedMaterial;

        public int EditSelectedMaterial
        {
            get => editSelectedMaterial;
            set => editSelectedMaterial = value;
        }

        #endregion*/
        #region EditMaterial

        #region EditTabVisible

        private bool editTabVisible;

        public bool EditTabVisible
        {
            get => editTabVisible;
            set => editTabVisible = value;
        }

        #endregion

        #region EditName

        private string editName;

        public string EditName
        {
            get => editName;
            set => editName = value;
        }

        #endregion

        #region EditUnity

        private int editUnity;

        public int EditUnity
        {
            get => editUnity;
            set => editUnity = value;
        }

        #endregion

        #region EditCount

        private float? editCount;

        public string EditCount
        {
            get => editCount.ToString();
            set => editCount = float.Parse(value);
        }

        #endregion

        #region EditCost

        private float? editCost;

        public float? EditCost
        {
            get => editCost;
            set => editCost = value;
        }

        #endregion

        #region EditVatRate

        private string editVatRate;

        public string EditVatRate
        {
            get => editVatRate;
            set => editVatRate = value;
        }

        #endregion
        #region EditVat

        private float? editVat;

        public float? EditVat
        {
            get => editVat;
            set => editVat = value;
        }

        #endregion

        #region EditWeight

        private float? editWeight = 300;
        public float? EditWeight
        {
            get => editWeight;
            set => editWeight = value;
        }

        #endregion

        #region EditAccount

        private int editAccount;

        public int EditAccount
        {
            get => editAccount;
            set => editAccount = value;
        }

        #endregion
        #region SaveChangesCommand

        private ICommand saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get => saveChangesCommand;
            set => saveChangesCommand = value;
        }
        private void SaveChanges()
        {
            if (editName == String.Empty || editUnity == 0 || editCount == null ||
                editCost == null || editVatRate == string.Empty || editVat == null || editAccount == 0)
                return;
            var newMaterials = new MaterialsInfo()
            {
                Name = editName,
                Unity = Units[editUnity],
                Account = accounts[editAccount]
            };
            var newIncome = new Income()
            {
                Count = (float) editCount,
               Cost =  (float) editCost,
               VatRate =  editVatRate,
               Vat = (float) editVat,
               Weight = editWeight
            };
            materialsInfoDb.Edit(materialsInfo.Id, newMaterials);
            incomeDb.Edit(income.Id, newIncome);
            GetJoinedList();
            EditCancel();
            UpdateResultValues();
        }

        #endregion

        #region DeleteCommand

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get => deleteCommand;
            set => deleteCommand = value;
        }

        private void Delete()
        {
            materialsInfoDb.Delete(materialsInfo);
            GetJoinedList();
            RaisePropertyChanged(nameof(materials));
            EditCancel();
            UpdateResultValues();

        }
        #endregion

        #region EditCancelCommand

        private ICommand editCancelCommand;

        public ICommand EditCancelCommand
        {
            get => editCancelCommand;
            set => editCancelCommand = value;
        }

        private void EditCancel()
        {
            editName = String.Empty;
            editUnity = 0;
            editCount = null;
            editCost = null;
            editVatRate = string.Empty;
            editVat = null;
            editWeight = null;
            editAccount = 0;
            
            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editUnity));
            RaisePropertyChanged(nameof(editCount));
            RaisePropertyChanged(nameof(editCost));
            RaisePropertyChanged(nameof(editVatRate));
            RaisePropertyChanged(nameof(editVat));
            RaisePropertyChanged(nameof(editWeight));
            RaisePropertyChanged(nameof(editAccount));
        }

        #endregion

        

        #endregion

        #region RowDobuleClickCommand

        private ICommand rowDoubleClickCommand;
        
        public ICommand RowDoubleClickCommand
        {
            get => rowDoubleClickCommand;
            set => rowDoubleClickCommand = value;
        }

        #region SelectedMaterial

        private int selectedMaterial;

        public int SelectedMaterial
        {
            get => selectedMaterial;
            set => selectedMaterial = value;
        }

        #endregion

        private Income income;
        private MaterialsInfo materialsInfo;
        private void RowDoubleClick()
        {
            if (selectedMaterial == -1) return;
            selectedTabIndex = 1;
            income = incomeDb.GetList(billOfLadingId)[selectedMaterial];
            materialsInfo = materialsInfoDb.GetList().First(m => m.Id == income.MaterialId);
            editName = materialsInfo.Name;
            editUnity = Units.IndexOf(materialsInfo.Unity);
            editCount = income.Count;
            editCost = income.Cost;
            editVatRate = income.VatRate;
            editVat = income.Vat;
            editWeight = income.Weight;
            editAccount = accounts.IndexOf(materialsInfo.Account);
             
            RaisePropertyChanged(nameof(selectedTabIndex));
            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editUnity));
            RaisePropertyChanged(nameof(editCount));
            RaisePropertyChanged(nameof(editCost));
            RaisePropertyChanged(nameof(editVatRate));
            RaisePropertyChanged(nameof(editVat));
            RaisePropertyChanged(nameof(editWeight));
            RaisePropertyChanged(nameof(editAccount));
        }

        #endregion


        #region BackEditCommand

        private ICommand backEditCommand;

        public ICommand BackEditCommand
        {
            get => backEditCommand;
            set => backEditCommand = value;
        }

        private void BackEdit()
        {
            var window = new CreateBillOfLadingVM(new BillOfLadingDB().GetBillOfLading(billOfLadingId));
            var billOfLading = window.Show();
            var db = new BillOfLadingDB();
            if (billOfLading != null)
            {
                db.Edit(billOfLadingId, billOfLading);
                updater?.Invoke();
            }
            else
            {
                db.Delete(billOfLadingId);
                notifier?.Invoke();
            }
        }
        #endregion

        #region LoadIndicator

        private bool isActive;

        public bool IsActive
        {
            get
            {
                // RaisePropertyChanged(nameof(IsEnabled));
                return isActive;
            }
            set => isActive = value;
        }

        #endregion

        #region LoadMaterials

        private ICommand loadManterialsCommand;
        public ICommand LoadMaterialsCommand
        {
            get => loadManterialsCommand;
            set => loadManterialsCommand = value;
        }

        private void LoadMaterials()
        {
            GetJoinedList();
            isActive = false;
            UpdateResultValues();
            RaisePropertyChanged(nameof(materials));
            RaisePropertyChanged(nameof(isActive));
            RaisePropertyChanged(nameof(IsEnabled));
        }
        #endregion

        public List<string> MaterialsName
        {
            get => materialsInfoDb.GetNames();
        }

        private void GetJoinedList()
        {
            materials = incomeDb.GetList(billOfLadingId).Join(
                materialsInfoDb.GetList(),
                i => i.MaterialId, m => m.Id,
                (i, m) => new MaterialsJoined()
                {
                    Name = m.Name,
                    Unity = m.Unity,
                    Count = i.Count,
                    Cost = i.Cost,
                    Price = m.Price,
                    VatRate = i.VatRate,
                    Vat = i.Vat,
                    CostWithVat = i.CostWithVat,
                    Weight = i.Weight,
                    Account = m.Account

                }).ToList();
            RaisePropertyChanged(nameof(materials));
        }
    }
}