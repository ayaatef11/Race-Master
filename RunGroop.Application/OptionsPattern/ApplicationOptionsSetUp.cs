using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace RunGroop.Application.OptionsPattern
{
    /*
       ApplicationOptions: A class to hold configuration values in a strongly typed manner.
ApplicationOptionsSetup: Configures and binds configuration values to the ApplicationOptions class 
    from your app configuration.
These classes improve code maintainability, readability, testability, and follow the Dependency
    Injection (DI) principles of ASP.NET Core, 
    making configuration management easier and more robust.     
     */
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