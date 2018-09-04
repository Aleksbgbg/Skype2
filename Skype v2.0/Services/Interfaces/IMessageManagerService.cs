namespace Skype2.Services.Interfaces
{
    using System.Threading.Tasks;

    using Shared.Models;

    internal interface IMessageManagerService
    {
        Task<User> LoadSender(Message message);
    }
}