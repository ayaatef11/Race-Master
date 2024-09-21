using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;
using System.Xml.Linq;

namespace RunGroop.Infrastructure
{
    //this is the server and the client is the js file 
    //you can have more than a hub
    //default proxy is created for all the classes 
    [HubName("chat")]
    public class SignalServer : Hub
    {
        public static int TotalViews { get; set; } = 0;

        public async Task NewWindowLoaded()
        {

            TotalViews++;
            //send update to all clients that total views have been updated
            await Clients.All.SendAsync("updateTotalViews|", TotalViews);
            ChatContext db = new ChatContext();
            message m = new message() { name = name, messagel = mess, date = DateTime.Now };
            db.messages.Add(m);
            db.SaveChanges();
        }
        public async Task SendMessage(string user, string message)
        {
            //the protocol is new procedural protocol
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);// the text receive message is the notification received 

        }
        public async Task SendMessagespecific(string user, string message)
        {
            //send message to specific user that intiated the request 
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);

        }

        public async Task SendMessagespecificAnotherClient(string user, string message)
        {
            //send message to specific user how doesn;t initate the request
            await Clients.Client("Connection Id - A").SendAsync("ReceiveMessage", user, message);
            await Clients.All.newMessage(user, message);//grpc
        }

        public async Task SendMessageExcept(string user, string message)
        {
            //send message to all users except a and c 
            await Clients.AllExcept("Connection Id - A", "Connection Id - C").SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendSpecificType(string user, string message)
        {
            //send message to all users of type Aya@gmail
            await Clients.User("Aya@gmail").SendAsync("ReceiveMessage", user, message);
        }

        public void joimGroup(string gname, string name)
        {

            Groups.Add(Context.ConnectionId, gname);

            Clients.OthersInGroup(gname).newMemeber(name, gname);
        }
    }
}
