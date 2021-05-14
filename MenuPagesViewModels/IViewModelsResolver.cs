using System.ComponentModel;

namespace MenuPagesViewModels

{
    public interface IViewModelsResolver
    {
        INotifyPropertyChanged GetViewModelInstance(string alias);
    }
}