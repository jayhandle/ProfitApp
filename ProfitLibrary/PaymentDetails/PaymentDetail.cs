
using System;

namespace ProfitLibrary
{
    public abstract class PaymentDetail
    {
        protected const int amount = 6;
        public abstract void GetAmount(string[] values, ref OrderItem orderItem);

        public static long ConvertDollarstoPennies(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            if (value.Contains("$"))
            {
                value = value.Split('$')[1];
            } 
            var dollars = value.Split('.')[0];
            var cents = "0";
            if (value.Contains("."))
            {
                cents = value.Split('.')[1];
                if(cents.Length<2)
                {
                    cents += "0";
                }
            }
            var longDollar = int.Parse(dollars) * 100;
            var negative = dollars.Contains("-");
            if (negative)
            {
                longDollar *= -1;
            }

            var longCent = int.Parse(cents);
            var pennies = longDollar + longCent;
            return pennies = negative ? MakeNegative(pennies): pennies;
        }

        private static int MakeNegative(int pennies)
        {
            if(pennies < 0 )
            {
                return pennies;
            }

            return pennies * -1;
        }

        private static string MakeNegative(string value)
        {
            if (value.Contains("-"))
            {
                return value; 
            }
            value = value.Insert(1, "-");
            return value;
        }
        public static string ConvertPenniesToDollars(long value)
        {
            var dollars = Math.DivRem(value, long.Parse("100"), out long cents);
            var d = $"${dollars}.{Math.Abs(cents).ToString("D2")}";
            if (value < 0)
            {
                d = MakeNegative(d);
            }
            return d;
        }
    }
}
