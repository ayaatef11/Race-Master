using Microsoft.AspNetCore.Identity;
using RunGroop.Data.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroop.Data.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Club>? Clubs { get; set; }
        public ICollection<Race>? Races { get; set; }
    }
}
