namespace HttpServer.Authorization.Handlers
{
    using System.Threading.Tasks;

    using HttpServer.Authorization.Requirements;

    using Microsoft.AspNetCore.Authorization;

    using Shared.Models;

    internal class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement, User targetUser)
        {
            if (context.User.Identity.Name == targetUser.Name)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}