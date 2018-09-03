namespace Skype2.Services
{
    using System;
    using System.Collections.Generic;
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
        private readonly HttpClient _httpClient = new HttpClient();

        public string AuthToken { get; private set; }

        public RestService()
        {
            _httpClient.BaseAddress = new Uri($"http://{Constants.ServerIp}:{Constants.HttpPort}/");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        }

        public User LoggedInUser { get; private set; }

        public async Task Login(string username, SecureString password)
        {
            UserCredentials credentials = new UserCredentials(username, HashPassword(password));

            string credentialsSerialised = JsonConvert.SerializeObject(credentials);

            HttpResponseMessage response = await _httpClient.PostAsync("session/login", new StringContent(credentialsSerialised, Encoding.UTF8, "application/json"));

            string responseContent = await response.Content.ReadAsStringAsync();

            await Login(username, responseContent);
        }

        public async Task Register(string username, SecureString password)
        {
            HttpResponseMessage response = await _httpClient.PostAsync("session/register", new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", HashPassword(password))
            }));

            string responseContent = await response.Content.ReadAsStringAsync();

            await Login(username, responseContent);
        }

        public async Task Logout()
        {
            await _httpClient.GetAsync("session/logout");
        }

        public async Task<Message[]> GetMessages()
        {
            return await Get<Message[]>("messages/get/all");
        }

        public async Task<T> Get<T>(string path)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(path);

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        private async Task Login(string username, string token)
        {
            AuthToken = token;

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            LoggedInUser = await Get<User>($"user/get/by/name/{username}");
        }

        private static string HashPassword(SecureString password)
        {
            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();

            byte[] hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password))));

            string hashString = BitConverter.ToString(hashBytes).Replace("-", "");

            return hashString;
        }
    }
}