
using RabbitMQ.Client;
using RunGroopWebApp;
using System.Text;
using System.Text.Json;
namespace RunGroop.ServiceBus;

/*RabbitMQ: A message broker that allows applications to communicate asynchronously
 * by sending messages through queues.
Message Producer: This class acts as the producer, responsible for publishing messages 
to a RabbitMQ queue.
JSON Serialization: The message sent to the queue is serialized into JSON format.
Queue Durability and Persistence: Messages and queues are configured to persist across
RabbitMQ restarts*/
public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "mypass",
            VirtualHost = "/"
        };

        // Ensure the connection is properly disposed
        using var conn = factory.CreateConnection();
        using var channel = conn.CreateModel();
   /* QueueDeclare: Declares a queue named "bookings" in RabbitMQ.
durable: true: Ensures that the queue persists even if RabbitMQ restarts.
exclusive: false: The queue can be used by multiple connections.*/
        channel.QueueDeclare("bookings", durable: true, exclusive: false);

        // Serialize the message to JSON
        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        // Ensure messages are persistent
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        // Publish message to the queue
        channel.BasicPublish(exchange: "", routingKey: "bookings", basicProperties: properties, body: body);
    }
}

