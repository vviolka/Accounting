using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;
using Timer = System.Timers.Timer;


namespace SalaryPagesViewModels
{
    public class VacationsPageVM : BindableBase
    {
        private readonly VacationsDB dataBase;

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


        #region AddVacation

        #region AddName

        private int addName;

        public int AddName
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
        
        #region AddStartDate

        private DateTime addStartDate = DateTime.Now;

        public DateTime AddStartDate
        {
            set
            {
                addStartDate = value;
            }
            get
            {
                return addStartDate;
            }
        }
        #endregion
        
        #region AddEndDate

        private DateTime addEndDate = DateTime.Now;

        public DateTime AddEndDate
        {
            set
            {
                addEndDate = value;
            }
            get
            {
                return addEndDate;
            }
        }
        #endregion
        
        
        #region AddDaysCount

        private int addDaysCount;

        public int AddDaysCount
        {
            set
            {
                addDaysCount = value;
            }
            get
            {
                return addDaysCount;
            }
        }
        #endregion

       
        #endregion


        #region SearchVacation

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
            // nameTimer.Stop();
            // vacations = dataBase.GetList(); //why?
            // if (searchName != string.Empty)
            //     await Task.Run(() => vacations = (List<Vacation>)dataBase.SearchByName(searchName));
            // RaisePropertyChanged(nameof(vacations));
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
            if (employees[addName] != null && addDaysCount > 0 && addDaysCount <= (addEndDate - addStartDate).Days + 1)
            {
                var vacation = new Vacation() {DaysCount = addDaysCount, EmployeeId = employees[addName].Id, StartDate = addStartDate, EndDate = addEndDate};
                dataBase.Add(vacation);
                vacations = dataBase.GetList();
                AddCancel();
                RaisePropertyChanged(nameof(vacations));
            }
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

        #region LoadVacationsCommand

        public ICommand LoadVacationsCommand
        {
            get => loadVacationsCommand;
            set => loadVacationsCommand = value;
        }
        private ICommand loadVacationsCommand;

        public async void LoadVacations()
        {
            await Task.Run(() => Vacations = dataBase.GetList());

            RaisePropertyChanged(nameof(Vacations));
            isActive = false;
            RaisePropertyChanged(nameof(isActive));
        }
        #endregion

        #region SelectedVacation

        private int selectedVacation;

        public int SelectedVacation
        {
            get => selectedVacation;
            set
            {
                selectedVacation = value;
                RaisePropertyChanged(nameof(selectedVacation));
            }
        }

        #endregion

        #region AddCancel

        public ICommand AddCancelCommand { get; set; }

        private void AddCancel()
        {
            addName = -1;
            addDaysCount = 0;
            addEndDate = DateTime.Now;
            addStartDate = DateTime.Now;
            selectedVacation = 0;
            RaisePropertyChanged(nameof(addName));
            RaisePropertyChanged(nameof(addDaysCount));
            RaisePropertyChanged(nameof(addStartDate));
            RaisePropertyChanged(nameof(addEndDate));
        }

        #endregion

        #region EditVacation

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

            /*if (editName != string.Empty && editUNP.Replace(" ", string.Empty).Length != 9)
            {
                var vacation = new Vacation() { Name = editName, UNP = editUNP};
                dataBase.Edit(vacations[selectedVacation].Id, vacation);
                vacations = dataBase.GetList();
                RaisePropertyChanged(nameof(vacations));
                EditCancel();
            }*/
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
                var vacation = new Vacation()
                {
                    // Name = editName,
                    // UNP = editUNP,
                };
                dataBase.Add(vacation);
                vacations = dataBase.GetList();
                RaisePropertyChanged(nameof(vacations));
                EditCancel();
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
            // dataBase.Delete(vacations[selectedVacation]);
            // vacations = dataBase.GetList();
            RaisePropertyChanged(nameof(vacations));
            EditCancel();
        }

        #endregion

        #endregion

        #endregion


        #region RowClick

        private void Search()
        {
            // vacations = dataBase.Search(searchName, searchUNP.Replace(" ", string.Empty));
            // RaisePropertyChanged(nameof(vacations));
        }

        public ICommand VacationsRowClickCommand { get; set; }

        private void OnRowClick()
        {
            if (selectedVacation == -1) return;
            selectedTabIndex = 2;
            var vacation = vacations[selectedVacation];
            // editName = vacation.Name;
            // editUNP = vacation.UNP;

            RaisePropertyChanged(nameof(selectedVacation));
            RaisePropertyChanged(nameof(selectedTabIndex));
            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editUNP));
        }
        #endregion

        #region construcor
        public VacationsPageVM()
        {
            employees = new EmployeesDB().GetActualList();
            saveChangesCommand = new DelegateCommand(SaveChanges);
            addNewCommand = new DelegateCommand(AddNew);
            VacationsRowClickCommand = new DelegateCommand(OnRowClick);
            AddCancelCommand = new DelegateCommand(AddCancel);
            loadVacationsCommand = new DelegateCommand(LoadVacations);
            nameTimer.Elapsed += SearchByName;
            editCancelCommand = new DelegateCommand(EditCancel);
            deleteCommand = new DelegateCommand(Delete);
            saveAsNewCommand = new DelegateCommand(SaveAsNew);
            searchCancelCommand = new DelegateCommand(SearchCancel);

            //vacation = new MaterialsM(); 
            dataBase = new VacationsDB();
            IsActive = true;
        }
        #endregion

        #region Lists

        private List<Employee> employees;

        public List<string> Names
        {
            get => employees.Select(x => $"{x.LastName} {x.Name} {x.MiddleName}").ToList();
        }

        #endregion
        private List<AdaptedVacation> vacations;

        public List<AdaptedVacation> Vacations
        {
            get => vacations;
            set => vacations = value;
        }
    }
}