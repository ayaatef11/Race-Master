using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace RunGroop.Application.OptionsPattern
{
  
    public class ApplicationOptionsSetup(IConfiguration configuration) : IConfigureOptions<ApplicationOptions>
    {

        private readonly IConfiguration _configuration=configuration;
        public void Configure(ApplicationOptions options)
        {

            _configuration.GetSection(nameof(ApplicationOptions))
            .Bind(options);
        }
    }
}