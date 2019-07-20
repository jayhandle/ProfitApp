using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitLibrary
{
    public class Item
    {
        private static int sku = 0;
        private static int name = 1;
        private static int amazon_sku = 2;
        private static int ebay_sku = 3;
        private static long item_cost = 4;
        private static int quantity_bought = 5;
        private static int quantity_sold = 6;
        public string SKU { get; set; }
        public string Name { get; set; }
        public long ItemCost { get; set; }
        public string AmazonSKU { get; set; }
        public string EbaySKU { get; set; }
        public int QuantityBought { get; set; }
        public int QuantitySold { get; set; }

        public static List<Item> GetItemList(string file)
        { 
            var deliminator = ";";
            var itemList = new List<Item>();
            if (string.IsNullOrWhiteSpace(file))
            {
                return itemList;
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
                        var item = new Item
                        {
                            SKU = values[sku],
                            Name = values[name],
                            AmazonSKU = values[amazon_sku],
                            EbaySKU = values[ebay_sku],
                            ItemCost = long.Parse(values[item_cost]),
                            QuantityBought = int.Parse(values[quantity_bought]),
                            QuantitySold = int.Parse(values[quantity_sold])
                        };

                        itemList.Add(item);
                    }
                    catch
                    {
                        break;
                    }                   
                }
            }
            return itemList;
        }
    }
}
