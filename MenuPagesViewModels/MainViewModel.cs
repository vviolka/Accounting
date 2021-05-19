using System.ComponentModel;
using System.Windows.Input;
using DevExpress.Mvvm;
using Navigator;

namespace MenuPagesViewModels
{
	public class MainViewModel : BindableBase
	{
		#region Constants

		public static readonly string InfoPageViewModelAlias = "InfoPageVM";
		public static readonly string DocumentsPageViewModelAlias = "Documents";
		public static readonly string ReportsPageViewModelAlias = "Reports";
        public static readonly string SalaryPageViewModelAlias = "Salary";
        public static readonly string LogPageViewModelAlias = "Log";
		public static readonly string CompanyInfoPageViewModelAlias = "CompanyInfoPageVM";
        public static readonly string NotFoundPageViewModelAlias = "404VM";

		#endregion

		#region Fields
		private bool openMenuBtnVisible = true;
		private bool closeMenuBtnVisible = false;
		public DelegateCommand OpenMenuBtnClick { get; }
		public DelegateCommand CloseMenuBtnClick { get; }
		public DelegateCommand BackPageCommand { get; }
		public DelegateCommand ForwardPageCommand { get; }

		private ICommand goToInfoPage;
        private ICommand goToDocumentsPage;
        private ICommand goToReportsPage;
        private ICommand goToSalaryPage;
        private ICommand goToLogPage;
		private readonly INotifyPropertyChanged infoPageViewModel;
        private readonly INotifyPropertyChanged documentsPageViewModel;
        private readonly INotifyPropertyChanged reportsPageViewModel;
        private readonly INotifyPropertyChanged salaryPageViewModel;
        private readonly INotifyPropertyChanged logPageViewModel;

		#endregion

		#region Properties

		public bool OpenMenuBtnVisible
		{
			get => openMenuBtnVisible;
			set
			{
				openMenuBtnVisible = value;
				UpdateMenuBtn();
			}
		}

		public bool CloseMenuBtnVisible
		{
			get => closeMenuBtnVisible;
			set
			{
				closeMenuBtnVisible = value;
				UpdateMenuBtn();
			}
		}
		public ICommand GoToInfoPage
		{
			get => goToInfoPage;
			set
			{
				goToInfoPage = value;
				RaisePropertyChanged("GoToPage1Command");
			}
		}

        public ICommand GoToDocumentsPage
        {
            get => goToDocumentsPage;
            set
            {
				goToDocumentsPage = value;
				RaisePropertyChanged("GoToDocumentsPageCommand");
            }
        }

        public ICommand GoToReportsPage
        {
            get => goToReportsPage;
            set
            {
                goToReportsPage = value;
                RaisePropertyChanged("GoToReportsPageCommand");
            }
        }

        public ICommand GoToSalaryPage
        {
            get => goToSalaryPage;
            set
            {
                goToSalaryPage = value;
				RaisePropertiesChanged("GoToSalaryPageCommand");
            }
        }

        public ICommand GoToLogPage
        {
            get => goToLogPage;
            set
            {
                goToLogPage = value;
				RaisePropertiesChanged("GoToLogPageCommand");
            }
        }
public INotifyPropertyChanged InfoPageViewModel => infoPageViewModel;
        public INotifyPropertyChanged DocumentsPageViewModel => documentsPageViewModel;
        public INotifyPropertyChanged ReportsPageViewModel => reportsPageViewModel;
        public INotifyPropertyChanged SalaryPageViewModel => salaryPageViewModel;
        public INotifyPropertyChanged LogPageViewModel => logPageViewModel;

		#endregion

		#region IMainViewModelImplementation

		#endregion





		private readonly IViewModelsResolver resolver;

		#region Constructor

		public MainViewModel(IViewModelsResolver resolver)
		{
			this.resolver = resolver;
			OpenMenuBtnClick = new DelegateCommand(OpenMenu);
			CloseMenuBtnClick = new DelegateCommand(CloseMenu);
			BackPageCommand = new DelegateCommand(BackPage);
			ForwardPageCommand = new DelegateCommand(ForwardPage);
			infoPageViewModel = this.resolver.GetViewModelInstance(InfoPageViewModelAlias);
            documentsPageViewModel = this.resolver.GetViewModelInstance(DocumentsPageViewModelAlias);
            reportsPageViewModel = this.resolver.GetViewModelInstance(ReportsPageViewModelAlias);
            salaryPageViewModel = this.resolver.GetViewModelInstance(SalaryPageViewModelAlias);
            logPageViewModel = this.resolver.GetViewModelInstance(LogPageViewModelAlias);
            InitializeCommands();
		}

		private void BackPage()
		{
			if (Navigation.Service.CanGoBack)
				Navigation.Service.GoBack();
		}
		private void ForwardPage()
		{
			if (Navigation.Service.CanGoForward)
				Navigation.Service.GoForward();
		}

		#endregion


		private void InitializeCommands()
		{
			goToInfoPage = new RelayCommand<INotifyPropertyChanged>(GoToInfoPageCommandExecute);
			goToDocumentsPage = new RelayCommand<INotifyPropertyChanged>(GoToDocumentsPageCommandExecute);
            goToReportsPage = new RelayCommand<INotifyPropertyChanged>(GoToReportsPageCommandExecute);
            goToSalaryPage = new RelayCommand<INotifyPropertyChanged>(GoToSalaryPageCommandExecute);
            goToLogPage = new RelayCommand<INotifyPropertyChanged>(GoToLogPageCommandExecute);
        }
		private void GoToInfoPageCommandExecute(INotifyPropertyChanged viewModel)
		{
			Navigation.Navigate(Navigation.InfoPageAlias, InfoPageViewModel);
		}

        private void GoToDocumentsPageCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.DocumentsPageAlias, DocumentsPageViewModel);
        }
        private void GoToReportsPageCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.ReportsPageAlias, ReportsPageViewModel);
        }
        private void GoToSalaryPageCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.SalaryPageAlias, SalaryPageViewModel);
        }

        private void GoToLogPageCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.LogPageAlias, LogPageViewModel);
        }


		private void CloseMenu()
		{
			openMenuBtnVisible = true;
			closeMenuBtnVisible = false;
		}

		private void OpenMenu()
		{
			openMenuBtnVisible = false;
			closeMenuBtnVisible = true;
		}

		private void UpdateMenuBtn()
		{
			RaisePropertyChanged(nameof(OpenMenuBtnVisible));
			RaisePropertyChanged(nameof(CloseMenuBtnVisible));
		}

	}


}


