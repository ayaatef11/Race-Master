using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;

namespace RunGroopWebApp.ViewModels
{
    public class EditRaceViewModel
    {
        public string Date { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Distance { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
    }
}
