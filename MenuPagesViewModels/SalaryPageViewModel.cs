using System;
using System.ComponentModel;
using System.Windows.Input;
using Common;
using Navigator;
using ReportPagesViewModels.Annotations;

namespace MenuPagesViewModels
{
    public class SalaryPageViewModel: BaseViewModel
    {
        public SalaryPageViewModel(IViewModelsResolver resolver)
        {
            this.resolver = resolver;
            EmployeesPostsPageVM = this.resolver.GetViewModelInstance(EmployeesPostsPageAlias);
            SalaryViewPageVM = this.resolver.GetViewModelInstance(SalaryViewPageAlias);
            SalariesViewPageVM = this.resolver.GetViewModelInstance(SalariesViewPageAlias);
            VacationsPageVM = this.resolver.GetViewModelInstance(VacationsPageAlias);
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            goToEmployeesPostsPage = new RelayCommand<INotifyPropertyChanged>(GoToEmployeesPostsPageExecute);
            goToSalaryViewPage = new RelayCommand<INotifyPropertyChanged>(GoToSalaryViewPageExecute);
            goToSalariesPage = new RelayCommand<INotifyPropertyChanged>(GoToSalariesPageExecute);
            goToVacationsPage = new RelayCommand<INotifyPropertyChanged>(GoToVacationsViewPageExecute);
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

        #region SalariesPage

        public static readonly string SalariesViewPageAlias = "SalariesViewPage";
        private ICommand goToSalariesPage;

        public ICommand GoToSalariesPage
        {
            get => goToSalariesPage;
            set => goToSalariesPage = value;
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

        public INotifyPropertyChanged SalariesViewPageVM { get; set; }
        private void GoToSalariesPageExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.SalariesPageAlias, SalariesViewPageVM);
        }

        #endregion

        #region VacationsPage

        public static readonly string VacationsPageAlias = "VacationsPage";
        private ICommand goToVacationsPage;

        [NotNull]
        public ICommand GoToVacationsPage
        {
            get => goToVacationsPage;
            set => goToVacationsPage = value;
        }

        public INotifyPropertyChanged VacationsPageVM { get; set; }

        private void GoToVacationsViewPageExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.VacationsPageAlias, VacationsPageVM);
        }
        
        #endregion
    }
}
