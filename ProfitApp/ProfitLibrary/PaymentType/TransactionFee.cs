namespace ProfitLibrary
{
    internal class TransactionFee : PaymentDetail
    {
        public override void GetAmount(string[] values, ref OrderItem orderItem)
        {
            orderItem.SellingFees += ConvertDollarstoPennies(values[amount]);
        }
    }
}