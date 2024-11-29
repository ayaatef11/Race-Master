
using Microsoft.AspNetCore.SignalR;
namespace RunGroop.Infrastructure
{
    public  class NotificationHub:Hub<INotificationClient>//stronlgly typed hub
    {
      public async Task SendNotification(string message)
    {
        //if it is weakly typed hub
        //await Clients.All.SendAsync("ReceiveNotification", message);
            await Clients.Client(Context.ConnectionId).ReceiveNotification("hello");
    }

    public async Task SendNotificationToUser(string userId, string message)
    {
        await Clients.User(userId).ReceiveNotification( message);
    }
    }
}

//client interface
public interface INotificationClient
{
    Task ReceiveNotification(string message);
}