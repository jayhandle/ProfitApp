namespace ProfitLibrary
{
    internal class PromoRebates : PaymentType
    {
        public override void GetPaymentDetail(string[] values, ref OrderItem orderItem)
        {
            switch (values[payment_detail])
            {
                case Empty_String:
                    orderItem.SoldFor += PaymentDetail.ConvertDollarstoPennies(values[amount]);
                    break;
                    case Shipping:
                    orderItem.SellingFees += PaymentDetail.ConvertDollarstoPennies(values[amount]);
                    break;
            }
        }
    }
}