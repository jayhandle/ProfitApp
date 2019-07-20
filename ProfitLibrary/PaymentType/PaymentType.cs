
namespace ProfitLibrary
{
    public abstract class PaymentType
    {
        protected const string Empty_String = "";
        protected const string Shipping = "Shipping";
        protected const string Referral_Fee_On_Item_Price = "Referral Fee on Item Price";
        protected const string Shipping_Commission = "Shipping commission";
        protected const string Transaction_Fee = "Transaction Fee";
        protected const string FBA_Pick_Pack_Fee = "FBA Pick & Pack Fee";
        protected const string FBA_Inventory_Storage_Fee = "FBA Inventory Storage Fee";
        protected const string Subscription_Fee = "Subscription Fee";
        protected const string Product_Tax = "Product Tax";
        protected const string Shipping_Tax = "Shipping tax";

        protected const int payment_detail = 5;
        protected const int amount = 6;
        protected PaymentDetail pd = null;
        public abstract void GetPaymentDetail(string[] values, ref OrderItem orderItem);
    }
}
