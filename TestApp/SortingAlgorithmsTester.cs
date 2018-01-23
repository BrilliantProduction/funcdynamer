using FunctionalExtentions.ValueCollections.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    internal class SortingAlgorithmsTester
    {
        public static void DoTest()
        {
            var collection = new List<int>();
            FillRandomData(collection);

            var sorting = new IterativeMergeSortAlgorithm();
            sorting.Sort(collection, SortDirection.Down);
            PrintCollection(collection);
        }

        private static void PrintCollection<T>(ICollection<T> collection)
        {
            int i = 0;
            foreach (var item in collection)
            {
                i++;
                Console.WriteLine($"{i}-th item is: {item}");
            }
        }

        private static void FillRandomData(List<int> target)
        {
            var random = new Random();
            var size = random.Next(10, 100);
            for (int i = 0; i < size; i++)
            {
                target.Add(random.Next(0, 5000));
            }
        }
    }
}