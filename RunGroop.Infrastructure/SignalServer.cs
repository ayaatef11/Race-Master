using Microsoft.AspNetCore.SignalR;

namespace RunGroop.Infrastructure
{
    public  class SignalServer:Hub
    {
public static int TotalViews { get; set; } = 0;

        public async Task NewWindowLoaded()
        {

            TotalViews++;
            //send update to all clients that total views have been updated
            await Clients.All.SendAsync("updateTotalViews|", TotalViews);
        }
        public async Task SendMessage(string user, string message)
        {
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);// the text receive message is the notification received 
        }
    }
}
