
namespace ProfitLibrary
{
    public class Other : PaymentType
    {
        public override void GetPaymentDetail(string[] values, ref OrderItem orderItem)
        {
            orderItem.SoldFor += PaymentDetail.ConvertDollarstoPennies(values[amount]);

            //pd = null;
            //switch (values[payment_detail])
            //{
            //    case Shipping:
            //        orderItem.SoldFor += PaymentDetail.ConvertDollarstoPennies(values[amount]);

            //        break;
            //    case Product_Tax:
            //        orderItem.SoldFor += PaymentDetail.ConvertDollarstoPennies(values[amount]);
            //        break;
            //    case Shipping_Tax:
            //        orderItem.SoldFor += PaymentDetail.ConvertDollarstoPennies(values[amount]);

            //        break;
            //}

            //pd.GetAmount(values, ref orderItem);
        }
    }
}
