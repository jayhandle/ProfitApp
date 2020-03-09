
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
          
            try
            {
                if (value.Contains("$"))
                {
                    value = value.Split('$')[1];
                }
                var values = value.Split('.');
                var dollars = values[0];
                var cents = "0";
                if (values.Length > 1)
                {
                    cents = value.Split('.')?[1];
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
            catch
            {
                return 0;
            }
        }

        public static string ConvertPenniesToDollars(long value)
        {
            var dollars = Math.DivRem(value, long.Parse("100"), out long cents);
            return $"${dollars}.{Math.Abs(cents).ToString("D2")}";
        }
    }
}
