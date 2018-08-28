namespace HttpServer.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HttpServer.Database;

    using Microsoft.AspNetCore.Mvc;

    using Shared.Models;

    [ApiController]
    [Route("[Controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly Skype2Context _database;

        public MessagesController(Skype2Context database)
        {
            _database = database;
        }

        [HttpGet("get/all")]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            return _database.Messages.OrderBy(message => message.Id).ToArray();
        }

        [HttpGet("get/{id}")]
        public ActionResult<Message> GetMessage(long id)
        {
            return _database.Messages.Single(message => message.Id == id);
        }

        [HttpPost("post")]
        public async Task PostMessage([FromBody] Message message)
        {
            await _database.Messages.AddAsync(message);
            await _database.SaveChangesAsync();
        }
    }
}