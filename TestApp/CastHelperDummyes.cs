using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class Account
    {
        public int Money { get; set; }
        public string Owner { get; set; }
    }

    public class AnotherMoney
    {
        public int Money { get; set; }
        public string Owner { get; set; }

        public static AnotherMoney operator +(AnotherMoney money, AnotherMoney other)
        {
            return new AnotherMoney { Money = money.Money + other.Money };
        }

        public static implicit operator AnotherMoney(Account money)
        {
            return new AnotherMoney() { Money = money.Money, Owner = money.Owner };
        }

        public static implicit operator AnotherMoney(int money)
        {
            return new AnotherMoney { Money = money };
        }

        public static explicit operator AnotherMoney(double money)
        {
            return new AnotherMoney { Money = (int)money };
        }
    }
}