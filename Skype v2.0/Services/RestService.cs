namespace Skype2.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
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

        public User LoggedInUser { get; private set; } = new User { Id = 1853778537283060748, Name = "Aleksbgbg" };

        public async Task Login(string username, string password)
        {
            string passwordBase64Hashed;

            {
                SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();

                byte ignoreCharacter = (byte)'-';
                byte[] hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password))
                                         .Where(byteValue => byteValue != ignoreCharacter)
                                         .ToArray();

                passwordBase64Hashed = Convert.ToBase64String(hashBytes);
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GetUrl("session/login"))
            {
                    Headers =
                    {
                            { "Authorization", $"Basic {username} {passwordBase64Hashed}" }
                    }
            };

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            LoggedInUser = await Get<User>($"get/by/name/{username}");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {responseContent}");
        }

        public async Task Logout()
        {
            await _httpClient.GetAsync(GetUrl("session/logout"));
        }

        public async Task<Message[]> GetMessages()
        {
            return await Get<Message[]>("messages/get/all");
        }

        public async Task<T> Get<T>(string path)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(GetUrl(path));

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        private static string GetUrl(string target)
        {
            return string.Concat(BaseRestAddress, target);
        }
    }
}