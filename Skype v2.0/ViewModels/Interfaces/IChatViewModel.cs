namespace Skype2.ViewModels.Interfaces
{
    using Caliburn.Micro;

    internal interface IChatViewModel : IViewModelBase
    {
        IObservableCollection<IMessageClusterViewModel> MessageClusters { get; }
    }
}