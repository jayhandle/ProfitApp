
namespace ProfitLibrary
{
    public class AmazonFees : PaymentType
    {      
        public override void GetPaymentDetail(string[] values, ref OrderItem orderItem)
        {
            orderItem.SellingFees += PaymentDetail.ConvertDollarstoPennies(values[amount]);
            //pd = null;
            //switch (values[payment_detail])
            //{
            //    case Referral_Fee_On_Item_Price:
            //        orderItem.SellingFees += PaymentDetail.ConvertDollarstoPennies(values[amount]);
            //        break;
            //    case Shipping_Commission:
            //        pd = new ShippingCommission();                   
            //        break;
            //    case Transaction_Fee:
            //        pd = new TransactionFee();
            //        break;
            //    case FBA_Pick_Pack_Fee:
            //        pd = new FBAPickPackFee();
            //        break;
            //    case FBA_Inventory_Storage_Fee:
            //        pd = new FBAInventoryStorageFee();
            //        break;
            //    case Subscription_Fee:
            //        pd = new SubscriptionFee();
            //        break;
            //}
           
            //pd.GetAmount(values, ref orderItem);
        }  
    }
}
