using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleArbitraryPrecisionArithmetic;
using Contracts;
using System.Text;
using RabbitMQ.Client;
using FibonacciApi;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static IDictionary<int, MyLong> _values;
        private static Configs _configs;

        [HttpPost("init")]
        public void Init([FromBody] InitParameters initParameters)
        {
            _configs = initParameters.Configs;
            _values = initParameters
                .CalculationInits
                .ToDictionary(x => x.QueueNumber, x => MyLong.FromString(x.Value));
        }

        [HttpGet]
        public string Get()
        {
            return _values == null ? "< nothing >" : string.Join("; ", _values.Select(x => $"[{x.Key} : {x.Value}]"));
        }

        [HttpPost]
        public void Post(int n, string value)
        {
            System.IO.File.AppendAllText($"C:\\Temp\\log_{n}.log", $"{n}:{value}" + Environment.NewLine);

            var message = MyLong.FromString(value) + _values[n];
            new MessageSender(_configs).SendMessage(message.ToString(), _configs.GetQueueName(n));
        }
    }
}