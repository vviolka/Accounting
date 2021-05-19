using System;
using System.Collections.Generic;
using System.ComponentModel;
using InfoPagesViewModels;
using SalaryPagesViewModels;

namespace MenuPagesViewModels
{
    public class ViewModelsResolver : IViewModelsResolver
    {

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> vmResolvers = new Dictionary<string, Func<INotifyPropertyChanged>>();

        public ViewModelsResolver(IErrorAlert errorAlert, IShowWindow showWindow)
        {

            vmResolvers.Add(MainViewModel.InfoPageViewModelAlias, () => new InfoViewModel(this));
            vmResolvers.Add(InfoViewModel.CompanyInfoPageAlias, () => new CompanyInfoVM());
            vmResolvers.Add(InfoViewModel.PartnersInfoPageAlias, () => new PartnersInfoVM(errorAlert));
            vmResolvers.Add(InfoViewModel.PostsInfoPageAlias, () => new PostsInfoVM(errorAlert));
            vmResolvers.Add(InfoViewModel.EmployeesInfoPageAlias, () => new EmployeesInfoVM());
          //  vmResolvers.Add(InfoViewModel.MaterialsInfoPageAlias, () => new MaterialsInfoVM(errorAlert));

            vmResolvers.Add(MainViewModel.DocumentsPageViewModelAlias, () => new DocumentsViewModel(showWindow));
            vmResolvers.Add(MainViewModel.ReportsPageViewModelAlias, () => new ReportsPageViewModel());
            vmResolvers.Add(MainViewModel.SalaryPageViewModelAlias, () => new SalaryPageViewModel(this));
            vmResolvers.Add(MainViewModel.LogPageViewModelAlias, () => new LogViewModel());

            //salary
            vmResolvers.Add(SalaryPageViewModel.EmployeesPostsPageAlias, () => new PostsEmployeesVM());
            vmResolvers.Add(SalaryPageViewModel.SalaryViewPageAlias, () => new SalaryViewVM());
        }

        public INotifyPropertyChanged GetViewModelInstance(string alias)
        {
            if (vmResolvers.ContainsKey(alias))
            {
                return vmResolvers[alias]();
            }

            return vmResolvers[MainViewModel.NotFoundPageViewModelAlias]();
        }

    }
}
