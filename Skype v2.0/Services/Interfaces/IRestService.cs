namespace Skype2.Services.Interfaces
{
    using System.Threading.Tasks;

    using Shared.Models;

    internal interface IRestService
    {
        Task<Message[]> GetMessages();
    }
}