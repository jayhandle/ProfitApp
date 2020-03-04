using System;
using System.Collections.Generic;
using System.IO;

namespace ProfitLibrary
{
    public class PayPalReportUpload
    {
        private static int date = 0;
        private static int order_id=12;
        private static int sku=16;
        private static int product_title=15;
        private static int sold_for = 9;
        private static int bought_from = 26;

        public static List<OrderItem> GetReport(string file)
        {
            string[] separater = { @""",""" };
            var orderItems = new List<OrderItem>();
            if (string.IsNullOrWhiteSpace(file))
            {
                return orderItems;
            }

            using (var reader = new StreamReader(file))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                var line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var newItem = true;
                    line = reader.ReadLine();
                    var values = line.Split(separater, StringSplitOptions.None);
                    OrderItem orderItem = null;

                    orderItem = new OrderItem();
                    orderItem.SKU = values[sku];
                    orderItem.OrderID = values[order_id];
                    orderItem.DateSold = values[date];
                    orderItem.SoldFor = PaymentDetail.ConvertDollarstoPennies(values[sold_for]);
                    orderItem.BoughtFrom = values[bought_from].Contains("EBAY") ? "Ebay" : string.Empty;
                    orderItem.ItemName = values[product_title];
                    orderItems.Add(orderItem);
                    
                }
            }
            return orderItems;
        }
    }
}