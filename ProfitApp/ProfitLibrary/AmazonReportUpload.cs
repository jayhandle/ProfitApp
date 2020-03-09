using System.Collections.Generic;
using System.IO;

namespace ProfitLibrary
{
    public class AmazonReportUpload
    {
        private const int date = 0;
        private const int order_id = 1;
        private const int sku = 2;
        private const int transaction_type = 3;
        private const int payment_type = 4;
        private const int payment_detail = 5;
        private const int amount = 6;
        private const int quantity = 7;
        private const int product_title = 8;
        
        public static List<OrderItem> GetReport(string file)
        {
            string tab = "\t";
            var orderItems = new List<OrderItem>();
            if(string.IsNullOrWhiteSpace(file))
            {
                return orderItems;
            }

            using (var reader = new StreamReader(file))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                var line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var newItem = true;
                    line = reader.ReadLine();
                    var values = line.Split(tab.ToCharArray());
                    OrderItem orderItem = null;
                    if(string.IsNullOrWhiteSpace(values[date]))
                    {
                        continue;
                    }

                    if (orderItems.Find(x => x.OrderID == values[order_id]) != null)
                    {
                        newItem = false;
                        orderItem = orderItems.Find(x => x.OrderID == values[order_id]);
                    }
                    else
                    {
                        orderItem = new OrderItem
                        {
                            SKU = values[sku],
                            OrderID = values[order_id],
                            DateSold = values[date],
                            BoughtFrom = "Amazon",
                            ItemName = values[product_title],
                        };
                    }
                    TransactionType transactionType = new TransactionType();
                    //switch (values[transaction_type])
                    //{
                    //    case "Order Payment":
                    //        transactionType = new OrderPayment();
                    //        break;
                    //    case "Shipping services purchased through Amazon":
                    //        transactionType = new ShippingServicesPurchasedThroughAmazon();
                    //        break;
                    //    case "Service Fees":
                    //        transactionType = new ServiceFees();
                    //        break;
                    //    case "Refund":
                    //        transactionType = new Refund();
                    //        break;
                    //}
                    transactionType?.GetPaymentType(values, ref orderItem);
                    if (newItem)
                    {
                        orderItems.Add(orderItem);
                    }
                }
            }
            return orderItems;
        }

       
    }
}
