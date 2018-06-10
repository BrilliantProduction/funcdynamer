using FunctionalExtentions;
using FunctionalExtentions.Core;
using FunctionalExtentions.Collections;
using FunctionalExtentions.Collections.Queues;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionalExtentions.Dependencies;

namespace TestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var list = new ArrayList<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //list.RemoveRange(2, 5);

            //Console.WriteLine("Account is implicitly castable to AnotherMoney? {0}", typeof(Account).IsImplicitlyCastableTo(typeof(AnotherMoney)));

            //Console.WriteLine("Int32 is implicitly castable to AnotherMoney? {0}", typeof(int).IsImplicitlyCastableTo(typeof(AnotherMoney)));

            //Console.WriteLine("Double is implicitly castable to AnotherMoney? {0}", typeof(double).IsImplicitlyCastableTo(typeof(AnotherMoney)));

            //Console.WriteLine("Double is explicitly castable to AnotherMoney? {0}", typeof(double).IsCastableTo(typeof(AnotherMoney)));

            //Console.WriteLine("Array of account is explicitly castable to IList AnotherMoney? {0}", typeof(Account[]).IsCastableTo(typeof(IList<AnotherMoney>)));

            //var rnd = new Random();
            //for (int i = 0; i < 10000; i++)
            //{
            //    var stopWatch = new Stopwatch();
            //    stopWatch.Start();
            //    Optional<Optional<Optional<int>>> opt = Optional.From<Optional<Optional<int>>>(5);
            //    var value = opt.Cast<int>();
            //    stopWatch.Stop();
            //    if ((i + 1) % 100 == 0)
            //        Console.WriteLine($"{value} {i + 1}-th iteration elapsed timespan: {stopWatch.ElapsedTicks}");
            //}

            //SortingAlgorithmsTester.DoTest();

            var my = new MyClass();

            var deque = new Deque<int>();
            for (int i = 0; i < 50; i++)
            {
                deque.AddFirst(i);
            }
            Console.WriteLine(string.Join(", ", deque));

            Console.ReadKey();
        }
    }
}