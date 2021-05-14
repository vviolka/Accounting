using System;
using System.Collections.Generic;
using System.Windows.Controls;
using InfoPages;
using MenuPages;
using SalaryPages;

// using InfoPages;
// using Pages;

//using InfoPages;


namespace Navigator
{
    public class PagesResolver : IPageResolver
    {

        private readonly Dictionary<string, Func<Page>> pagesResolvers = new Dictionary<string, Func<Page>>();

        public PagesResolver()
        {

            pagesResolvers.Add(Navigation.InfoPageAlias, () => new InfoPage()); 
            pagesResolvers.Add(Navigation.CompanyInfoPageAlias, () => new CompanyInfoPage());
            pagesResolvers.Add(Navigation.PartnersInfoPageAlias, () => new PartnersInfoPage());
            pagesResolvers.Add(Navigation.PostsInfoPageAlias, () => new PostsInfoPage());
            pagesResolvers.Add(Navigation.EmployeesInfoPageAlias, () => new EmployeesInfoPage());
           // pagesResolvers.Add(Navigation.MaterialsInfoPageAlias, () => new MaterialsInfoPage());

            pagesResolvers.Add(Navigation.DocumentsPageAlias, () => new DocumentsPage());
            pagesResolvers.Add(Navigation.ReportsPageAlias, () => new ReportsPage());
            pagesResolvers.Add(Navigation.SalaryPageAlias, () => new SalaryPage());

            //salary
            pagesResolvers.Add(Navigation.EmployeesPostsPageAlias, () => new PostsEmployeesPage());
            pagesResolvers.Add(Navigation.SalaryViewPageAlias, () => new SalaryView());


        }

        public Page GetPageInstance(string alias)
        {
            if (pagesResolvers.ContainsKey(alias))
            {
                return pagesResolvers[alias]();
            }

            return pagesResolvers[Navigation.NotFoundPageAlias]();
        }
    }
}