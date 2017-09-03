using FunctionalExtentions.ValueCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var list = new ArrayList<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            list.RemoveRange(2, 5);

            Console.ReadKey();
        }
    }
}