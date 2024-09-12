
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace RunGroop.Infrastructure
{
    public  class NotificationHub:Hub
    {
      public async Task SendNotification(string message)
    {
        
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

    public async Task SendNotificationToUser(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
    }
}

