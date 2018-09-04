namespace Skype2.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

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
        }

        protected override void OnActivate()
        {
            Task.Run(async () =>
            {
                foreach (Message message in await _restService.GetMessages())
                {
                    AddMessage(message);
                }
            });

            _messageService.MessageReceived += (sender, e) => AddMessage(e.Message);
        }

        public IObservableCollection<IMessageClusterViewModel> MessageClusters { get; } = new BindableCollection<IMessageClusterViewModel>();

        private string _message;
        public string Message
        {
            get => _message;

            set
            {
                if (_message == value) return;

                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        public void KeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Shift && Message != string.Empty)
            {
                _messageService.SendMessage(Message.TrimEnd());
                Message = string.Empty;
            }
        }

        private void AddMessage(Message message)
        {
            IMessageClusterViewModel lastMessageCluster = MessageClusters.LastOrDefault();

            if (lastMessageCluster == null || lastMessageCluster.RootMessage.Message.SenderId != message.SenderId)
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