namespace Skype2.Services.Interfaces
{
    using Shared.Models;

    internal interface ISessionService
    {
        User LoggedInUser { get; }
    }
}