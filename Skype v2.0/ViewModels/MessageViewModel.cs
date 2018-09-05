namespace Skype2.ViewModels
{
    using System.Threading.Tasks;

    using Shared.Models;

    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    internal class MessageViewModel : ViewModelBase, IMessageViewModel
    {
        private readonly IMessageManagerService _messageManagerService;

        public MessageViewModel(IMessageManagerService messageManagerService)
        {
            _messageManagerService = messageManagerService;
        }

        public Message Message { get; private set; }

        public void Initialize(Message message)
        {
            Message = message;
        }

        public void LoadSender()
        {
            Task.Run(async () =>
            {
                await _messageManagerService.LoadSender(Message);
                NotifyOfPropertyChange(() => Message);
            });
        }
    }
}