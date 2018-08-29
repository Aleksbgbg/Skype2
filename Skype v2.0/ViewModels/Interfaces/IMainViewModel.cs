namespace Skype2.ViewModels.Interfaces
{
    using Caliburn.Micro;

    internal interface IMainViewModel : IViewModelBase, IConductor
    {
        ILoginViewModel LoginViewModel { get; }

        IChatViewModel ChatViewModel { get; }
    }
}