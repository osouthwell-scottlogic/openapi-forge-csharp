using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenApiForge
{
    // Swagger Petstore - OpenAPI 3.0
    public class ApiClient : IApiClient
    {
        private const string ContentType = "application/json";
        private readonly string _baseAddress;
        private readonly HttpClient _client;

        public ApiClient(HttpClient httpClient, Configuration config)
        {
            _client = httpClient;
            _baseAddress = config.GetBaseAddress();
        }


        public async Task<Pet> updatePet(Pet body)
        {
            var requestUri = $"{_baseAddress}/pet";
            var request = new HttpRequestMessage(new HttpMethod("put"), requestUri)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, ContentType)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pet>(responseBody);
        }

        public async Task<Pet> addPet(Pet body)
        {
            var requestUri = $"{_baseAddress}/pet";
            var request = new HttpRequestMessage(new HttpMethod("post"), requestUri)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, ContentType)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pet>(responseBody);
        }

        public async Task<Pet[]> findPetsByStatus(string status = "available")
        {
            var queryString = new StringBuilder();
            queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}status={Uri.EscapeDataString(status)}");
            var requestUri = $"{_baseAddress}/pet/findByStatus{queryString}";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pet[]>(responseBody);
        }

        public async Task<Pet[]> findPetsByTags(string[] tags)
        {
            var queryString = new StringBuilder();
            if (tags != null && tags.Length > 0)
            {
                queryString.Append(
                    $"{(queryString.Length == 0 ? "?" : "&")}{string.Join("&", tags.Select(p => $"tags={Uri.EscapeDataString(p)}"))}");
            }

            var requestUri = $"{_baseAddress}/pet/findByTags{queryString}";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pet[]>(responseBody);
        }

        public async Task<Pet> getPetById(long petId)
        {
            var requestUri = $"{_baseAddress}/pet/{petId}";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Pet>(responseBody);
        }

        public async Task updatePetWithForm(long petId, string name, string status)
        {
            var queryString = new StringBuilder();
            if (name != null)
            {
                queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}name={Uri.EscapeDataString(name)}");
            }

            if (status != null)
            {
                queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}status={Uri.EscapeDataString(status)}");
            }

            var requestUri = $"{_baseAddress}/pet/{petId}{queryString}";
            var request = new HttpRequestMessage(new HttpMethod("post"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task deletePet(long petId, string api_key)
        {
            var requestUri = $"{_baseAddress}/pet/{petId}";
            var request = new HttpRequestMessage(new HttpMethod("delete"), requestUri)
            {
            };
            request.Headers.Add("api_key", api_key);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public Task<ApiResponse> uploadFile(long petId, string additionalMetadata)
        {
            throw new NotImplementedException(
                "Operation 'uploadFile' most likely does not support json encoded requests which are not supported by openapi forge.");
        }

        public async Task<Dictionary<string, int>> getInventory()
        {
            var requestUri = $"{_baseAddress}/store/inventory";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, int>>(responseBody);
        }

        public async Task<Order> placeOrder(Order body)
        {
            var requestUri = $"{_baseAddress}/store/order";
            var request = new HttpRequestMessage(new HttpMethod("post"), requestUri)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, ContentType)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Order>(responseBody);
        }

        public async Task<Order> getOrderById(long orderId)
        {
            var requestUri = $"{_baseAddress}/store/order/{orderId}";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Order>(responseBody);
        }

        public async Task deleteOrder(long orderId)
        {
            var requestUri = $"{_baseAddress}/store/order/{orderId}";
            var request = new HttpRequestMessage(new HttpMethod("delete"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<User> createUser(User body)
        {
            var requestUri = $"{_baseAddress}/user";
            var request = new HttpRequestMessage(new HttpMethod("post"), requestUri)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, ContentType)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody);
        }

        public async Task<User> createUsersWithListInput(User[] body)
        {
            var requestUri = $"{_baseAddress}/user/createWithList";
            var request = new HttpRequestMessage(new HttpMethod("post"), requestUri)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, ContentType)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody);
        }

        public async Task<string> loginUser(string username, string password)
        {
            var queryString = new StringBuilder();
            if (username != null)
            {
                queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}username={Uri.EscapeDataString(username)}");
            }

            if (password != null)
            {
                queryString.Append($"{(queryString.Length == 0 ? "?" : "&")}password={Uri.EscapeDataString(password)}");
            }

            var requestUri = $"{_baseAddress}/user/login{queryString}";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task logoutUser()
        {
            var requestUri = $"{_baseAddress}/user/logout";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<User> getUserByName(string username)
        {
            var requestUri = $"{_baseAddress}/user/{username}";
            var request = new HttpRequestMessage(new HttpMethod("get"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody);
        }

        public async Task updateUser(string username, User body)
        {
            var requestUri = $"{_baseAddress}/user/{username}";
            var request = new HttpRequestMessage(new HttpMethod("put"), requestUri)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, ContentType)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task deleteUser(string username)
        {
            var requestUri = $"{_baseAddress}/user/{username}";
            var request = new HttpRequestMessage(new HttpMethod("delete"), requestUri)
            {
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}