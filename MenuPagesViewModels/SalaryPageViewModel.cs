using System.ComponentModel;
using System.Windows.Input;
using Navigator;

namespace MenuPagesViewModels
{
    public class SalaryPageViewModel: BaseViewModel
    {
        public SalaryPageViewModel(IViewModelsResolver resolver)
        {
            this.resolver = resolver;
            EmployeesPostsPageVM = this.resolver.GetViewModelInstance(EmployeesPostsPageAlias);
            SalaryViewPageVM = this.resolver.GetViewModelInstance(SalaryViewPageAlias);
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            goToEmployeesPostsPage = new RelayCommand<INotifyPropertyChanged>(GoToEmployeesPostsPageExecute);
            goToSalaryViewPage = new RelayCommand<INotifyPropertyChanged>(GoToSalaryViewPageExecute);
        }

        private readonly IViewModelsResolver resolver;
        #region EmployeesPosts

        public static readonly string EmployeesPostsPageAlias = "EmployeesPostsPage";
        private ICommand goToEmployeesPostsPage;

        public ICommand GoToEmployeesPostsPage
        {
            get => goToEmployeesPostsPage;
            set => goToEmployeesPostsPage = value;
        }

        public INotifyPropertyChanged EmployeesPostsPageVM { get; set; }

        private void GoToEmployeesPostsPageExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.EmployeesPostsPageAlias, EmployeesPostsPageVM);
        }
        #endregion
        
        #region SalaryView

        public static readonly string SalaryViewPageAlias = "SalaryViewPage";
        private ICommand goToSalaryViewPage;

        public ICommand GoToSalaryViewPage
        {
            get => goToSalaryViewPage;
            set => goToSalaryViewPage = value;
        }

        public INotifyPropertyChanged SalaryViewPageVM { get; set; }

        private void GoToSalaryViewPageExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.SalaryViewPageAlias, SalaryViewPageVM);
        }
        #endregion
    }
}
