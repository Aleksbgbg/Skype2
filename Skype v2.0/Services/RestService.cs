namespace Skype2.Services
{
    using System;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Security;
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

        public User LoggedInUser { get; private set; }

        public async Task Login(string username, SecureString password)
        {
            string passwordBase64Hashed;

            {
                SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();

                byte[] hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password))));

                string hashString = BitConverter.ToString(hashBytes).Replace("-", "");

                passwordBase64Hashed = Convert.ToBase64String(Encoding.UTF8.GetBytes(hashString));
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

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {responseContent}");

            LoggedInUser = await Get<User>($"user/get/by/name/{username}");
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