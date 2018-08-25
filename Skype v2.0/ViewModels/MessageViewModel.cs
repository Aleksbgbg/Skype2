namespace Skype2.ViewModels
{
    using Shared.Models;

    using Skype2.ViewModels.Interfaces;

    internal class MessageViewModel : ViewModelBase, IMessageViewModel
    {
        public Message Message { get; private set; }

        public void Initialize(Message message)
        {
            Message = message;
        }
    }
}