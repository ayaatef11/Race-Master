using RunGroopWebApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroopWebApp.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }=string.Empty;
        public string? Image { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public int? EntryFee { get; set; }
        public string? Website { get; set; }=string.Empty ;
        public string? Twitter { get; set; } = string.Empty;
        public string? Facebook { get; set; } = string.Empty;
        public string? Contact { get; set; }=string.Empty;
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; } = string.Empty;
        public AppUser? AppUser { get; set; }
    }
}
