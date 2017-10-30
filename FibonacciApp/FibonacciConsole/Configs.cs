using Contracts;
using System;

namespace FibonacciConsole
{
    public class ConfigsProvider
    {
        public Configs GetConfigs()
        {
            return new Configs
            {
                RabbitMQHost = "rabbitmq://localhost",
                Username = "guest",
                Password = "guest",
                QueueNamePattern = "my_command_{0}"
            };
        }
    }
}
