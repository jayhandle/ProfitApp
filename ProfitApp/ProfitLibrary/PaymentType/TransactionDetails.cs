namespace ProfitLibrary
{
    internal class TransactionDetails : PaymentType
    {
        public override void GetPaymentDetail(string[] values, ref OrderItem orderItem)
        {
            orderItem.SellingFees += PaymentDetail.ConvertDollarstoPennies(values[amount]);
        }
    }
}