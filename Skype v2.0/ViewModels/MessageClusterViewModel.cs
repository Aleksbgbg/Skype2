﻿namespace Skype2.ViewModels
{
    using System.Threading.Tasks;

    using Caliburn.Micro;

    using Skype2.Factories.Interfaces;
    using Skype2.Services.Interfaces;
    using Skype2.ViewModels.Interfaces;

    using Message = Shared.Models.Message;

    internal class MessageClusterViewModel : ViewModelBase, IMessageClusterViewModel
    {
        private readonly IMessageFactory _messageFactory;

        private readonly IRestService _sessionService;

        public MessageClusterViewModel(IMessageFactory messageFactory, IRestService sessionService, IMessageManagerService messageManagerService)
        {
            _messageFactory = messageFactory;
            _sessionService = sessionService;

            Task.Run(async () => await messageManagerService.LoadSender(RootMessage.Message));
        }

        public IMessageViewModel RootMessage { get; private set; }

        public IObservableCollection<IMessageViewModel> AllMessages { get; } = new BindableCollection<IMessageViewModel>();

        public bool SelfIsSender { get; private set; }

        public void Initialize(Message rootMessage)
        {
            RootMessage = _messageFactory.MakeMessageViewModel(rootMessage);
            AllMessages.Add(RootMessage);

            SelfIsSender = _sessionService.LoggedInUser.Id == rootMessage.SenderId;
        }

        public void AddMessage(Message message)
        {
            AllMessages.Add(_messageFactory.MakeMessageViewModel(message));
        }
    }
}