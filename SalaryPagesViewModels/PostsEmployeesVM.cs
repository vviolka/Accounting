using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;

namespace SalaryPagesViewModels
{
    public class PostsEmployeesVM: BindableBase
    {
        #region constructor

        private PostsEmployeesDB model;
        public PostsEmployeesVM()
        {
            model = new PostsEmployeesDB();
            addNewCommand = new DelegateCommand(AddNew);
            addCancelCommand = new DelegateCommand(AddCancel);
            employeesPosts = model.GetAdaptedList();
        }

        #endregion

        #region Lists

        private List<Post> posts = new PostDB().GetList();

        public List<string> Posts => new PostDB().GetList().Select(x => x.Name).ToList();

        private List<Employee> employees = new EmployeesDB().GetActualList();

        public List<string> Employees => new EmployeesDB().GetActualList().Select(x => x.LastName + " " + x.Name + " " + x.MiddleName).ToList();
        private IEnumerable employeesPosts;

        public IEnumerable EmployeesPosts
        {
            get => employeesPosts;
            set
            {
                employeesPosts = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region Add

        #region AddEmployee

        private int? addSelectedEmployee;

        public int? AddSelectedEmployee
        {
            get => addSelectedEmployee;
            set
            {
                addSelectedEmployee = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddPost

        private int? addSelectedPost;

        public int? AddSelectedPost
        {
            get => addSelectedPost;
            set
            {
                addSelectedPost = value;
                RaisePropertiesChanged();
            }
        }

        #endregion

        #region AddBid

        private float? addBid;

        public float? AddBid
        {
            get => addBid;
            set
            {
                addBid = value;
                RaisePropertiesChanged();
            }
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
            if (addSelectedEmployee == null || addSelectedPost == null || addBid == null)
                return;
            var employeePost = new EmployeePost()
            {
                EmployeeId = employees[(int) addSelectedEmployee].Id,
                PostId = posts[(int) addSelectedPost].Id,
                Bid = (float) addBid
            };
            model.Add(employeePost);
            EmployeesPosts = model.GetAdaptedList();

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
            AddBid = null;
            AddSelectedEmployee = null;
            AddSelectedPost = null;
        }

        #endregion

        #endregion

    }
}
