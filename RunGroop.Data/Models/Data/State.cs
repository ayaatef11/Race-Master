namespace RunGroop.Data.Models.Data
{
    public class State:Entity
    {
        public int Id { get; set; }
        public string StateName { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;

    }
}
