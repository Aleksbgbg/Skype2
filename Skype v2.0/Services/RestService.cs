namespace Skype2.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Shared.Config;
    using Shared.Models;

    using Skype2.Services.Interfaces;

    internal class RestService : IRestService
    {
        private static readonly string BaseRestAddress = $"http://{Constants.ServerIp}:{Constants.HttpPort}/";

        private readonly HttpClient _httpClient = new HttpClient();

        public RestService()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        }

        public async Task<Message[]> GetMessages()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(GetUrl("messages/get/all"));

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Message[]>(content);
        }

        private static string GetUrl(string target)
        {
            return string.Concat(BaseRestAddress, target);
        }
    }
}