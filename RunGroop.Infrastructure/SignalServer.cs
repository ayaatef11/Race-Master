

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
- `builder.Services.AddSignalR()` registers SignalR services with the dependency injection container.
- `app.MapHub<ChatHub>("/chatHub")` maps the `ChatHub` endpoint to `/chatHub`.

### **Step 4: Create a Client to Connect to the Hub**

To test your SignalR server, you can create a simple HTML page that connects to the Hub.

1. **Create an HTML Page**:

In the `wwwroot` folder, create an HTML file named `index.html`:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>SignalR Chat</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.9/signalr.min.js"></script>
</head>
<body>
    <h2>SignalR Chat</h2>
    <input id="userInput" type="text" placeholder="Name" />
    <br />
    <input id="messageInput" type="text" placeholder="Message" />
    <button onclick="sendMessage()">Send</button>
    <ul id="messagesList"></ul>

    <script>
        // Connect to the hub
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .build();

        // Receive messages from the hub
        connection.on("ReceiveMessage", (user, message) => {
            const li = document.createElement("li");
            li.textContent = `${user}: ${message}`;
            document.getElementById("messagesList").appendChild(li);
        });

        // Start the connection
        connection.start().catch(err => console.error(err.toString()));

        // Send messages to the hub
        function sendMessage() {
            const user = document.getElementById("userInput").value;
            const message = document.getElementById("messageInput").value;
            connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
        }
    </script>
</body>
</html>
*/