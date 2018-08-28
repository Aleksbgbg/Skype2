namespace Skype2.Services
{
    using Shared.Models;

    using Skype2.Services.Interfaces;

    internal class SessionService : ISessionService
    {
        public User LoggedInUser { get; } = new User { Id = 1853778537283060748, Name = "Aleksbgbg" };
    }
}