using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts2
{
    public struct MyLong
    {
        public string Value;

        public static MyLong operator +(MyLong left, MyLong right)
        {
            try
            {
                var maxLength = Math.Max(left.Value.Length, right.Value.Length);

                var leftR = left.Value.PadLeft(maxLength, '0').Reverse().ToArray();
                var rightR = right.Value.PadLeft(maxLength, '0').Reverse().ToArray();

                var minLength = Math.Min(leftR.Length, rightR.Length);
                var result = new List<int>();
                var additional = 0;
                for (int i = 0; i < maxLength; i++)
                {
                    var value = leftR[i] + rightR[i] - '0' * 2 + additional;
                    additional = value / 10;
                    result.Add(value % 10);
                }

                if (additional != 0)
                {
                    result.Add(additional);
                }

                result.Reverse();
                return new MyLong { Value = new string(result.Select(x => (char)(x + '0')).ToArray()) };
            }
            catch(Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Temp\error.log", $"Error plusing {left.Value} + {right.Value} " + ex.Message);
                throw ex;
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class MyMessage
    {
        public MyLong Value { get; set; }
    }
}
