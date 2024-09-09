namespace RunGroop.Data.Models.SignalR
{
    public class Notification
    {
        public int NotificationId { get; set; } = 0;
        public int FromUserId { get; set; } = 0;
        public int ToUserId { get; set; } = 0;
        public string NotificationHeader { get; set; } = string.Empty;//type: Alert", "Message", "Warning"
        public string NotificationBody { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Message { get; set; } = string.Empty;
        public string CreatedDateSt => CreatedDate.ToString("dd-MMM-yyyy HH:mm:ss");
        public string IsReadSt => IsRead ? "YES" : "NO";
        public string FromUserName { get; set; } = string.Empty;
        public string ToUserName { get; set; } = string.Empty;
    }
}
