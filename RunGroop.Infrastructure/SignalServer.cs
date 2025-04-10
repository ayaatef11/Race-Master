using RunGroop.Data.Models.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace RunGroop.Infrastructure
{
    //you can have more than a hub
    public class SignalServer : Hub
    {
        public static int TotalViews { get; set; } = 0;

        public async Task NewWindowLoaded(string name,string mess)
        {

            TotalViews++;
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
            ChatContext db = new();
            Message m = new Message() { Name = name, Messagel = mess, Date = DateTime.Now };
            db.Messages.Add(m);
            db.SaveChanges();
        }
        public async Task SendMessage(string user, string message)
        {
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
            await Clients.All.SendAsync(user, message);//grpc
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

        public void JoinGroup(string gname, string name)
        {

            Groups.AddToGroupAsync(Context.ConnectionId, gname);

            Clients.OthersInGroup(gname).SendAsync(name, gname);
        }
    }
}
