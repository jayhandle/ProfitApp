namespace ProfitLibrary
{
    internal class FBAInventoryStorageFee : PaymentDetail
    {
        public override void GetAmount(string[] values, ref OrderItem orderItem)
        {
            orderItem.SellingFees += ConvertDollarstoPennies(values[amount]);
        }
    }
}