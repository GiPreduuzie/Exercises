using Contracts;
using RestSharp;

namespace FibonacciConsole
{
    public class FibbonacciRestClient
    {
        private readonly RestClient _restClient;
        private readonly Configs _configs;

        public FibbonacciRestClient(Configs configs, RestClient restClient)
        {
            _configs = configs;
            _restClient = restClient;
        }

        public void Init(CalculationInit[] calculationInits)
        {
            var request = new RestRequest("values/init", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(new InitParameters { CalculationInits = calculationInits, Configs = _configs });
            _restClient.Execute(request);
        }

        public void Stop()
        {
            var request = new RestRequest("values/stop", Method.POST) { RequestFormat = DataFormat.Json };
            _restClient.Execute(request);
        }

        public void Next(int queueNumber, string result)
        {
            var request = new RestRequest("values", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddParameter("n", queueNumber);
            request.AddParameter("value", result);

            _restClient.ExecuteAsync(request, x => { });
        }
    }
}