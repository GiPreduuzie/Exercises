using SimpleArbitraryPrecisionArithmetic;

namespace FibonacciConsole
{
    public class Calculator
    {
        FibbonacciRestClient _client;

        public Calculator(FibbonacciRestClient client, int queueNumber, MyLong value)
        {
            _client = client;
            QueueNumber = queueNumber;
            Value = value;
        }

        public int QueueNumber { get; }
        public MyLong Value { get; private set; }

        public void Next(MyLong newValue)
        {
            var result = Value + newValue;
            Value = result;

            _client.Next(QueueNumber, Value.ToString());
        }
    }
}
