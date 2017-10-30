using System;

namespace Contracts
{
    public class Configs
    {
        public string RabbitMQHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueNamePattern { get; set; }
        public string GetQueueName(int queueNumber) => string.Format(QueueNamePattern, queueNumber);
    }
}
