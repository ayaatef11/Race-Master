
using RabbitMQ.Client;
using RunGroopWebApp;
using System.Text;
using System.Text.Json;
namespace RunGroop.ServiceBus;

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

        using var conn = factory.CreateConnection();
        using var channel = conn.CreateModel();
        channel.QueueDeclare("bookings", durable: true, exclusive: false);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "", routingKey: "bookings", basicProperties: properties, body: body);
    }
}

