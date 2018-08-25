namespace Skype2.Factories.Interfaces
{
    using Shared.Models;

    using Skype2.ViewModels.Interfaces;

    internal interface IMessageFactory
    {
        IMessageViewModel MakeMessageViewModel(Message message);

        IMessageClusterViewModel MakeMessageClusterViewModel(Message rootMessage);
    }
}