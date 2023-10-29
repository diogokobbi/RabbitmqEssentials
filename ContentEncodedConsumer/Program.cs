using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using BrotliSharpLib;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "compressed-messages",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine(" [*] Waiting for compressed messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{    
    if (ea.BasicProperties.ContentEncoding == "gzip")
    {
        byte[] compressedData = ea.Body.ToArray();
        byte[] uncompressedData = Brotli.DecompressBuffer(compressedData, 0, compressedData.Length);
        var message = Encoding.UTF8.GetString(uncompressedData);
        Console.WriteLine($" [x] Received and decompressed {message.Substring(0, 15)}...");
        Console.WriteLine($" ==== compressed message size: {compressedData.Length} bytes");
    }
    else
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] Received {message.Substring(0, 15)}...");
        Console.WriteLine($" ==== uncompressed message size: {body.Length} bytes");
    }
    
};
channel.BasicConsume(queue: "compressed-messages",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();