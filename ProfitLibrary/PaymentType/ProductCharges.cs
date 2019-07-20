
namespace ProfitLibrary
{
    public class ProductCharges : PaymentType
    {
        public override void GetPaymentDetail(string[] values, ref OrderItem orderItem)
        { 
            int quantity = 7;
            orderItem.SoldFor += PaymentDetail.ConvertDollarstoPennies(values[amount]);
            orderItem.QuantitySold = int.Parse(values[quantity]);
        }
    }
}
