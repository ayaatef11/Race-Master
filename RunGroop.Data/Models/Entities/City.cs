using RunGroop.Data.Contracts;

namespace RunGroop.Data.Models.Data
{
    public class City:Entity
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public int Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Country { get; set; } = string.Empty;
        public string TenantId { get ; set ; }=string.Empty;
    }
}
