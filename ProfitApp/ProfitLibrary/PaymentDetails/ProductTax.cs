namespace ProfitLibrary
{
    internal class ProductTax : PaymentDetail
    {
        public override void GetAmount(string[] values, ref OrderItem orderItem)
        {
            orderItem.SoldFor += ConvertDollarstoPennies(values[amount]);
        }
    }
}