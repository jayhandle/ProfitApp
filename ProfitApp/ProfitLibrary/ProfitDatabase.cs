using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProfitLibrary
{
    public class ProfitDatabase : IProfitDatabase
    {
        public IDatabase Database = new SQLiteDatabase();

        public ProfitDatabase(string dbLocation)
        {
           var result = Database.OpenDatabase(dbLocation);
        }
        public IList<Item> GetItemList()
        {
            var result = Database.Select("Items");
            return result.Result as List<Item>;
        }

        public IList<OrderItem> GetOrderItems()
        {
            var result = Database.Select("Orders");
            return result.Result as List<OrderItem>;

        }

        public void CreateDatabase(string profitFile, string itemFile)
        {
            var OrderItems = new List<OrderItem>(OrderItem.GetOrderItemList(profitFile));
            var ItemList = new List<Item>(Item.GetItemList(itemFile));
            var itemColumn = new List<string> {"sku","name","amazon_sku","ebay_sku","item_cost", "quantity_bought","quantity_sold","money_back"};
            var profitColumn = new List<string> {"assigned", "sku", "item_name","order_id", "date_sold", "bought_from", "item_cost", "sales_tax","sold_for", "shipping_cost", "selling_fee", "profit"};
            var result = Database.Insert("Items", itemColumn, ItemList);
            result = Database.Insert("Items", profitColumn, OrderItems);
        }
    }
}