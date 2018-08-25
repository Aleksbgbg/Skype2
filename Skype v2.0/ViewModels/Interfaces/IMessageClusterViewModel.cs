namespace Skype2.ViewModels.Interfaces
{
    using Caliburn.Micro;

    using Message = Shared.Models.Message;

    internal interface IMessageClusterViewModel : IViewModelBase
    {
        IMessageViewModel RootMessage { get; }

        IObservableCollection<IMessageViewModel> AllMessages { get; }

        void Initialize(Message rootMessage);

        void AddMessage(Message message);
    }
}