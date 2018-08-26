namespace HttpServer.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using HttpServer.Database;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Shared.Models;

    [ApiController]
    [Route("[controller]")]
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
            return _database.Messages.Include(message => message.Sender).ToArray();
        }

        [HttpGet("get/{id}")]
        public ActionResult<Message> GetMessage(long id)
        {
            return _database.Messages.Include(message => message.Sender).FirstOrDefault(message => message.Id == id);
        }
    }
}