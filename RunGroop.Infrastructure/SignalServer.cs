using Microsoft.AspNetCore.SignalR;

namespace RunGroop.Infrastructure
{
    public  class SignalServer:Hub
    {

        public async Task SendMessage(string user, string message)
        {
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

/*
 builder.Services.AddSignalR() registers SignalR services with the dependency injection container.
 app.MapHub<ChatHub>("/chatHub") maps the `ChatHub` endpoint to `/chatHub`.
*/