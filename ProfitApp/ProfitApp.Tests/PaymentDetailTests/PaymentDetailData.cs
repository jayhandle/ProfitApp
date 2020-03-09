namespace ProfitLibrary.Test
{
    public class PaymentDetailData
    {
        public struct ConvertDollarsToPenniesData
        {
            public string Title;
            public string StringValue;
            internal long Pennies;
        }

        public static readonly ConvertDollarsToPenniesData[] ConvertDollarsToPenniesDatas =
        {
            new ConvertDollarsToPenniesData
            {
                Title = "nothing",
                Pennies = 0
            },
            new ConvertDollarsToPenniesData
            {
                Title = "number with no $",
                StringValue = "5",
                Pennies = 500
            },
            new ConvertDollarsToPenniesData
            {
                Title = "number with$",
                StringValue = "$5",
                Pennies = 500
            },
            new ConvertDollarsToPenniesData
            {
                Title = "number with $ and cents",
                StringValue = "$5.12",
                Pennies = 512
            },
            new ConvertDollarsToPenniesData
            {
                Title = "number with $ and cents",
                StringValue = "$5.12",
                Pennies = 512
            },
            new ConvertDollarsToPenniesData
            {
                Title = "$ and cents",
                StringValue = "$0.12",
                Pennies = 12
            },
            new ConvertDollarsToPenniesData
            {
                Title = " No $ and cents",
                StringValue = "0.12",
                Pennies = 12
            },
            new ConvertDollarsToPenniesData
            {
                Title = "not number",
                StringValue = "A",
                Pennies = 0
            },
        };
    }
}