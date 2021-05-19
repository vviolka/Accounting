using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Navigator
{
    public sealed class Navigation
    {
        #region Constants

        public static readonly string InfoPageAlias = "InfoPage";
        public static readonly string CompanyInfoPageAlias = "CompanyInfoPage";
        public static readonly string PartnersInfoPageAlias = "PartnersInfoPage";
        public static readonly string MaterialsInfoPageAlias = "MaterialsInfoPage";
        public static readonly string PostsInfoPageAlias = "PostsInfoPage";
        public static readonly string EmployeesInfoPageAlias = "EmployeesInfoPage";


        public static readonly string LogPageAlias = "LogPage";
        public static readonly string NotFoundPageAlias = "404";

        public static readonly string DocumentsPageAlias = "DocumentsPage";
        public static readonly string ReportsPageAlias = "ReportsPage";
        public static readonly string SalaryPageAlias = "SalaryPage";

        //salary
        public static readonly string EmployeesPostsPageAlias = "EmployeesPostsPage";
        public static readonly string SalaryViewPageAlias = "SalaryViewPage";
        #endregion

        #region Fields

        private NavigationService navService;
        private readonly IPageResolver resolver;

        #endregion


        #region Properties

        public static NavigationService Service
        {
            get { return Instance.navService; }
            set
            {
                if (Instance.navService != null)
                {
                    Instance.navService.Navigated -= Instance._navService_Navigated;
                }

                Instance.navService = value;
                Instance.navService.Navigated += Instance._navService_Navigated;
            }
        }

        #endregion


        #region Public Methods

        public static void Navigate(Page page, object context)
        {
            if (Instance.navService == null || page == null)
            {
                return;
            }

            Instance.navService.Navigate(page, context);
        }


        public static void Navigate(string uri, object context)
        {
            if (Instance.navService == null || uri == null)
            {
                return;
            }

            var page = Instance.resolver.GetPageInstance(uri);

            Navigate(page, context);
        }

        public static void Navigate(string uri)
        {
            Navigate(uri, null);
        }

        #endregion


        #region Private Methods

        void _navService_Navigated(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;

            if (page == null)
            {
                return;
            }

            page.DataContext = e.ExtraData;
        }

        #endregion


        #region Singleton

        private static volatile Navigation instance;
        private static readonly object SyncRoot = new Object();

        private Navigation()
        {
            resolver = new PagesResolver();
        }

        private static Navigation Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                            instance = new Navigation();
                    }
                }

                return instance;
            }
        }
        #endregion
    }
}
