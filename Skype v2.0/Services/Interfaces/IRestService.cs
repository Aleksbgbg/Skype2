namespace Skype2.Services.Interfaces
{
    using System.Security;
    using System.Threading.Tasks;

    using Shared.Models;

    internal interface IRestService
    {
        string AuthorizationHeader { get; }

        User LoggedInUser { get; }

        Task<Message[]> GetMessages();

        Task<T> Get<T>(string path);

        Task Login(string username, SecureString password);

        Task Register(string username, SecureString password);

        Task Logout();
    }
}