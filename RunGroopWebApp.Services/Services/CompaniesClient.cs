
using System.Text.Json;

namespace RunGroopWebApp.Services.Services
{
    public class CompaniesClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public CompaniesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<CompanyViewModel>> GetCompanies()
        {
            using var response = await _httpClient.GetAsync("Companies", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<CompanyViewModel>>(stream, _options);
        }
     }
}
