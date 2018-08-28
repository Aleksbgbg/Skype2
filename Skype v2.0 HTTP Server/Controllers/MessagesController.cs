namespace HttpServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
            return _database.Messages.Include(message => message.Sender).OrderBy(message => message.Id).ToArray();
        }

        [HttpGet("get/{id}")]
        public ActionResult<Message> GetMessage(long id)
        {
            return _database.Messages.Include(message => message.Sender).FirstOrDefault(message => message.Id == id);
        }

        [HttpPost("post")]
        public async Task PostMessage([FromBody] Message message)
        {
            await _database.Messages.AddAsync(message);
            await _database.SaveChangesAsync();
        }
    }
}