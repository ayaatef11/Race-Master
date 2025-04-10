//using RabbitMQ.Client;
//using RunGroopWebApp;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace RunGroop.ServiceBus
//{
//    public class MessageProducer : IMessageProducer
//    {
//        public async Task SendingMessage<T>(T message)
//        {
//            var factory = new ConnectionFactory()
//            {
//                HostName = "localhost",
//                UserName = "user",
//                Password = "mypass",
//                VirtualHost = "/"
//            };

//            // Asynchronously create the connection
//            using var conn = await factory.CreateConnectionAsync();  // Await the async method
//            using var channel = conn.CreateModel();  // Create the channel

//            // Declare the queue with the specified parameters
//            channel.QueueDeclare("bookings", durable: true, exclusive: false);

//            // Serialize the message into JSON and convert it to a byte array
//            var jsonString = JsonSerializer.Serialize(message);
//            var body = Encoding.UTF8.GetBytes(jsonString);

//            // Set the properties to make the message persistent
//            var properties = channel.CreateBasicProperties();
//            properties.Persistent = true;

//            // Publish the message to the queue
//            channel.BasicPublish(exchange: "", routingKey: "bookings", basicProperties: properties, body: body);
//        }
//    }
//}
