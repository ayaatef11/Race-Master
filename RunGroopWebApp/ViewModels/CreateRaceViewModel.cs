using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;

namespace RunGroop.Application.ViewModels
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Distance { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string AppUserId { get; set; } = string.Empty;
    }
}
