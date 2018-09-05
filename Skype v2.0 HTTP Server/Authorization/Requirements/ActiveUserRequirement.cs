namespace HttpServer.Authorization.Requirements
{
    using Microsoft.AspNetCore.Authorization;

    internal class ActiveUserRequirement : IAuthorizationRequirement
    {
        // Ensures that a user can only edit their personal data, and not other users'
    }
}