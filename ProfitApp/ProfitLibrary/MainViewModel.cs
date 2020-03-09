using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitLibrary
{
    public class MainViewModel: ViewModel
    {
        private ObservableCollection<OrderItem> orderItems;
        private string itemListLocation;
        private string paypalFileLocation;
        private ObservableCollection<Item> itemList;
        private string profitFileLocation;
        public IProfitDatabase ProfitDB;
        public ObservableCollection<Item> ItemList
        {
            get => itemList;
            set
            {
                itemList = value;            
                OnPropertyChanged();
            }
        }
        public ObservableCollection<OrderItem> OrderItems
        {
            get => orderItems;
            set
            {
                orderItems = value;
                OnPropertyChanged();
            }
        }
        
        public string ProfitFileLocation
        {
            get => profitFileLocation;
            set
            {
                profitFileLocation = value;
                OnPropertyChanged();
            }
        }
        public string PayPalFileLocation
        {
            get=> paypalFileLocation;
            set
            {
                paypalFileLocation = value;
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
            ItemList = new ObservableCollection<Item>();
            
           
        }

        public void GetReport()
        {
            GetItemListFromFile();
            GetOrderItemFromProfitReportFile();
            //GetOrderItemsFromAmazonFile();
            GetOrderItemsFromPayPalFile();
        }

        private void GetOrderItemsFromPayPalFile()
        {
            var paypalOrderItem = new List<OrderItem>();
            //AddOrderItemList(ProfitLibrary.PayPalReportUpload.GetReport(PayPalFileLocation));
            paypalOrderItem.AddRange(ProfitLibrary.PayPalReportUpload.GetReport(PayPalFileLocation));
            paypalOrderItem.RemoveAll(i => string.IsNullOrWhiteSpace(i.SKU));
            if (ItemList != null)
            {
                paypalOrderItem.ForEach((x) =>
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

        private void GetOrderItemFromProfitReportFile()
        {
            //OrderItems = new ObservableCollection<OrderItem>(OrderItem.GetOrderItemList(ProfitFileLocation));
            OrderItems =new ObservableCollection<OrderItem>(ProfitDB.GetOrderItems());

        }
      

        //private void GetOrderItemsFromAmazonFile()
        //{
        //    orderItems = orderItems ?? new ObservableCollection<OrderItem>();
        //    AddOrderItemList(ProfitLibrary.AmazonReportUpload.GetReport(AmazonFileLocation));
        //    orderItems.AddRange(ProfitLibrary.AmazonReportUpload.GetReport(AmazonFileLocation));
        //    orderItems.RemoveAll(i => string.IsNullOrWhiteSpace(i.SKU));
        //    if (ItemList != null)
        //    {
        //        orderItems.ForEach((x) =>
        //        {
        //            foreach (var item in ItemList)
        //            {
        //                if (item.AmazonSKU == x.SKU)
        //                {
        //                    x.Assigned = true;
        //                    item.QuantitySold = x.QuantitySold;
        //                    x.ItemCost = -item.ItemCost;
        //                    x.Profit = (x.SoldFor * x.QuantitySold) + x.ItemCost + x.SellingFees + x.ShippingCost;
        //                    break;
        //                }
        //            }
        //        });
        //    }

        //    OrderItems = orderItems;
        //}

        public void GetItemListFromFile()
        {
            //itemList = new ObservableCollection<Item>(Item.GetItemList(ItemListLocation));
            ItemList = new ObservableCollection<Item>(ProfitDB.GetItemList());
        }

        public void AutoCreateItems()
        {
            itemList = ItemList ?? new ObservableCollection<Item>();
            var unassignedItems = OrderItems?.Where(x => !x.Assigned).ToList();
            unassignedItems?.ForEach((x) =>
            {
                var item = new Item
                {
                    SKU = itemList.Count.ToString(),
                    AmazonSKU = x.SKU,
                    Name = x.ItemName,
                };

                if (!itemList.ToList().Exists(i => i.AmazonSKU == item.AmazonSKU))
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
                var line = new UTF8Encoding(true).GetBytes($"SKU;Name;AmazonSKU;EbaySKU;ItemCost;QuantityBought;QuantitySold;MoneyBack;Profit" + Environment.NewLine);
                writetext.Write(line, 0, line.Length);
                foreach (var item in ItemList)
                {
                    line = new UTF8Encoding(true).GetBytes($"{item.SKU};{item.Name};{item.AmazonSKU};{item.AmazonSKU};{item.ItemCost};{item.QuantityBought};{item.QuantitySold};{item.MoneyBack};{item.Profit}" + Environment.NewLine);
                    writetext.Write(line, 0, line.Length);
                }
            }
        }

        public void Import()
        {
            ProfitDB = new ProfitDatabase(string.Empty);
            ProfitDB.CreateDatabase(ProfitFileLocation, ItemListLocation);
        }

        public void AutoAssign()
        {
            if (ItemList != null)
            {
                orderItems?.ToList().ForEach((x) =>
                {
                    if (!x.Assigned)
                    {
                        foreach (var item in ItemList)
                        {
                            if (x.SKU == item.SKU || x.SKU == item.AmazonSKU || x.SKU == item.EbaySKU)
                            {
                                x.ItemCost = -item.ItemCost;
                                x.ItemName = item.Name;
                                break;
                            }
                        }
                        //else if (item.SKU == x.SKU)
                        //{
                        //    x.Assigned = true;
                        //    x.QuantitySold = item.QuantitySold;
                        //    x.ItemCost = -item.ItemCost;
                        //    x.ItemName = item.Name;
                        //    x.Profit = (x.SoldFor * x.QuantitySold) - x.ItemCost - x.SellingFees - x.ShippingCost;
                        //    break;
                        //}
                    }

                    x.Profit = (x.SoldFor * x.QuantitySold) + x.ItemCost + x.SellingFees + x.ShippingCost;

                    x.Assigned = true;
                });

                itemList.ToList().ForEach((x) => 
                {
                    x.QuantitySold = 0;
                    x.Profit = 0;
                    long totalProfit = 0;
                    foreach(var orderItem in orderItems)
                    {
                        if(orderItem.SKU == x.SKU || orderItem.SKU == x.AmazonSKU || orderItem.SKU == x.EbaySKU)
                        {
                            x.QuantitySold += orderItem.QuantitySold;
                            totalProfit += orderItem.Profit;
                        }
                    }

                    x.MoneyBack = (x.QuantitySold * x.ItemCost) + totalProfit;

                    EnsureItemCalculations(ref x);                    
                });
            }

            OrderItems = orderItems;

            
        }

        private void EnsureItemCalculations(ref Item item)
        {
            item.TotalCost = item.ItemCost * item.QuantityBought;
            item.Profit = item.MoneyBack - item.TotalCost;
        }

        public void EditOrderListItem(OrderItem selectedOrderItem, string header, string value)
        {
            switch (header)
            {
                case "SKU":
                    orderItems[orderItems.IndexOf(selectedOrderItem)].SKU = value;
                    var item = ItemList.FirstOrDefault(x => x.AmazonSKU == value || x.EbaySKU == value || x.SKU == value);
                    if (item != null)
                    {
                        orderItems[orderItems.IndexOf(selectedOrderItem)].ItemName = item.Name;
                        orderItems[orderItems.IndexOf(selectedOrderItem)].ItemCost = -item.ItemCost;
                    }
                    break;
                case "Item Cost":
                    orderItems[orderItems.IndexOf(selectedOrderItem)].ItemCost = PaymentDetail.ConvertDollarstoPennies(value);
                    break;
                case "Bought From":
                case "Sold For":
                    if (orderItems[orderItems.IndexOf(selectedOrderItem)].BoughtFrom == "Ebay")
                    {
                        var soldfor = orderItems[orderItems.IndexOf(selectedOrderItem)].SoldFor;
                        value = GetEbayFee(soldfor.ToString());
                        orderItems[orderItems.IndexOf(selectedOrderItem)].SellingFees = -PaymentDetail.ConvertDollarstoPennies(value);
                    }
                    break;
            }

            OrderItems = orderItems;
        }

        private string GetEbayFee(string soldfor)
        {
            decimal value = long.Parse(soldfor) / 100m;
            value = Math.Round(value, 2);
            var ebay = (value * .1m);
            ebay = Math.Round(ebay, 2);
            var pp = (value * .029m) + .30m;
            pp = Math.Round(pp, 2);
            return Math.Round(pp + ebay, 2).ToString();
        }

        public void SaveOrderItemListToFile(Stream stream)
        {
            using (Stream writetext = stream)
            {
                var line = new UTF8Encoding(true).GetBytes($"Assigned;SKU;ItemName;OrderID;QuantitySold;DateSold;BoughtFrom;ItemCost;SalesTax;SoldFor;ShippingCost;SellingFees;Profit" + Environment.NewLine);
                writetext.Write(line, 0, line.Length);
                foreach (var item in OrderItems)
                {
                    line = new UTF8Encoding(true).GetBytes($"{item.Assigned};{item.SKU};{item.ItemName};{item.OrderID};{item.QuantitySold};{item.DateSold};{item.BoughtFrom};{item.ItemCost};{item.SalesTax};{item.SoldFor};{item.ShippingCost};{item.SellingFees};{item.Profit}" + Environment.NewLine);
                    writetext.Write(line, 0, line.Length);
                }
            }
        }

        public void CreateItemReport()
        {
            orderItems = orderItems ?? new ObservableCollection<OrderItem>();
            orderItems.Add(new OrderItem());
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
            EnsureItemCalculations(ref selectedItem);
            itemList[itemList.IndexOf(selectedItem)] = selectedItem;
            ItemList = itemList;
        }
    }
}
