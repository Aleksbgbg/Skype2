namespace Skype2.Server.Endpoints.Http.Controllers
{
    using System.Web.Http;

    using Shared.Models;

    [RoutePrefix("messages")]
    public class MessagesController : ApiController
    {
        [HttpGet]
        [Route("get/all")]
        public Message[] GetAllMessages()
        {
            return new Message[0];
        }
    }
}