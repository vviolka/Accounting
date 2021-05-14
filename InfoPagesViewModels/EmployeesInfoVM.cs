using System;
using System.Collections.Generic;
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
    }
}
