using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using ProfitLibrary;
using RestSharp.Extensions;

namespace ProfitLibrary.Test
{
    public static class Mocks
    {
        public static Mock<IProfitDatabase> mockProfitDB;

        public static void SetupMocks()
        {
            SetupProfitDB();
        }

        public static List<Item> ItemTable;
        public static List<OrderItem> OrderItemsTable;
        private static void SetupProfitDB()
        {
            ItemTable = new List<Item>();
            OrderItemsTable = new List<OrderItem>(); 
            mockProfitDB = new Mock<IProfitDatabase>();
            mockProfitDB.Setup(m=>m.Update(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string table, int row, string column, string value)=>
            {
                var updated = 0;
                if(table == "Order")
                {
                    switch(column)
                    {
                        case "tem":
                            updated++;
                            break;
                    }
                }
                else
                {
                    if (ItemTable.Count > row)
                    {
                        switch (column)
                        {
                            case "SKU":
                                ItemTable[row].SKU = value;
                                updated++;
                                break;
                            case "Name":
                                ItemTable[row].Name = value;
                                updated++;
                                break;
                            case "Amazon SKU":
                                ItemTable[row].AmazonSKU = value;
                                updated++;
                                break;
                            case "Item Cost":
                                ItemTable[row].ItemCost = PaymentDetail.ConvertDollarstoPennies(value);
                                updated++;
                                break;
                            case "Ebay SKU":
                                ItemTable[row].EbaySKU = value;
                                updated++;
                                break;
                            case "Quantity Bought":
                                ItemTable[row].QuantityBought = int.Parse(value);
                                updated++;
                                break;
                            case "Quantity Sold":
                                ItemTable[row].QuantityBought = int.Parse(value);
                                updated++;
                                break;
                        }
                    }
                }
                return updated;
            });
            mockProfitDB.Setup(m => m.InsertItem(It.IsAny<Item>())).Returns((Item item)=> 
            {
                var response = string.Empty;
                if(!ItemTable.Contains(item))
                {
                    ItemTable.Add(item);
                    response = "SUCCESS";
                }
                
                return response;
            });
        }
    }
}
