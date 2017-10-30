using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using SimpleArbitraryPrecisionArithmetic;
using Contracts;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static IDictionary<int, MyLong> _values;
        private static IBusControl _bus;
        private static Configs _configs;

        [HttpPost("init")]
        public void Init([FromBody] InitParameters initParameters)
        {
                _configs = initParameters.Configs;
                _values = initParameters
                    .CalculationInits
                    .ToDictionary(x => x.QueueNumber, x => MyLong.FromString(x.Value));

                _bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    var host = sbc.Host(new Uri(_configs.RabbitMQHost), h =>
                    {
                        h.Username(_configs.Username);
                        h.Password(_configs.Password);
                    });
                });

                _bus.Start();
        }

        [HttpGet]
        public string Get()
        {
            return _values == null ? "< nothing >" : string.Join("; ", _values.Select(x => $"[{x.Key} : {x.Value}]"));
        }

        [HttpPost("stop")]
        public void Stop()
        {
            _bus?.StopAsync();
        }

        [HttpPost]
        public async void Post(int n, string value)
        {
            System.IO.File.AppendAllText($"C:\\Temp\\log_{n}.log", $"{n}:{value}" + Environment.NewLine);

            var result = _values[n] + MyLong.FromString(value);
            _values[n] = result;

            var endpoint = await _bus.GetSendEndpoint(new Uri(_configs.GetQueueName(n)));
            await endpoint.Send(new NextNumberMessage() { Value = result.ToString() });
        }
    }
}