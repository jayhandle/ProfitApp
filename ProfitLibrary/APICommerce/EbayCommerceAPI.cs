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
                if(transaction.Item == null)
                {
                    continue;
                }

                var item = new Item();
                item.EbaySKU = transaction.Item?.ItemID;
                item.ItemCost = PaymentDetail.ConvertDollarstoPennies(transaction.Item?.SellingStatus.CurrentPrice.value);
                item.Name = transaction.Item?.Title;

                var order = new OrderItem();
                order.BoughtFrom = "Ebay";
                order.DateSold = transaction.CreatedDate.ToString("MM/dd/yyyy");
                order.ItemName = transaction.Item?.Title;
                order.OrderID = transaction.ExtendedOrderID;
                order.QuantitySold = transaction.QuantityPurchased;
                order.SalesTax = PaymentDetail.ConvertDollarstoPennies(transaction.Taxes.TotalTaxAmount.value);
                var extTransaction = PaymentDetail.ConvertDollarstoPennies(transaction.ExternalTransaction?.FeeOrCreditAmount.value);
                if (extTransaction <= 0)
                {
                    extTransaction = PaymentDetail.ConvertDollarstoPennies(transaction.MonetaryDetails?.Payments[0].FeeOrCreditAmount.value);
                }

                order.SellingFees = PaymentDetail.ConvertDollarstoPennies(transaction.FinalValueFee.value) + extTransaction;
                order.SoldFor = PaymentDetail.ConvertDollarstoPennies(transaction.AmountPaid.value);
                order.SKU = string.IsNullOrWhiteSpace(item.SKU) ? item.EbaySKU : item.SKU;
                var t = new Transaction
                {
                    Item = item,
                    Order = order,
                };

                profitTransactions.Add(t);
            }
            return profitTransactions;
        }

        internal List<OrderItem> GetTransactions(List<OrderItem> orderItems)
        {
            var orderIDs = new List<string>();
            foreach(var orderitem in orderItems)
            {
                if(string.IsNullOrEmpty(orderitem.SKU))
                {
                    continue;
                }
                var orderID = $"{orderitem.SKU}-{(string.IsNullOrEmpty(orderitem.TransID)?"0":orderitem.TransID)}";
                orderIDs.Add(orderID);
            }
            var orders = ebayAPI.GetOrders(orderIDs);
            var tempOrderItems = new List<OrderItem>();
            foreach(var order in orders)
            {
                var o = orderItems.Find(x => x.SKU == order.Item.ItemID);
                o.SellingFees = -PaymentDetail.ConvertDollarstoPennies(order.FinalValueFee.value);
                tempOrderItems.Add(o);
            }
            return tempOrderItems;
        }
        internal Uri GetEbayAuthSource()
        {
            ebayAPI.GetSessionID();
            return new Uri(ebayAPI.FetchToken());
        }

        internal bool GetAccessToken(Uri uri)
        {
            var token = ebayAPI.GetAccessToken(uri.AbsoluteUri);     
            return string.IsNullOrEmpty(token?.AccessToken);
        }

        internal bool HasAccessToken()
        {
            return ebayAPI.HasToken();
        }
    }
}