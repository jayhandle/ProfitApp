using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitLibrary
{
    public class TransactionType
    {
        protected const string Amazon_fees = "Amazon fees";
        protected const string Product_charges = "Product charges";
        protected const string Other = "Other";
        protected const string Shipping_Service_Charges = "Shipping Service Charges";
        protected const string Promo_rebates = "Promo rebates";
        protected const string Transaction_Details = "Transaction Details";
        protected const int payment_type = 4;
        internal PaymentType paymentType;
        public void GetPaymentType(string[] values, ref OrderItem orderItem)
        {
            switch (values[payment_type])
            {
                case Amazon_fees:
                    paymentType = new AmazonFees();
                    break;
                case Product_charges:
                    paymentType = new ProductCharges();
                    break;
                case Other:
                    paymentType = new Other();
                    break;
                case Shipping_Service_Charges:
                    paymentType = new ShippingServiceCharges();
                    break;
                case Promo_rebates:
                    paymentType = new PromoRebates();
                    break;
                case Transaction_Details:
                    paymentType = new TransactionDetails();
                    break;
            }
            paymentType.GetPaymentDetail(values, ref orderItem);

        }
    }
}
