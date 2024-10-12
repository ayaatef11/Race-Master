
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace RunGroop.Infrastructure
{
    public  class NotificationHub:Hub<INotificationClient>
    {
      public async Task SendNotification(string message)
    {
        //if it is weal=ky typed hub
        //await Clients.All.SendAsync("ReceiveNotification", message);
            await Clients.Client(Context.ConnectionId).ReceiveNotification("hello");
    }

    public async Task SendNotificationToUser(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
    }
}

//client interface
public interface INotificationClient
{
    Task ReceiveNotification(string message);
}