using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;

namespace InfoPagesViewModels
{
    public class EmployeesInfoVM: BindableBase
    {
        #region constructor

        private EmployeesDB model;
        public EmployeesInfoVM()
        {
            addNewCommand = new DelegateCommand(AddNew);
            addCancelCommand = new DelegateCommand(AddCancel);
            model = new EmployeesDB();
            loadEmployeesCommand = new DelegateCommand(LoadEmployees);
            EmployeesRowClickCommand = new DelegateCommand(OnRowClick);
            
            // employees = model.GetList();
        }

        #endregion

        #region IsActiveLoadingIndicator

        private bool isActive = true;

        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value; 
                RaisePropertiesChanged();
            }
        }

        #endregion
        #region LoadEmployeesCommand

        public ICommand LoadEmployeesCommand
        {
            get => loadEmployeesCommand;
            set => loadEmployeesCommand = value;
        }
        private ICommand loadEmployeesCommand;

        public async void LoadEmployees()
        {
            await Task.Run(() => employees = model.GetActualList());

            RaisePropertyChanged(nameof(Employees));
            IsActive = false;
            RaisePropertyChanged(nameof(isActive));
        }
        #endregion
        #region Lists

        public List<string> Education
        {
            get => Lists.GetEducationList();
        }

        #region Employees

        private List<Employee> employees;

        public List<Employee> Employees
        {
            get => employees;
            set => employees = value;
        }

        #endregion
        #endregion
        #region Add

        #region AddName

        private string addName;

        public string AddName
        {
            get => addName;
            set
            {
                addName = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddLastName

        private string addLastName;

        public string AddLastName
        {
            get => addLastName;
            set
            {
                addLastName = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddMiddleName

        private string addMiddleName;

        public string AddMiddleName
        {
            get => addMiddleName;
            set
            {
                addMiddleName = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddDateBirth

        private DateTime addDateBirth = DateTime.Today;

        public DateTime AddDateBirth
        {
            get => addDateBirth;
            set
            {
                addDateBirth = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddAcceptableDate

        private DateTime addAcceptableDate = DateTime.Today;

        public DateTime AddAcceptableDate
        {
            get => addAcceptableDate;
            set
            {
                addAcceptableDate = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddEducation

        private int? addEducation;

        public int? AddEducation
        {
            get => addEducation;
            set
            {
                addEducation = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddNewCommand

        private ICommand addNewCommand;

        public ICommand AddNewCommand
        {
            get => addNewCommand;
            set => addNewCommand = value;
        }

        private void AddNew()
        {
            if (addName == string.Empty ||
                addLastName == string.Empty ||
                addMiddleName == string.Empty ||
                addDateBirth == null ||
                addAcceptableDate == null ||
                addEducation == null
            )
                return;
            var employee = new Employee()
            {
                Name = addName, LastName = addLastName, MiddleName = addMiddleName, DateBirth = (DateTime) addDateBirth,
                AcceptableDate = (DateTime) addAcceptableDate, Education = Education[(int)addEducation]
            };
            model.Add(employee);
            employees = model.GetActualList();
            RaisePropertiesChanged(nameof(employees));
            AddCancel();
        }

        #endregion

        #region AddCancelCommand;

        private ICommand addCancelCommand;

        public ICommand AddCancelCommand
        {
            get => addCancelCommand;
            set => addCancelCommand = value;
        }

        private void AddCancel()
        {
            AddName = null;
            AddMiddleName = null;
            AddLastName = null;
            AddDateBirth = DateTime.Today;
            AddAcceptableDate = DateTime.Today;
            AddEducation = null;
        }

        #endregion

        #endregion
        
        
           #region EditEmployee

        #region EditName

        private Employee selectedEmployee;

        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set => selectedEmployee = value;
        }

        #region EditName
        private string editName = string.Empty;

        public string EditName
        {
            get => editName;
            set => editName = value;
        }

        #endregion
        
        #region EditMiddleName
        private string editMiddleName = string.Empty;

        public string EditMiddleName
        {
            get => editMiddleName;
            set => editMiddleName = value;
        }

        #endregion

        #region EditLastName
        private string editLastName = string.Empty;

        public string EditLastName
        {
            get => editLastName;
            set => editLastName = value;
        }

        #endregion

        #region dateBirth

        private DateTime? editDateBirth;

        public DateTime? EditDateBirth
        { 
            get => editDateBirth;
            set => editDateBirth = value;
        }

        #endregion

        #region AcceptableDate

        private DateTime? editAcceptableDate;

        public DateTime? EditAcceptableDate
        {
            get => editAcceptableDate;
            set => editAcceptableDate = value;
        }

        #endregion

        #region education

        private int editEducation;

        public int EditEducation
        {
            get => editEducation;
            set => editEducation = value;
        }

        #endregion
      
        #region SaveChanges

        private ICommand saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get { return saveChangesCommand; }
            set { saveChangesCommand = value; }
        }

        private void SaveChanges()
        {

            if (editName != string.Empty && editLastName != string.Empty && editMiddleName != string.Empty && editAcceptableDate != null && editDateBirth != null && editEducation != -1)
            {
                var employee = new Employee()
                {
                    Name = editName, LastName = editLastName, MiddleName = editMiddleName,
                    Education = Education[editEducation], AcceptableDate = (DateTime) editAcceptableDate, DateBirth = (DateTime) editDateBirth
                };
                model.Edit(selectedEmployee.Id, employee);
                employees = model.GetActualList();
                RaisePropertyChanged(nameof(employees));
                EditCancel();
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
            editLastName = String.Empty;
            editMiddleName = String.Empty;
            editDateBirth = null;
            editAcceptableDate = null;
            editEducation = -1;

            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editLastName));
            RaisePropertyChanged(nameof(editMiddleName));
            RaisePropertyChanged(nameof(editDateBirth));
            RaisePropertyChanged(nameof(editAcceptableDate));
            RaisePropertyChanged(nameof(editEducation));

        }

        #endregion

        #region SaveAsNew

        private ICommand saveAsNewCommand;

        public ICommand SaveAsNewCommand
        {
            get { return saveAsNewCommand; }
            set { saveAsNewCommand = value; }
        }

        #region SearchName
        private string searchName = string.Empty;

        public string SearchName
        {
            get => searchName;
            set 
            {
            searchName = value;
            Search();
        }
        }

        #endregion
        
        #region SearchMiddleName
        private string searchMiddleName = string.Empty;

        public string SearchMiddleName
        {
            get => searchMiddleName;
            set
            {
            searchMiddleName = value;
            Search();
        }
        }

        #endregion

        #region SearchLastName
        private string searchLastName = string.Empty;

        public string SearchLastName
        {
            get => searchLastName;
            set
            {
            searchLastName = value;
            Search();
        }
        }

        #endregion

        #region dateBirth

        private DateTime? searchDateBirth;

        public DateTime? SearchDateBirth
        {
            get => searchDateBirth;
            set
            { 
                searchDateBirth = value;
                Search();
            }
        }

        #endregion

        #region AcceptableDate

        private DateTime? searchAcceptableDate;

        public DateTime? SearchAcceptableDate
        {
            get => searchAcceptableDate;
            set
            { 
                editAcceptableDate = value;
                Search();
            }
        }

        #endregion

        #region education

        private int searchEducation;

        public int SearchEducation
        {
            get => searchEducation;
            set
            {
                searchEducation = value;
                Search();
            }
        }

        #endregion
        private void SaveAsNew()
        {
            if (editName == string.Empty || editLastName == string.Empty || editMiddleName == string.Empty ||
                editAcceptableDate == null || editDateBirth == null || editEducation == -1) return;
            var employee = new Employee()
            {
                Name = editName,
                LastName = editLastName, 
                MiddleName = editMiddleName, 
                DateBirth = (DateTime) editDateBirth, 
                AcceptableDate = (DateTime) editAcceptableDate, 
                Education =  Education[editEducation]
            };
            model.Add(employee);
            employees = model.GetActualList();
            RaisePropertyChanged(nameof(employees));
            EditCancel();

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
            model.Delete(selectedEmployee);
            employees = model.GetActualList();
            RaisePropertyChanged(nameof(employees));
            EditCancel();
        }

        #endregion

        #endregion

        #endregion


        #region RowClick

        private void Search()
        {
            employees = model.Search(searchName, searchLastName, searchMiddleName);
            RaisePropertyChanged(nameof(employees));
        }

        public ICommand EmployeesRowClickCommand { get; set; }
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
        private void OnRowClick()
        {
            if (selectedEmployee == null) return;
            selectedTabIndex = 2;
            var employee = selectedEmployee;
            editName = employee.Name;
            editLastName = employee.LastName;
            editMiddleName = employee.MiddleName;
            editDateBirth = employee.DateBirth;
            editAcceptableDate = employee.AcceptableDate;
            editEducation = Education.IndexOf(employee.Education);
           
            RaisePropertyChanged(nameof(selectedEmployee));
            RaisePropertyChanged(nameof(selectedTabIndex));
            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editLastName));
            RaisePropertyChanged(nameof(editMiddleName));
            RaisePropertyChanged(nameof(editAcceptableDate));
            RaisePropertyChanged(nameof(editDateBirth));
            RaisePropertyChanged(nameof(editEducation));
        }
        #endregion
    }
}
