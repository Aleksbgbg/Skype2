namespace Skype2.ViewModels
{
    using Caliburn.Micro;

    using Skype2.ViewModels.Interfaces;

    internal class ChatViewModel : ViewModelBase, IChatViewModel
    {
        public IObservableCollection<IMessageClusterViewModel> MessageClusters { get; } = new BindableCollection<IMessageClusterViewModel>();
    }
}