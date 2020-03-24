using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProfitLibrary
{
    public class ProfitDatabase : IProfitDatabase
    {
        public IDatabase Database = new SQLiteDatabase();

        public ProfitDatabase(string dbLocation)
        {
           var result = Database.OpenDatabase(dbLocation);
            if(result.Result.ToString() == "FILE DOES NOT EXIT")
            {
                result = CreateDatabase(dbLocation);
            }
        }
        public IList<Item> GetItemList()
        {
            var itemColumn = new List<string> { "id", "sku", "name", "amazon_sku", "ebay_sku", "item_cost", "quantity_bought", "quantity_sold", "money_back", "profit" };

            var result = Database.Select("Items", "*", convertItemForSelect(new Item()));
            return result.Result == null? new List<Item>(): convertSelectResultToItems((List<Dictionary<string,object>>)result.Result);
        }

        private List<Item> convertSelectResultToItems(List<Dictionary<string, object>> data)
        {
            var Items = new List<Item>();
            foreach (var item in data)
            {
                var i = new Item();
                i.ID = int.Parse(item["id"].ToString());
                i.SKU = (string)item["sku"];
                i.Name = (string)item["name"];
                i.AmazonSKU = (string)(item["amazon_sku"]?? string.Empty);
                i.EbaySKU = (string)(item["ebay_sku"]?? string.Empty);
                i.ItemCost = (long)item["item_cost"];
                i.QuantityBought = int.Parse(item["quantity_bought"].ToString());
                i.QuantitySold = int.Parse(item["quantity_sold"].ToString());
                i.MoneyBack = (long)item["money_back"];
                i.Profit = (long)item["profit"];
                
                Items.Add(i);
            }
            return Items;
        }

        private Dictionary<string, object> convertItemForSelect(Item item)
        {
            return new Dictionary<string, object>
            {
                {"id", item.ID },
                {"sku", item.SKU },
                {"name", item.Name },
                {"amazon_sku", item.AmazonSKU },
                {"ebay_sku", item.EbaySKU },
                {"item_cost", item.ItemCost },
                {"quantity_bought", item.QuantityBought },
                {"quantity_sold", item.QuantitySold },
                {"money_back", item.MoneyBack },
                {"profit", item.Profit },
            };
        }

        public IList<OrderItem> GetOrderItems()
        {
            var result = Database.Select("Orders", "*", convertOrderForSelect(new OrderItem()));
            return result.Result == null? new List<OrderItem>() : convertSelectResultToOrderItems((List<Dictionary<string, object>>)result.Result);
        }

        private List<OrderItem> convertSelectResultToOrderItems(List<Dictionary<string, object>> data)
        {
            var orderItems = new List<OrderItem>();
            foreach(var orderitem in data)
            {
                var o = new OrderItem();

                o.ID = int.Parse(orderitem["id"].ToString());
                o.Assigned = orderitem["assigned"].ToString() == "1" || orderitem["assigned"].ToString().ToLower() == "true";
                o.SKU = (string)orderitem["sku"];
                o.ItemName = (string)orderitem["item_name"];
                o.OrderID = (string)orderitem["order_id"];
                o.QuantitySold = int.Parse(orderitem["quantity_sold"].ToString());
                o.DateSold = (string)orderitem["date_sold"];
                o.BoughtFrom = (string)orderitem["bought_from"];
                o.ItemCost = (long)orderitem["item_cost"];
                o.SalesTax = (long)orderitem["sale_tax"];
                o.SoldFor = (long)orderitem["sold_for"];
                o.ShippingCost = (long)orderitem["shipping_cost"];
                o.SellingFees = (long)orderitem["selling_fee"];
                o.Profit = (long)orderitem["profit"];
                
                orderItems.Add(o);
            }
            return orderItems;
        }

        private Dictionary<string, object> convertOrderForSelect(OrderItem orderItem)
        {
            return new Dictionary<string, object>
            {
                {"id", orderItem.ID },
                {"assigned", orderItem.Assigned},
                {"sku", orderItem.SKU},
                {"item_name", orderItem.ItemName },
                {"order_id", orderItem.OrderID },
                {"quantity_sold", orderItem.QuantitySold },
                {"date_sold", orderItem.DateSold },
                {"bought_from", orderItem.BoughtFrom},
                {"item_cost", orderItem.ItemCost },
                {"sale_tax", orderItem.SalesTax },
                {"sold_for", orderItem.SoldFor },
                {"shipping_cost", orderItem.ShippingCost },
                {"selling_fee", orderItem.SellingFees },
                {"profit", orderItem.Profit },
            };
        }

        public IDBResult CreateDatabase(string dbLocation)
        {
            var ItemsTable = $@"CREATE TABLE ""Items"" (
    ""id""    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	""sku""   TEXT NOT NULL UNIQUE,
    ""name""  TEXT NOT NULL,
	""amazon_sku""    TEXT,
    ""ebay_sku""  TEXT,
    ""item_cost"" INTEGER NOT NULL DEFAULT 0,
	""quantity_bought""   INTEGER NOT NULL DEFAULT 0,
	""quantity_sold"" INTEGER NOT NULL DEFAULT 0,
	""money_back""    INTEGER NOT NULL DEFAULT 0,
    ""profit""        INTEGER DEFAULT 0
)";

            var OrdersTable = $@"CREATE TABLE ""Orders"" (
    ""id""    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	""assigned""  TEXT NOT NULL DEFAULT 'FALSE',
	""sku""   TEXT NOT NULL,
    ""item_name"" TEXT NOT NULL,
	""order_id""  TEXT NOT NULL,
	""quantity_sold"" INTEGER NOT NULL DEFAULT 0,
	""date_sold"" TEXT NOT NULL,
	""bought_from""   TEXT NOT NULL,
	""item_cost"" INTEGER,
	""sale_tax""  INTEGER DEFAULT 0,
	""sold_for""  INTEGER NOT NULL DEFAULT 0,
	""shipping_cost"" INTEGER NOT NULL DEFAULT 0,
	""selling_fee""   INTEGER NOT NULL DEFAULT 0,
	""profit""    INTEGER DEFAULT 0,
    UNIQUE(order_id,bought_from)
)";
            var tables = new List<string> { ItemsTable, OrdersTable };
            return Database.CreateDatabase(dbLocation, tables);
        }

        private List<object> convertItemForDatabase(Item item, bool withID = false)
        {
            var data = new List<object>
            {
                { item.SKU },
                { item.Name ?? string.Empty},
                { item.AmazonSKU ?? string.Empty},
                { item.EbaySKU??string.Empty},
                { item.ItemCost},
                { item.QuantityBought},
                { item.QuantitySold},
                { item.MoneyBack},
                { item.Profit}
            };

            if(withID)
            {
                data.Insert(0, item.ID);
            }

            return data; 
        }

        private List<object> convertOrderForDatabase(OrderItem order, bool withID = false)
        {    
            var data = new List<object>
            {
                { order.Assigned},
                { order.SKU ?? string.Empty},
                { order.ItemName ?? string.Empty},
                { order.OrderID ?? string.Empty},
                { order.QuantitySold},
                { order.DateSold ?? string.Empty},
                { order.BoughtFrom ?? string.Empty},
                { order.ItemCost},
                { order.SalesTax},
                { order.SoldFor},
                { order.ShippingCost},
                { order.SellingFees},
                { order.Profit}
            };

            if (withID)
            {
                data.Insert(0, order.ID);
            }

            return data;
        }

        public void SaveItemList(ObservableCollection<Item> itemList)
        {
            if(itemList == null)
            {
                return;
            }
            var itemColumn = new List<string> { "sku", "name", "amazon_sku", "ebay_sku", "item_cost", "quantity_bought", "quantity_sold", "money_back", "profit" };

            foreach (var item in itemList)
            {
                if (Database.Exist("Items", "id", item.ID))
                {
                    var result = Database.Update("Items", item.ID, itemColumn, convertItemForDatabase(item));
                }
                else
                {
                    var result = Database.Insert("Items", itemColumn, convertItemForDatabase(item));
                }
            }
        }

        public void SaveOrderList(ObservableCollection<OrderItem> orderItems)
        {
            if(orderItems == null)
            {
                return;
            }
            var profitColumn = new List<string> { "assigned", "sku", "item_name", "order_id", "quantity_sold", "date_sold", "bought_from", "item_cost", "sale_tax", "sold_for", "shipping_cost", "selling_fee", "profit" };
           
            foreach (var order in orderItems)
            {
                if(Database.Exist("Orders", "id", order.ID))
                {
                  var result =  Database.Update("Orders", order.ID, profitColumn, convertOrderForDatabase(order));
                }
                else
                {
                    var result = Database.Insert("Orders", profitColumn, convertOrderForDatabase(order));
                }
            }
            
            //using (Stream writetext = stream)
            //{
            //    var line = new UTF8Encoding(true).GetBytes($"Assigned;SKU;ItemName;OrderID;QuantitySold;DateSold;BoughtFrom;ItemCost;SalesTax;SoldFor;ShippingCost;SellingFees;Profit" + Environment.NewLine);
            //    writetext.Write(line, 0, line.Length);
            //    foreach (var item in OrderItems)
            //    {
            //        line = new UTF8Encoding(true).GetBytes($"{item.Assigned};{item.SKU};{item.ItemName};{item.OrderID};{item.QuantitySold};{item.DateSold};{item.BoughtFrom};{item.ItemCost};{item.SalesTax};{item.SoldFor};{item.ShippingCost};{item.SellingFees};{item.Profit}" + Environment.NewLine);
            //        writetext.Write(line, 0, line.Length);
            //    }
            //}
        }

        public int Update(string Table, int row, string column, string value)
        {
            switch(column)
            {
                case "SKU":
                    column = "sku";
                    break;
                case "Order ID":
                    column = "order_id";
                    break;
                case "Selling Fees":
                    column = "selling_fee";
                    value = PaymentDetail.ConvertDollarstoPennies(value).ToString();
                    break;
                case "Item Name":
                    column = "item_name";
                    break;
                case "Quantity Bought":
                    column = "quantity_bought";
                    break;
                case "Item Cost":
                    column = "item_cost";
                    value = PaymentDetail.ConvertDollarstoPennies(value).ToString();
                    break;
                case "SalesTax":
                    column = "sale_tax";
                    value = PaymentDetail.ConvertDollarstoPennies(value).ToString();
                    break;
                case "Quantity Sold":
                    column = "quantity_sold";
                    break;
                case "Date Sold":
                    column = "date_sold";
                    break;
                case "Bought From":
                    column = "bought_from";
                    break;
                case "Sold For":
                    column = "sold_for";
                    value = PaymentDetail.ConvertDollarstoPennies(value).ToString();
                    break;
                case "Shipping Cost":
                    column = "shipping_cost";
                    value = PaymentDetail.ConvertDollarstoPennies(value).ToString();
                    break;
                case "Assigned":
                case "assigned":
                    value = value.ToLower()== "true" ? "1" : "0";
                    break;
                default:
                    break;
            }
            var result = Database.Update(Table, row, new List<string> { column.ToLower() }, new List<object> { value });
            if(result?.Result.ToString() != "SUCCESS")
            {
                return -1;
            }
            else
            {
                try
                {
                    var num = int.Parse(result.Message.Split(':')[1]);
                    return num;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public string InsertItem(Item item)
        {
            var itemColumn = new List<string> { "sku", "name", "amazon_sku", "ebay_sku", "item_cost", "quantity_bought", "quantity_sold", "money_back", "profit" };
            return Database.Insert("Items", itemColumn, convertItemForDatabase(item)).Result.ToString();      
        }

        public string InsertOrder(OrderItem order)
        {
            var profitColumn = new List<string> { "assigned", "sku", "item_name", "order_id", "quantity_sold", "date_sold", "bought_from", "item_cost", "sale_tax", "sold_for", "shipping_cost", "selling_fee", "profit" };
            return Database.Insert("Orders", profitColumn, convertOrderForDatabase(order)).Result.ToString();            
        }
    }
}