
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

/*client to receive notifications
 
 <!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>SignalR Notifications</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.9/signalr.min.js"></script>
</head>
<body>
    <h2>Real-Time Notifications</h2>
    <ul id="notificationsList"></ul>

    <script>
        // Connect to the notification hub
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/notificationHub")
            .build();

        // Listen for notifications
        connection.on("ReceiveNotification", (message) => {
            const li = document.createElement("li");
            li.textContent = message;
            document.getElementById("notificationsList").appendChild(li);
        });

        // Start the connection
        connection.start().catch(err => console.error(err.toString()));
    </script>
</body>
</html>
*/