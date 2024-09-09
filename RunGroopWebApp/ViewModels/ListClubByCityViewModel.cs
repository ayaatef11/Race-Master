using RunGroop.Data.Models.Data;

namespace RunGroopWebApp.ViewModels
{
    public class ListClubByCityViewModel
    {
        public IEnumerable<Club>? Clubs { get; set; }
        public bool NoClubWarning { get; set; } = false;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
