namespace QueueTests
{
    public class Result<T>
    {
        public Result(T value, bool exists)
        {
            Exists = exists;
            Value = value;
        }

        public bool Exists;
        public T Value;
    }
}
