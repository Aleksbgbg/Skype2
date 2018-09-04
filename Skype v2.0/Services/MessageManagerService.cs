namespace Skype2.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shared.Models;

    using Skype2.Services.Interfaces;

    internal class MessageManagerService : IMessageManagerService
    {
        private readonly IRestService _restService;

        private readonly Dictionary<long, User> _userCache = new Dictionary<long, User>();

        public MessageManagerService(IRestService restService)
        {
            _restService = restService;
        }

        public async Task<User> LoadSender(Message message)
        {
            if (!_userCache.TryGetValue(message.SenderId, out User user))
            {
                user = await _restService.Get<User>($"users/get/{message.SenderId}");
                _userCache.Add(message.SenderId, user);
            }

            message.Sender = user;

            return user;
        }
    }
}