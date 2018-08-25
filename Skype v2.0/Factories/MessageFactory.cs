namespace Skype2.Factories
{
    using Caliburn.Micro;

    using Skype2.Factories.Interfaces;
    using Skype2.ViewModels.Interfaces;

    using Message = Shared.Models.Message;

    internal class MessageFactory : IMessageFactory
    {
        public IMessageViewModel MakeMessageViewModel(Message message)
        {
            IMessageViewModel messageViewModel = IoC.Get<IMessageViewModel>();
            messageViewModel.Initialize(message);

            return messageViewModel;
        }

        public IMessageClusterViewModel MakeMessageClusterViewModel(Message rootMessage)
        {
            IMessageClusterViewModel messageClusterViewModel = IoC.Get<IMessageClusterViewModel>();
            messageClusterViewModel.Initialize(rootMessage);

            return messageClusterViewModel;
        }
    }
}