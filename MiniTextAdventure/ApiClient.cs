using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MiniApiTextAdv.Models;
using System;
using System.Security.Cryptography;

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

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<LoginResponse>(resultJson, options);

            JwtToken = result?.token?.Trim();
    
            if (JwtToken != null)
            {
                _http.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", JwtToken);
            }

            Console.WriteLine("TOKEN VAN API:");
            Console.WriteLine(JwtToken);
            return JwtToken != null;
        }
        
        private HttpRequestMessage AddAuthHeader(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(JwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            }
            return request;
        }

        public async Task<RoomDto?> GetCurrentRoom()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/game/current-room");
            AddAuthHeader(request);
            
            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<RoomDto>();

        }

        public async Task<string?> GetKeyshare(string roomId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/keys/keyshare/{roomId}");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<KeyshareResponse>();
            return result?.keyshare;
        }

        public async Task<MoveResultDto?> MoveAsync(string direction)
        {
            var body = new { Direction = direction };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/game/move")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var resultJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MoveResultDto>(resultJson);
        }

        public async Task<string?> TakeAsync(string itemId)
        {
            var body = new { ItemId = itemId };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/game/take")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return "Fout bij ophalen item.";

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(resultJson);
            return result.GetProperty("message").GetString();
        }

        public async Task<FightResultDto?> FightAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/game/fight");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FightResultDto>(json);
        }

        public async Task<InventoryDto?> GetInventoryAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/game/inventory");
            AddAuthHeader(request);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<InventoryDto>(json);
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
