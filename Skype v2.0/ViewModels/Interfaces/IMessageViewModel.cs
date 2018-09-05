namespace Skype2.ViewModels.Interfaces
{
    using Shared.Models;

    internal interface IMessageViewModel : IViewModelBase
    {
        Message Message { get; }

        void Initialize(Message message);

        void LoadSender();
    }
}