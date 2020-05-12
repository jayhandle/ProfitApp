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
    public class MainViewModel : ViewModel
    {
        private ObservableCollection<OrderItem> orderItems;
        private string itemListLocation;
        private string paypalFileLocation;
        private ObservableCollection<Item> itemList;
        private string profitFileLocation;
        public IProfitDatabase ProfitDB;
        private string databaseLocation;
        private DateTime lastHitDate = DateTime.Now;
        private bool ebayWebBrowserIsVisible;
        private Uri getEbayAuthSource;
        private ObservableCollection<ChartData> chartDatas;
        private ObservableCollection<string> chartInfo;
        private ObservableCollection<string> chartFrom;
        private ObservableCollection<string> chartTo;

        public ObservableCollection<string> ChartInfo
        {
            get => chartInfo; set => chartInfo = value;
        }

        public ObservableCollection<string> ChartFrom
        {
            get => chartFrom; 
            set 
            { 
                chartFrom = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ChartTo
        {
            get => chartTo;
            set 
            { 
                chartTo = value;
                OnPropertyChanged();
            }
        }

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
            get => paypalFileLocation;
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

        public string DatabaseLocation
        {
            get => databaseLocation;
            set
            {
                databaseLocation = value;
                OnPropertyChanged();
            }
        }

        public DateTime LastHitDate
        {
            get => lastHitDate;
            set
            {
                lastHitDate = value;
                OnPropertyChanged();
            }
        }

        public bool EbayWebBrowserIsVisible
        {
            get => ebayWebBrowserIsVisible;
            private set
            {
                ebayWebBrowserIsVisible = value;
                OnPropertyChanged();
            }
        }

        public Uri GetEbayAuthSource
        {
            get => getEbayAuthSource;
            set
            {
                getEbayAuthSource = value;
                OnPropertyChanged();
            }
        }
        public EbayCommerceAPI ClientAPI { get; private set; }
        public ObservableCollection<ChartData> ChartDatas
        {
            get => chartDatas;
            set
            {
                chartDatas = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ItemList = new ObservableCollection<Item>();
            var chartInfoList = new List<string> { "Total Profit by Month" };
            ChartInfo = new ObservableCollection<string>(chartInfoList);
            ChartFrom = new ObservableCollection<string>();
            ChartTo = new ObservableCollection<string>();
        }

        public void GetReport()
        {
            GetItemListFromFile();
            GetOrderItemFromProfitReportFile();
            //GetOrderItemsFromAmazonFile();
            //GetOrderItemsFromPayPalFile();
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
            OrderItems = new ObservableCollection<OrderItem>(OrderItem.GetOrderItemList(ProfitFileLocation));
            //OrderItems =new ObservableCollection<OrderItem>(ProfitDB.GetOrderItems());

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
            ItemList = new ObservableCollection<Item>(Item.GetItemList(ItemListLocation));
            //ItemList = new ObservableCollection<Item>(ProfitDB.GetItemList());
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

        public void OpenDatabase()
        {
            ProfitDB = new ProfitDatabase(DatabaseLocation);
            ItemList = new ObservableCollection<Item>(ProfitDB.GetItemList());
            OrderItems = new ObservableCollection<OrderItem>(ProfitDB.GetOrderItems());
        }

        public void GetEbayBrowserVisiblity(Uri uri)
        {
            EbayWebBrowserIsVisible = false;
            if (!ClientAPI.GetAccessToken(uri))
            {
                EbayWebBrowserIsVisible = true;
            }

        }

        public void GetEbayTransaction()
        {
            var ebayTransactions = ClientAPI.GetTransactions(LastHitDate);
            CompareEbayTransactionWithDatabaseTransaction(ebayTransactions);
        }

        private void CompareEbayTransactionWithDatabaseTransaction(List<Transaction> ebayTransactions)
        {
            if (OrderItems != null)
            {
                var newOrders = new List<OrderItem>(OrderItems.ToList());
                foreach (var ebayTransaction in ebayTransactions)
                {
                    if (!OrderItems.ToList().Exists(o => o.OrderID == ebayTransaction.Order.OrderID && o.BoughtFrom == ebayTransaction.Order.BoughtFrom))
                    {
                        newOrders.Add(ebayTransaction.Order);
                        ProfitDB.InsertOrder(ebayTransaction.Order);
                    }
                }

                OrderItems = new ObservableCollection<OrderItem>(newOrders);
            }
        }

        public GraphData ChartGraph(DateTime DateFrom, DateTime DateTo)
        {
            var xTicks = new List<string>();
            chartDatas = new ObservableCollection<ChartData>();
            while (DateFrom.Month < DateTo.Month)
            {
                xTicks.Add(DateFrom.ToString("MMM"));
                DateFrom = DateFrom.AddMonths(1);
            }
            var graphData = new GraphData
            {
                XTicks = xTicks,
                Points = new List<Point>()
            };
            long maxProfit = -10000;
            foreach (var data in xTicks)
            {
                var items = OrderItems.Where(x => DateTime.Parse(x.DateSold).ToString("MMM") == data).ToList();

                var point = new Point();
                point.X = DateTime.Parse(items[0].DateSold).Month;
                long sumProfit = 0;
                long sumGrossProfit = 0;
                foreach (var item in items)
                {
                    sumProfit += item.Profit;
                    sumGrossProfit += item.SoldFor;
                }
                if (sumProfit > maxProfit)
                {
                    maxProfit = sumProfit;
                }

                var chartData = new ChartData
                {
                    X = data,
                    Y = PaymentDetail.ConvertPenniesToDollars(sumProfit),
                    Z = PaymentDetail.ConvertPenniesToDollars(sumGrossProfit),
                };
                chartDatas.Add(chartData);
                point.Y = (double)(sumProfit / 100);

                graphData.Points.Add(point);
            }

            ChartDatas = chartDatas;
            graphData.YTicks = maxProfit <= 0 ? 1 : maxProfit;
            graphData.YTicks = Math.Round(graphData.YTicks / 100) + 1;
            return graphData;
        }

        public void ChartInfoSelectionChanged(string selectedChartInfo)
        {
            switch (selectedChartInfo)
            {
                case "Total Profit by Month":
                    var fromList = new List<string>();
                    var toList = new List<string>();

                    for (int i = 0; i < 12; i++)
                    {
                        var date = new DateTime();
                        date = date.AddMonths(i);
                        fromList.Add(date.ToString("MMM"));
                        toList.Add(date.ToString("MMM"));
                    }

                    ChartFrom = new ObservableCollection<string>(fromList);
                    ChartTo = new ObservableCollection<string>(toList);
                    break;
            }
        }

        public bool HasEbayToken()
        {
            return ClientAPI.HasAccessToken();
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

                    x.Profit = x.SoldFor + x.ItemCost + x.SellingFees + x.ShippingCost;
                    ProfitDB.Update("Orders", x.ID, "profit", x.Profit.ToString());
                    x.Assigned = true;
                    ProfitDB.Update("Orders", x.ID, "assigned", x.Assigned.ToString());
                });

                itemList.ToList().ForEach((x) =>
                {
                    x.QuantitySold = 0;
                    x.Profit = 0;
                    long totalProfit = 0;
                    foreach (var orderItem in orderItems)
                    {
                        if (orderItem.SKU == x.SKU || orderItem.SKU == x.AmazonSKU || orderItem.SKU == x.EbaySKU)
                        {
                            x.QuantitySold += orderItem.QuantitySold;
                            totalProfit += orderItem.Profit;
                        }
                    }

                    x.MoneyBack = (x.QuantitySold * x.ItemCost) + totalProfit;

                    EnsureItemCalculations(ref x);
                    ProfitDB.Update("Items", x.ID, "quantity_sold", x.QuantitySold.ToString());
                    ProfitDB.Update("Items", x.ID, "money_back", x.MoneyBack.ToString());
                    ProfitDB.Update("Items", x.ID, "profit", x.Profit.ToString());
                });
            }

            OrderItems = orderItems;


        }

        public Uri GetAuthSource()
        {
            ClientAPI = new EbayCommerceAPI();
            return ClientAPI.GetEbayAuthSource();
            //var transactions = ClientAPI.GetTransactions(LastHitDate);
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
                case "Quantity Sold":
                    var itemCost = orderItems[orderItems.IndexOf(selectedOrderItem)].ItemCost;
                    if(int.TryParse(value, out int soldNum ))
                    {
                        itemCost *= soldNum;
                    }

                    orderItems[orderItems.IndexOf(selectedOrderItem)].ItemCost = itemCost;
                    break;
                case "Item Cost":
                    orderItems[orderItems.IndexOf(selectedOrderItem)].ItemCost = PaymentDetail.ConvertDollarstoPennies(value);
                    break;
                case "Bought From":
                case "Sold For":
                    if (orderItems[orderItems.IndexOf(selectedOrderItem)].BoughtFrom == "Ebay")
                    {
                        var soldfor = orderItems[orderItems.IndexOf(selectedOrderItem)].SoldFor;
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            value = GetEbayFee(soldfor.ToString());
                        }
                        orderItems[orderItems.IndexOf(selectedOrderItem)].SellingFees = -PaymentDetail.ConvertDollarstoPennies(value);
                    }
                    break;
                case "Selling Fees":
                    orderItems[orderItems.IndexOf(selectedOrderItem)].SellingFees = -PaymentDetail.ConvertDollarstoPennies(value);
                    break;
                default:
                    break;
            }

            OrderItems = orderItems;
            var result = ProfitDB.Update("Orders", selectedOrderItem.ID, header, value);
            if (result == 0)
            {
                var response = ProfitDB.InsertOrder(selectedOrderItem);
                if (response == "SUCCESS")
                {
                    selectedOrderItem.ID = OrderItems.Count();
                }
            }
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

        public void SaveOrderItemListToFile()
        {
            ProfitDB = new ProfitDatabase(DatabaseLocation);
            ProfitDB.SaveItemList(ItemList);
            ProfitDB.SaveOrderList(OrderItems);
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

        public void CreateItemReport()
        {
            orderItems = orderItems ?? new ObservableCollection<OrderItem>();
            orderItems.Add(new OrderItem());
            OrderItems = orderItems;
        }

        public void DeletedItemsFromItemList(IList<Item> items)
        {
            if (items == null || itemList == null)
            {
                return;
            }

            foreach (var item in items)
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
                default:
                    break;
            }

            selectedItem.SKU = string.IsNullOrWhiteSpace(selectedItem.SKU) ? itemList.Count.ToString() : selectedItem.SKU;
            EnsureItemCalculations(ref selectedItem);
            itemList[itemList.IndexOf(selectedItem)] = selectedItem;
            ItemList = itemList;
            var result = ProfitDB.Update("Items", selectedItem.ID, header, value);
            if (result == 0)
            {
                var response = ProfitDB.InsertItem(selectedItem);
                if (response == "SUCCESS")
                {
                    selectedItem.ID = ItemList.Count();
                }
            }

        }
    }
}
