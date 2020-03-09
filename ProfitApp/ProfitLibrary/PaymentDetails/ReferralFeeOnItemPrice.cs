
namespace ProfitLibrary
{
    public class ReferralFeeOnItemPrice : PaymentDetail
    {
        public override void GetAmount(string[] values, ref OrderItem orderItem)
        {
            orderItem.ItemCost += ConvertDollarstoPennies(values[amount]);
        }
    }
}
