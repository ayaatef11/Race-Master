using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace RunGroop.Data.Models.Localization
{
    //used to take the instance of json string localizer
    public class JsonStringLocalizerFactory(IDistributedCache _cache) : IStringLocalizerFactory
    {

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_cache);
        }
    }
}
