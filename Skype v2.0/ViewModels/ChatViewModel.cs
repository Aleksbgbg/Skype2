namespace Skype2.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;

    using Caliburn.Micro;

    using Skype2.Factories.Interfaces;
    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    using Message = Shared.Models.Message;

    internal class ChatViewModel : ViewModelBase, IChatViewModel
    {
        private readonly IMessageFactory _messageFactory;

        private readonly IMessageService _messageService;

        private readonly IRestService _restService;

        public ChatViewModel(IMessageFactory messageFactory, IMessageService messageService, IRestService restService)
        {
            _messageFactory = messageFactory;
            _messageService = messageService;
            _restService = restService;

            Task.Run(LoadMessages);

            _messageService.MessageReceived += (sender, e) => AddMessage(e.Message);
        }

        public IObservableCollection<IMessageClusterViewModel> MessageClusters { get; } = new BindableCollection<IMessageClusterViewModel>();

        private async Task LoadMessages()
        {
            foreach (Message message in await _restService.GetMessages())
            {
                AddMessage(message);
            }
        }

        private void AddMessage(Message message)
        {
            IMessageClusterViewModel lastMessageCluster = MessageClusters.LastOrDefault();

            if (lastMessageCluster == null || lastMessageCluster.RootMessage.Message.Sender != message.Sender)
            {
                MessageClusters.Add(_messageFactory.MakeMessageClusterViewModel(message));
            }
            else
            {
                lastMessageCluster.AddMessage(message);
            }
        }
    }
}