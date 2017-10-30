using Contracts;
using RabbitMQ.Client;
using System.Text;

namespace FibonacciApi
{
    public class MessageSender
    {
        private Configs _configs;

        public MessageSender(Configs configs)
        {
            _configs = configs;
        }

        public void SendMessage(string message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = _configs.RabbitMQHost };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                              routingKey: queueName,
                              basicProperties: null,
                              body: body);
            }
        }
    }
}
