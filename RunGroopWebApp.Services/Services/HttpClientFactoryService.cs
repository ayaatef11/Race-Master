

using System.Text.Json;

namespace RunGroopWebApp.Services.Services
{
    public class HttpClientFactoryService(IHttpClientFactory _httpClientFactory,CompaniesClient _companiesClient)
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public async Task Execute()
        {
            //await GetCompaniesWithHttpClientFactory();
            await GetCompaniesWithTypedClient();
        }

        private async Task GetCompaniesWithHttpClientFactory()
        {
            var httpClient = _httpClientFactory.CreateClient("Companies");
        using (var response =await httpClient.GetAsync("Companies", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream =await response.Content.ReadAsStreamAsync();
                var compaines= await JsonSerializer.DeserializeAsync<List<CompanyViewModel>> (stream,_options);
            }
        
        }
        private async Task GetCompaniesWithTypedClient() => await _companiesClient.GetCompanies();
    }
}
