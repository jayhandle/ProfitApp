namespace ProfitLibrary
{
    internal class ShippingServiceCharges : PaymentType
    {
        public override void GetPaymentDetail(string[] values, ref OrderItem orderItem)
        {
            orderItem.ShippingCost += PaymentDetail.ConvertDollarstoPennies(values[amount]);

            //switch (values[payment_detail])
            //{
            //    case "Delivery Confirmation":
            //        orderItem.ShippingCost += PaymentDetail.ConvertDollarstoPennies(values[amount]);
            //        break;
            //    case "Shipping Label":
            //        orderItem.ShippingCost += PaymentDetail.ConvertDollarstoPennies(values[amount]);
            //        break;
            //}
        }
    }
}