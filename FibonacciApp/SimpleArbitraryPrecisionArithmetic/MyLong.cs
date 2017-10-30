using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleArbitraryPrecisionArithmetic
{
    public struct MyLong
    {
        public byte[] Value;

        public static MyLong FromString(string value)
        {
            return new MyLong { Value = value.Select(x => byte.Parse(x.ToString())).Reverse().ToArray() };
        }

        public static MyLong FromInt(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"[{nameof(value)}:{value}]");
            }

            if (value == 0)
            {
                return new MyLong { Value = new byte[] { 0 } };
            }

            var result = new List<byte>();
            while (value != 0)
            {
                result.Add((byte)(value % 10));
                value /= 10;
            }

            return new MyLong { Value = result.ToArray() };
        }

        public static MyLong operator +(MyLong left, MyLong right)
        {
            var shortArray = left.Value;
            var longArray = right.Value;
            if (left.Value.Length > right.Value.Length)
            {
                longArray = left.Value;
                shortArray = right.Value;
            }

            var minLength = Math.Min(left.Value.Length, right.Value.Length);

            var result = new List<byte>();
            var additional = 0;
            for (int i = 0; i < minLength; i++)
            {
                var value = shortArray[i] + longArray[i] + additional;
                additional = value / 10;
                result.Add((byte)(value % 10));
            }

            for (int i = minLength; i < longArray.Length; i++)
            {
                var value = longArray[i] + additional;
                additional = value / 10;
                result.Add((byte)(value % 10));
            }

            if (additional != 0)
            {
                result.Add((byte)additional);
            }

            return new MyLong { Value = result.ToArray() };
        }

        public override string ToString()
        {
            return string.Join("", Value.Select(x => x.ToString()).Reverse());
        }
    }
}
