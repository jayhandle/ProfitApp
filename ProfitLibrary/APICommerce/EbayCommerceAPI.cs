using EbayAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProfitLibrary
{
    public class EbayCommerceAPI : IAPICommerces
    {
        private  static string configlocation = @"C:\Users\sonov\Dropbox\samples\ProfitApp\ProfitApp\ProfitLibrary\ebay-config.yaml";
        private Context ebayAPI;

        public EbayCommerceAPI()
        {
            var configByte = File.ReadAllBytes(configlocation);
            ebayAPI = new EbayAPI.Context(configByte);
           
        }

        internal List<Transaction> GetTransactions(DateTime lastHitDate)
        {
            var transactions = ebayAPI.GetSellerTransactions(lastHitDate);
            var profitTransactions = new List<Transaction>();
            foreach(var transaction in transactions)
            {
                var item = new Item();
                item.EbaySKU = transaction.Item?.ItemID;
                item.ItemCost = PaymentDetail.ConvertDollarstoPennies(transaction.Item.SellingStatus.CurrentPrice.value);
                item.Name = transaction.Item.Title;

                var order = new OrderItem();
                order.BoughtFrom = "Ebay";
                order.DateSold = transaction.CreatedDate.ToString("MM/dd/yyyy");
                order.ItemName = transaction.Item.Title;
                order.OrderID = transaction.ExtendedOrderID;
                order.QuantitySold = transaction.QuantityPurchased;
                order.SalesTax = PaymentDetail.ConvertDollarstoPennies(transaction.Taxes.TotalTaxAmount.value);
                order.SellingFees = PaymentDetail.ConvertDollarstoPennies(transaction.FinalValueFee.value) + PaymentDetail.ConvertDollarstoPennies(transaction.ExternalTransaction?.FeeOrCreditAmount.value);
                order.SoldFor = PaymentDetail.ConvertDollarstoPennies(transaction.AmountPaid.value);

                var t = new Transaction
                {
                    Item = item,
                    Order = order,
                };

                profitTransactions.Add(t);
            }
            return profitTransactions;
        }

        internal Uri GetEbayAuthSource()
        {
            return new Uri(ebayAPI.GetSessionID());
        }

        internal bool GetAccessToken(Uri uri)
        {
            return ebayAPI.GetAccessToken(uri.AbsoluteUri);
        }

        internal bool HasAccessToken()
        {
            return ebayAPI.HasToken();
        }
    }
}