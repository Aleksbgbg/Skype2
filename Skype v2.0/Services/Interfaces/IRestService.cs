namespace Skype2.Services.Interfaces
{
    using System.Threading.Tasks;

    using Shared.Models;

    internal interface IRestService
    {
        User LoggedInUser { get; }

        Task<Message[]> GetMessages();

        Task<T> Get<T>(string path);

        Task Login(string username, string password);

        Task Logout();
    }
}