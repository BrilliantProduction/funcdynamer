using FunctionalExtentions;
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
            //var list = new ArrayList<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //list.RemoveRange(2, 5);

            Console.WriteLine("Account is implicitly castable to AnotherMoney? {0}", typeof(Account).IsImplicitlyCastableTo(typeof(AnotherMoney)));

            Console.WriteLine("Int32 is implicitly castable to AnotherMoney? {0}", typeof(int).IsImplicitlyCastableTo(typeof(AnotherMoney)));

            Console.WriteLine("Double is implicitly castable to AnotherMoney? {0}", typeof(double).IsImplicitlyCastableTo(typeof(AnotherMoney)));

            Console.WriteLine("Double is explicitly castable to AnotherMoney? {0}", typeof(double).IsCastableTo(typeof(AnotherMoney)));

            Console.WriteLine("Array of account is explicitly castable to IList AnotherMoney? {0}", typeof(Account[]).IsCastableTo(typeof(IList<AnotherMoney>)));

            Console.ReadKey();
        }
    }
}