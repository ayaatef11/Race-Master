using RunGroop.Data.Models.Identity;
using RunGroopWebApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RunGroop.Data.Models.Data
{
    public class Race
    {
        public int Id { get; set; }
        public string Date { get; set; }=string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Distance { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address? Location { get; set; }=new Address();
        public RaceCategory RaceCategory { get; set; }
        //[ForeignKey("AppUser")]
        //public string? AppUserId { get; set; } = string.Empty;
        //public AppUser? AppUser { get; set; }

    }
}
