﻿using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;

namespace RunGroop.Application.ViewModels
{
    public class EditClubViewModel
    {
        public int Id{ get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public  IFormFile? Image { get; set; }
        public string? URL { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
