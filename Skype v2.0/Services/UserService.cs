namespace Skype2.Services
{
    using Shared.Models;

    using Skype2.Services.Interfaces;

    internal class UserService : IUserService
    {
        public User LoggedInUser { get; } = new User(1853778537283060748, "Aleksbgbg");
    }
}