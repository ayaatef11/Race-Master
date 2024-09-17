using RunGroopWebApp.ViewModels;
using System.Net.Http;
using System.Text.Json;

namespace RunGroopWebApp.Clients
{
    public class CompaniesClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        public CompaniesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();
            _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }
        public async Task<List<CompanyViewModel>> GetCompanies()
        {
            using (var response = await httpClient.GetAsync("Companies", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var compaines = await JsonSerializer.DeserializeAsync<List<CompanyViewModel>>(stream, _options);
            }
            return companies;
        }
    }
    }
}
