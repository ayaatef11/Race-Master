﻿
namespace RunGroop.Data.Models.Data
{
    public class Address: Entity
    {
       public int Id { get; set; }  
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string TenantId { get; set; } = string.Empty;

    }
}
