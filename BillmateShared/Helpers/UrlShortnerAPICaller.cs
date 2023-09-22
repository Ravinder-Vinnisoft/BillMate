using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using BillMate.Models.UrlShortener;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BillMate.Helpers
{
    public class UrlShortnerAPICaller
    {
        private HttpClient _httpClient;
        private readonly string _serverDomain;


        public UrlShortnerAPICaller(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _serverDomain = configuration["ServerDomain"];
        }

        public async Task<APIResponse<object>> ShortenUrl(UrlParameter urlParameter)
        {
            var json = JsonConvert.SerializeObject(urlParameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_serverDomain}/api/shorten", data);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<APIResponse<object>>(resultString);
                result.Data = $"{_serverDomain}/r/{result.Data}";
                return result;
            }
            else
            {
                throw new Exception($"Request failed with status code: {response.StatusCode}");
            }
        }

        public async Task<string> GetShortenedUrl(string token)
        {
            var response = await _httpClient.GetAsync($"{_serverDomain}/r/{token}");
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
