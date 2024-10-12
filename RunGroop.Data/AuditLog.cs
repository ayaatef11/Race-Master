

namespace RunGroop.Data
{
    public class AuditLog
    {
        public int Id { get; set; }
        public required string UserEmail { get; set; }
        public required string EntityName { get; set; }
        public required string Action { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string Changes { get; set; }
    }
}
