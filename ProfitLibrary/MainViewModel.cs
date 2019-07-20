using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitLibrary
{
    public class MainViewModel: ViewModel
    {
        private List<OrderItem> orderItems;
        private string itemListLocation;
        private string amazonFileLocation;
        private List<Item> itemList;

        public List<Item> ItemList
        {
            get => itemList;
            set
            {
                itemList = value;            
                OnPropertyChanged();
            }
        }
        public List<OrderItem> OrderItems
        {
            get => orderItems;
            set
            {
                orderItems = value;
                OnPropertyChanged();
            }
        }

        public string AmazonFileLocation
        {
            get=> amazonFileLocation;
            set
            {
                amazonFileLocation = value;
                OnPropertyChanged();
            }
        }

        public string ItemListLocation
        {
            get => itemListLocation;
            set
            {
                itemListLocation = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ItemList = new List<Item>();
        }

        public void GetReport()
        {
            GetItemListFromFile();
            orderItems = new List<OrderItem>();
            orderItems.AddRange(ProfitLibrary.AmazonReportUpload.GetReport(AmazonFileLocation));
            orderItems.RemoveAll(i => string.IsNullOrWhiteSpace(i.SKU));
            if (ItemList != null)
            {
                orderItems.ForEach((x) =>
                {
                    foreach (var item in ItemList)
                    {
                        if (item.AmazonSKU == x.SKU)
                        {
                            x.Assigned = true;
                            item.QuantitySold = x.QuantitySold;
                            x.ItemCost = -item.ItemCost;
                            x.Profit = (x.SoldFor * x.QuantitySold) + x.ItemCost + x.SellingFees + x.ShippingCost;
                            break;
                        }
                    }
                });
            }

            OrderItems = orderItems;
        }

        public void GetItemListFromFile()
        {
            ItemList = Item.GetItemList(ItemListLocation);
        }

        public void AutoCreateItems()
        {
            itemList = ItemList ?? new List<Item>();
            var unassignedItems = OrderItems?.Where(x => !x.Assigned).ToList();
            unassignedItems?.ForEach((x) =>
            {
                var item = new Item
                {
                    SKU = itemList.Count.ToString(),
                    AmazonSKU = x.SKU,
                    Name = x.ItemName,
                };

                if (!itemList.Exists(i => i.AmazonSKU == item.AmazonSKU))
                {
                    itemList.Add(item);
                }
            });

            ItemList = itemList;
        }

        public void SaveItemListToFile(Stream stream)
        {
            using (Stream writetext = stream)
            {
                var line = new UTF8Encoding(true).GetBytes($"SKU;Name;AmazonSKU;EbaySKU;ItemCost;QuantityBought;QuantitySold" + Environment.NewLine);
                writetext.Write(line, 0, line.Length);
                foreach (var item in ItemList)
                {
                    line = new UTF8Encoding(true).GetBytes($"{item.SKU};{item.Name};{item.AmazonSKU};{item.AmazonSKU};{item.ItemCost};{item.QuantityBought};{item.QuantitySold}" + Environment.NewLine);
                    writetext.Write(line, 0, line.Length);
                }
            }
        }

        public void AutoAssign()
        {
            if (ItemList != null)
            {
                orderItems?.ForEach((x) =>
                {
                    foreach (var item in ItemList)
                    {
                        if (item.AmazonSKU == x.SKU)
                        {
                            x.Assigned = true;
                            item.QuantitySold = x.QuantitySold;
                            x.ItemCost = -item.ItemCost;
                            x.Profit = (x.SoldFor * x.QuantitySold) + x.ItemCost + x.SellingFees + x.ShippingCost;
                            break;
                        }
                    }
                });
            }

            OrderItems = orderItems;
        }

        public void DeletedItemsFromItemList(IList<Item> items)
        {
            if(items == null || itemList == null)
            {
                return;
            }

            foreach(var item in items)
            {
                itemList?.Remove(item);
            }

            ItemList = itemList;
        }

        public void EditItemListItem(Item selectedItem, string header, string value)
        {            
            switch (header)
            {
                case "SKU":
                    itemList[itemList.IndexOf(selectedItem)].SKU = value;
                    break;
                case "Name":
                    itemList[itemList.IndexOf(selectedItem)].Name = value;
                    break;
                case "Item Cost":
                    itemList[itemList.IndexOf(selectedItem)].ItemCost = PaymentDetail.ConvertDollarstoPennies(value);
                    break;
                case "Amazon SKU":
                    itemList[itemList.IndexOf(selectedItem)].AmazonSKU = value;
                    break;
                case "Ebay SKU":
                    itemList[itemList.IndexOf(selectedItem)].EbaySKU = value;
                    break;
                case "Quantity Bought":
                    itemList[itemList.IndexOf(selectedItem)].QuantityBought = int.Parse(value);
                    break;
                case "Quantity Sold":
                    itemList[itemList.IndexOf(selectedItem)].QuantitySold = int.Parse(value);
                    break;
            }

            selectedItem.SKU = string.IsNullOrWhiteSpace(selectedItem.SKU) ? itemList.Count.ToString() : selectedItem.SKU;
            ItemList = itemList;
        }
    }
}
