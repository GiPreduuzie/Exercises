using Contracts;
using RestSharp;
using SimpleArbitraryPrecisionArithmetic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FibonacciConsole
{
    public class Program
    {
        static IDictionary<int, MyLong> _values =
            Enumerable.Range(0, 15).ToDictionary(x => x, x => MyLong.FromInt(1));

        public static void Main()
        {
            Console.WriteLine("How many calculations you want to start?");
            var queueAmount = int.Parse(Console.ReadLine());

            if (queueAmount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (queueAmount == 0)
            {
                return;
            }

            Console.WriteLine("Starting...");

            var configs = new ConfigsProvider().GetConfigs();
            var client = new RestClient("http://localhost:55305/api");
            var calculations = new Calculations(
                new FibbonacciRestClient(configs, client),
                configs);
            var bus = calculations.StartCalculations(queueAmount);

            Console.WriteLine("Started. Press any key to exit");
            Console.ReadKey();

            Console.WriteLine("Stopping...");
            calculations.StopCalculations(bus);
        }
    }
}
