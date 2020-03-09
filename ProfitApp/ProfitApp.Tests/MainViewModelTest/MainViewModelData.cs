using System;
using System.Collections.Generic;
using System.Text;

namespace ProfitLibrary.Test
{
    public class MainViewModelData
    {
        public struct GetReportData
        {
            public string ItemListLocation;
            public string AmazonReportLocation;
            public int OrderItemsCount;
            public int ItemListCount;
            public string Title;
        }

        public static GetReportData[] GetReportDatas =
        {//nothing
            new GetReportData
            {
                Title = "nothing"

            },
            //Item
            new GetReportData
            {
                Title = "Item Only",
                ItemListLocation =@"C:\Users\juchendu\source\repos\ProfitApp\ProfitApp.Tests\Data\items.txt",
                ItemListCount= 12
            },          
            //AmazonReport
            new GetReportData
            {
                Title = "Amazon Only",
                AmazonReportLocation = @"C:\Users\juchendu\source\repos\ProfitApp\ProfitApp.Tests\Data\Test.txt",
                OrderItemsCount= 20
            },
            //AmazonReport and Item
            new GetReportData
            {
                Title = "Item and Amazon",
                ItemListLocation =@"C:\Users\juchendu\source\repos\ProfitApp\ProfitApp.Tests\Data\items.txt",
                AmazonReportLocation = @"C:\Users\juchendu\source\repos\ProfitApp\ProfitApp.Tests\Data\Test.txt",
                OrderItemsCount= 20,
                ItemListCount= 12
            },
        };

        public struct ItemAndOrderData
        {
            public string Title;
            public List<Item> ItemList;
            public List<OrderItem> OrderItemList;
            public int ItemListCount;
            public int Assigned;
        }

        public static ItemAndOrderData[] ItemAndOrderDatas =
        {
            //nothing
            new ItemAndOrderData
            {
                Title = "nothing"
            },
            //Item
            new ItemAndOrderData
            {
                Title = "Item only and no amazon sku exist",
                ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1"
                    }
                },
                ItemListCount = 1,
            },
            new ItemAndOrderData
            {
                Title = "Item only and 1 amazon sku exist",
                ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1",
                        AmazonSKU = "A1"
                    }
                },
                ItemListCount = 1,
            },
            new ItemAndOrderData
            {
                Title = "Item only and 1 amazon sku exist and 1 does not",
                ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1",
                        AmazonSKU = "A1"
                    },
                    new Item
                    {
                        SKU = "1",
                        Name = "item2",
                    }
                },
                ItemListCount = 2,
            },
            //Order
            new ItemAndOrderData
            {
                Title = "Order only with 1 amazon order",
                OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                },
                ItemListCount = 1,
            },
            new ItemAndOrderData
            {
                Title = "Order only with 2 amazon orders",
                OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                    new OrderItem
                    {
                        SKU ="A2",
                        ItemName = "item2"
                    },
                },
                ItemListCount = 2,
            },
            //Both
            new ItemAndOrderData
            {
                 Title = "Item with no amazon sku exist and Order with 1 amazon order",
                 ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1"
                    }
                },
                 OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                },
                 ItemListCount = 2,
            },
            new ItemAndOrderData
            {
                 Title = "Item with no amazon sku exist and Order with 2 amazon orders",
                 ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1"
                    }
                },
                 OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                    new OrderItem
                    {
                        SKU ="A2",
                        ItemName = "item2"
                    },
                },
                 ItemListCount = 3,
            },
            new ItemAndOrderData
            {
                 Title = "Item with 1 amazon sku exist and Order with 1 amazon order",
                 ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1",
                        AmazonSKU = "A1"
                    }
                },
                 OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                },
                 Assigned = 1,
                 ItemListCount = 1,
            },
            new ItemAndOrderData
            {
                 Title = "Item with 1 amazon sku exist and Order with 2 amazon orders",
                 ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1",
                        AmazonSKU = "A1"
                    }
                },
                 OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                    new OrderItem
                    {
                        SKU ="A2",
                        ItemName = "item2"
                    },
                },
                 Assigned = 1,
                 ItemListCount = 2,
            },
            new ItemAndOrderData
            {
                 Title = "Item with 1 amazon sku exist and 1 does not and Order with 1 amazon order",
                 ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1",
                        AmazonSKU = "A1"
                    },
                    new Item
                    {
                        SKU = "1",
                        Name = "item2",
                    }
                },
                 OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                },
                 Assigned=1,
                 ItemListCount = 2,
            },
            new ItemAndOrderData
            {
                 Title = "Item with 1 amazon sku exist and 1 does not and Order with 2 amazon orders",
                 ItemList = new List<Item>
                {
                    new Item
                    {
                        SKU = "0",
                        Name = "item1",
                        AmazonSKU = "A1"
                    },
                    new Item
                    {
                        SKU = "1",
                        Name = "item2",
                    }
                },
                 OrderItemList = new List<OrderItem>
                {
                    new OrderItem
                    {
                        SKU ="A1",
                        ItemName = "item1"
                    },
                    new OrderItem
                    {
                        SKU ="A2",
                        ItemName = "item2"
                    },
                },
                 Assigned=1,
                 ItemListCount = 3,
            },
        };
    }
}
