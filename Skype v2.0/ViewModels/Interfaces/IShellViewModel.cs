namespace Skype2.ViewModels.Interfaces
{
    using Caliburn.Micro;

    internal interface IShellViewModel : IViewModelBase, IConductor
    {
        IMainViewModel MainViewModel { get; }
    }
}