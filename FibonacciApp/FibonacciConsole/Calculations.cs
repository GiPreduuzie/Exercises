using Contracts;
using MassTransit;
using SimpleArbitraryPrecisionArithmetic;
using System;
using System.Linq;

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

        public IBusControl StartCalculations(int queueAmount)
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

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.AutoDelete = true;
                sbc.PurgeOnStartup = true;

                var host = sbc.Host(new Uri(_configs.RabbitMQHost), h =>
                {
                    h.Username(_configs.Username);
                    h.Password(_configs.Password);
                });

                foreach (var calculator in calculators)
                {
                    sbc.ReceiveEndpoint(
                        host,
                        _configs.GetQueueName(calculator.QueueNumber),
                        ep =>
                    {
                        ep.Handler<NextNumberMessage>(context =>
                        {
                            return Console
                            .Out
                            .WriteLineAsync($"from queue: {calculator.QueueNumber} " + context.Message.Value)
                            .ContinueWith(task => calculator.Next(MyLong.FromString(context.Message.Value)));
                        });
                    });
                }
            });

            foreach (var calculator in calculators)
            {
                calculator.Next(MyLong.FromInt(1));
            }

            bus.Start();
            return bus;
        }
    }
}
