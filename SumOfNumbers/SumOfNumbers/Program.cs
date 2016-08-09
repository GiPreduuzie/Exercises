using System;
using System.Collections.Generic;
using System.Linq;

namespace SumOfNumbers
{
    public interface IPartitionSearcher
    {
        IEnumerable<Tuple<int, int>> Search(IEnumerable<int> collection, int sum);
    }

    public class PartitionSearcher : IPartitionSearcher
    {
        public IEnumerable<Tuple<int, int>> Search(IEnumerable<int> collection, int sum)
        {
            var array = collection.OrderBy(x => x).ToArray();

            if (array.Length > 0)
            {
                int leftIndex = 0;
                int rightIndex = array.Length - 1;

                while (leftIndex < rightIndex)
                {
                    var treshold = sum - array[leftIndex];

                    while (array[rightIndex] > treshold)
                    {
                        rightIndex--;
                    }

                    if (array[rightIndex] == treshold)
                        yield return Tuple.Create(array[rightIndex], array[leftIndex]);

                    leftIndex++;
                }
            }
        }
    }
}
