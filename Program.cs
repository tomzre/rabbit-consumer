using System;
using System.Text;
using RabbitMQ.Client.Events;

namespace RabbitConsumer.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var connectionFactory = new RabbitMQ.Client.ConnectionFactory();

            var connection = connectionFactory.CreateConnection();

            var channel = connection.CreateModel();
            
            channel.QueueDeclare(queue: "identity.user",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: "identity.user",
                consumerTag: "consumer",
                noLocal: false,
                arguments: null,
                exclusive: true,
                autoAck: true,
                consumer: consumer);
            
            Console.ReadLine();
        }
    }
}
