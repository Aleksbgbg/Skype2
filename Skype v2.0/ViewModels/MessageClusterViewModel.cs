namespace Skype2.ViewModels
{
    using Caliburn.Micro;

    using Skype2.Factories.Interfaces;
    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    using Message = Shared.Models.Message;

    internal class MessageClusterViewModel : ViewModelBase, IMessageClusterViewModel
    {
        private readonly IMessageFactory _messageFactory;

        private readonly IUserService _userService;

        public MessageClusterViewModel(IMessageFactory messageFactory, IUserService userService)
        {
            _messageFactory = messageFactory;
            _userService = userService;
        }

        public IMessageViewModel RootMessage { get; private set; }

        public IObservableCollection<IMessageViewModel> AllMessages { get; } = new BindableCollection<IMessageViewModel>();

        public bool SelfIsSender { get; private set; }

        public void Initialize(Message rootMessage)
        {
            RootMessage = _messageFactory.MakeMessageViewModel(rootMessage);
            AllMessages.Add(RootMessage);

            SelfIsSender = _userService.LoggedInUser.Id == rootMessage.SenderId;
        }

        public void AddMessage(Message message)
        {
            AllMessages.Add(_messageFactory.MakeMessageViewModel(message));
        }
    }
}