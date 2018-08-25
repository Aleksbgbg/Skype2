namespace Skype2.ViewModels.Interfaces
{
    internal interface IMainViewModel : IViewModelBase
    {
        IChatViewModel ChatViewModel { get; }
    }
}