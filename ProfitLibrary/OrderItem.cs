using System.Collections.Generic;
using System.IO;

namespace ProfitLibrary
{
    public class OrderItem
    {
        private static int assigned = 0;
        private static int sku = 1;
        private static int item_name = 2;
        private static int order_id = 3;
        private static int quantity_sold = 4;
        private static int date_sold = 5;
        private static int bought_from = 6;
        private static int item_cost = 7;
        private static int sold_for = 8;
        private static int shipping_cost = 9;
        private static int selling_fees = 10;
        private static int profit = 11;
        private static int sales_tax = 12;
        private static int reviewed = 13;
        public bool Assigned { get; set; }
        public string SKU { get; set; }
        public string OrderID { get; set; }
        public bool Reviewed { get; set; }
        public string ItemName { get; set; }
        public int QuantitySold { get; set; }
        public string DateSold { get; set; }
        public long SoldFor { get; set; }
        public string BoughtFrom { get; set; }
        public long ItemCost { get; set; }
        public long ShippingCost { get; set; }
        public long SellingFees { get; set; }      
        public long Profit { get; set; }
        public long SalesTax { get; set; }

        internal static List<OrderItem> GetOrderItemList(string file)
        {
            var deliminator = ";";
            var orderItemList = new List<OrderItem>();
            if (string.IsNullOrWhiteSpace(file))
            {
                return orderItemList;
            }
            using (var reader = new StreamReader(file))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                var line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    //var newItem = true;
                    line = reader.ReadLine();
                    var values = line.Split(deliminator.ToCharArray());
                    try
                    {
                        var orderItem = new OrderItem
                        {
                            Assigned = bool.TryParse(values[assigned], out bool assign) ? assign : false,
                            SKU = values[sku],
                            ItemName = values[item_name],
                            OrderID = values[order_id],
                            QuantitySold = int.Parse(values[quantity_sold]),
                            DateSold = values[date_sold],
                            BoughtFrom = values[bought_from],
                            ItemCost = long.TryParse(values[item_cost], out long itemcost) ? itemcost : 0,
                            SoldFor = long.TryParse(values[sold_for], out long soldfor) ? soldfor : 0,
                            ShippingCost = long.TryParse(values[shipping_cost], out long shippingcost) ? shippingcost : 0,
                            SellingFees = long.TryParse(values[selling_fees], out long sellingfees) ? sellingfees : 0,
                            Profit = long.TryParse(values[profit], out long lprofit) ? lprofit : 0,
                            SalesTax = long.TryParse(values[sales_tax], out long lsalestax) ? lsalestax : 0,
                            Reviewed = bool.TryParse(values[assigned], out bool review) ? review : false,
                        };

                        orderItemList.Add(orderItem);
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            return orderItemList;
        }
    }
}
