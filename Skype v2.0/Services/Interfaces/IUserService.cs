namespace Skype2.Services.Interfaces
{
    using Shared.Models;

    internal interface IUserService
    {
        User LoggedInUser { get; }
    }
}