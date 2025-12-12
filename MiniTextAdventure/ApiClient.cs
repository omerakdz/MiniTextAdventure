using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace MiniTextAdventure
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        public string? JwtToken { get; private set; }

        public ApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> Login(string username, string password)
        {
            var body = new
            {
                Username = username,
                Password = password
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResponse>(resultJson);

            JwtToken = result?.token;

            return JwtToken != null;
        }

        private void AddJwt()
        {
            if (JwtToken != null)
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", JwtToken);
            }
        }

        public async Task<string?> GetKeyshare(string roomId)
        {
            AddJwt();

            var response = await _http.GetAsync($"/api/keys/keyshare/{roomId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<KeyshareResponse>(json);

            return result?.keyshare;
        }
    }
    public class LoginResponse
    {
        public string token { get; set; }
    }
    public class KeyshareResponse
    {
        public string keyshare { get; set; }
    }
}
