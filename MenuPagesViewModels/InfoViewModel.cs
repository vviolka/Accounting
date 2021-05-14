using System.ComponentModel;
using System.Windows.Input;
using Navigator; //using System.Windows.Forms;
//using Model;
//using Pages;
//using PartnerWindow;

namespace MenuPagesViewModels
{
	public class InfoViewModel : BaseViewModel
	{
		private readonly IViewModelsResolver resolver;

		#region CompanyInfo

		public static readonly string CompanyInfoPageAlias = "CompanyInfoPage";
		private ICommand goToCompanyInfoPage;

		public ICommand GoToCompanyInfoPage
		{
			get => goToCompanyInfoPage;
			set => goToCompanyInfoPage = value;
		}

		public INotifyPropertyChanged CompanyInfoPageVM { get; set; }

		private void GoToCompanyInfoPageExecute(INotifyPropertyChanged viewModel)
		{
			Navigation.Navigate(Navigation.CompanyInfoPageAlias, CompanyInfoPageVM);
		}
		#endregion
		#region PartnersInfo

		public static readonly string PartnersInfoPageAlias = "PartnersInfoPage";
		private ICommand goToPartnersInfoPage;

		public ICommand GoToPartnersInfoPage
		{
			get => goToPartnersInfoPage;
			set => goToPartnersInfoPage = value;
		}

		public INotifyPropertyChanged PartnersInfoPageVM { get; set; }

		private void GoToPartnersInfoPageExecute(INotifyPropertyChanged viewModel)
		{
			Navigation.Navigate(Navigation.PartnersInfoPageAlias, PartnersInfoPageVM);
		}
		#endregion

        #region PostsInfo

        public static readonly string PostsInfoPageAlias = "PostsInfoPage";
        private ICommand goToPostsInfoPage;

        public ICommand GoToPostsInfoPage
        {
            get => goToPostsInfoPage;
            set => goToPostsInfoPage = value;
        }

        public INotifyPropertyChanged PostsInfoPageVM { get; set; }

        private void GoToPostsInfoPageExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.PostsInfoPageAlias, PostsInfoPageVM);
        }
		#endregion

        #region EmployeesInfo

        public static readonly string EmployeesInfoPageAlias = "EmployeesInfoPage";
        private ICommand goToEmployeesInfoPage;

        public ICommand GoToEmployeesInfoPage
        {
            get => goToEmployeesInfoPage;
            set => goToEmployeesInfoPage = value;
        }

        public INotifyPropertyChanged EmployeesInfoPageVM { get; set; }

        private void GoToEmployeesInfoPageExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.EmployeesInfoPageAlias, EmployeesInfoPageVM);
        }
        #endregion
		/*
		#region MaterialsInfo

		public static readonly string MaterialsInfoPageAlias = "MaterialsInfoPage";
		private ICommand goToMaterialsInfoPage;

		public ICommand GoToMaterialsInfoPage
		{
			get => goToMaterialsInfoPage;
			set => goToMaterialsInfoPage = value;
		}

		public INotifyPropertyChanged MaterialsInfoPageVM { get; set; }

		private void GoToMaterialsInfoPageExecute(INotifyPropertyChanged viewModel)
		{
			Navigation.Navigate(Navigation.MaterialsInfoPageAlias, MaterialsInfoPageVM);
		}
		#endregion
		*/

		#region constructor

		public InfoViewModel(IViewModelsResolver resolver)
		{
			this.resolver = resolver;
			CompanyInfoPageVM = this.resolver.GetViewModelInstance(CompanyInfoPageAlias);
			PartnersInfoPageVM = this.resolver.GetViewModelInstance(PartnersInfoPageAlias);
            PostsInfoPageVM = this.resolver.GetViewModelInstance(PostsInfoPageAlias);
            EmployeesInfoPageVM = this.resolver.GetViewModelInstance(EmployeesInfoPageAlias);
			//MaterialsInfoPageVM = this.resolver.GetViewModelInstance(MaterialsInfoPageAlias);
			InitializeCommands();
		}
		private void InitializeCommands()
		{
			goToCompanyInfoPage = new RelayCommand<INotifyPropertyChanged>(GoToCompanyInfoPageExecute);
			goToPartnersInfoPage = new RelayCommand<INotifyPropertyChanged>(GoToPartnersInfoPageExecute);
            goToPostsInfoPage = new RelayCommand<INotifyPropertyChanged>(GoToPostsInfoPageExecute);
            goToEmployeesInfoPage = new RelayCommand<INotifyPropertyChanged>(GoToEmployeesInfoPageExecute);
            //goToMaterialsInfoPage = new RelayCommand<INotifyPropertyChanged>(GoToMaterialsInfoPageExecute);
        }
		#endregion
	}
}