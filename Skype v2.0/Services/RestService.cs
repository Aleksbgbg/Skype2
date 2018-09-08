namespace Skype2.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Timers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Shared.Config;
    using Shared.Models;

    using Skype2.Services.Interfaces;

    internal class RestService : IRestService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Timer _sessionRefreshTimer = new Timer();

        private ClientSession _session;

        public string AuthorizationHeader { get; private set; }

        public RestService()
        {
            _httpClient.BaseAddress = new Uri(Constants.HttpServerAddress);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            _sessionRefreshTimer.Elapsed += async (sender, e) => await RefreshToken();
        }

        public User LoggedInUser { get; private set; }

        public async Task Login(string username, SecureString password)
        {
            await PerformSessionAction("login", username, password);
        }

        public async Task Register(string username, SecureString password)
        {
            await PerformSessionAction("register", username, password);
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

            return await Read<T>(response);
        }

        private async Task PerformSessionAction(string actionPath, string username, SecureString password)
        {
            HttpResponseMessage response;

            {
                IntPtr passwordPointer = IntPtr.Zero;

                RuntimeHelpers.PrepareConstrainedRegions();

                try
                {
                    passwordPointer = Marshal.SecureStringToBSTR(password);

                    UserCredentials credentials = new UserCredentials(username, Marshal.PtrToStringBSTR(passwordPointer));

                    string credentialsSerialised = JsonConvert.SerializeObject(credentials);

                    response = await _httpClient.PostAsync($"session/{actionPath}", new StringContent(credentialsSerialised, Encoding.UTF8, "application/json"));
                }
                finally
                {
                    if (passwordPointer != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeBSTR(passwordPointer);
                    }
                }
            }

            ImplementSession(await Read<ClientSession>(response));

            LoggedInUser = await Get<User>($"user/get/by/name/{username}");
        }

        private async Task RefreshToken()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            HttpResponseMessage response = await _httpClient.PostAsync("session/refresh", new StringContent(JsonConvert.SerializeObject(_session), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Reset user to login screen in the future
                _session = null;
                return;
            }

            ImplementSession(await Read<ClientSession>(response));
        }

        private void ImplementSession(ClientSession session)
        {
            _sessionRefreshTimer.Stop();

            _session = session;

            _sessionRefreshTimer.Interval = (session.ExpiresAt - DateTime.Now).TotalMilliseconds;
            _sessionRefreshTimer.Start();

            AuthorizationHeader = $"Bearer {_session.Token}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _session.Token);
        }

        private static async Task<T> Read<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}