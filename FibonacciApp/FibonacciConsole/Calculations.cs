using Contracts;
using MassTransit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleArbitraryPrecisionArithmetic;
using System;
using System.Linq;
using System.Text;

namespace FibonacciConsole
{
    public class Calculations
    {
        private readonly FibbonacciRestClient _fibbonacciRestClient;
        private readonly Configs _configs;

        public Calculations(FibbonacciRestClient fibbonacciRestClient, Configs configs)
        {
            _fibbonacciRestClient = fibbonacciRestClient;
            _configs = configs;
        }

        public void StopCalculations(IBusControl bus)
        {
            _fibbonacciRestClient.Stop();
            bus.Stop();
        }

        public void StartCalculations(int queueAmount)
        {
            var initializers =
            Enumerable
                .Range(0, queueAmount)
                .Select(x => new CalculationInit { QueueNumber = x, Value = "1" })
                .ToArray();

            _fibbonacciRestClient.Init(initializers);

            var calculators =
            initializers
                .Select(x => new Calculator(_fibbonacciRestClient, x.QueueNumber, MyLong.FromInt(1)))
                .ToArray();


            foreach (var calculator in calculators)
            {
                Subscribe(
                    _configs.GetQueueName(calculator.QueueNumber),
                    x =>
                    {
                        Console.WriteLine($"from queue: {calculator.QueueNumber} " + x);
                        calculator.Next(MyLong.FromString(x));
                    });
            }

            foreach (var calculator in calculators)
            {
                calculator.Next(MyLong.FromInt(1));
            }
        }

        private void Subscribe(string queueName, Action<string> handler)
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    handler(message);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

            }
        }
    }
}
