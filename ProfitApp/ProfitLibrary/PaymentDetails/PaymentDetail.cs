
using System;

namespace ProfitLibrary
{
    public abstract class PaymentDetail
    {
        protected const int amount = 6;
        public abstract void GetAmount(string[] values, ref OrderItem orderItem);

        public static long ConvertDollarstoPennies(string value)
        {
            if (value.Contains("$"))
            {
                value = value.Split('$')[1];
            } 
            var dollars = value.Split('.')[0];
            var cents = "0";
            if (value.Contains("."))
            {
                cents = value.Split('.')[1];
            }
            var longDollar = int.Parse(dollars) * 100;
            var negative = dollars.Contains("-");
            if (negative)
            {
                longDollar *= -1;
            }

            var longCent = int.Parse(cents);
            var pennies = longDollar + longCent;
            return pennies = negative ? pennies * -1 : pennies;
        }

        public static string ConvertPenniesToDollars(long value)
        {
            var dollars = Math.DivRem(value, long.Parse("100"), out long cents);
            return $"${dollars}.{Math.Abs(cents).ToString("D2")}";
        }
    }
}
