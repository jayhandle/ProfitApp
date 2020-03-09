using NUnit.Framework;
using System.Collections.Generic;

namespace ProfitLibrary.Test
{
    public class MainViewModelTests
    {
        private MainViewModel vm;
        [SetUp]
        public void Setup()
        {
            vm = new MainViewModel();
        }

        [TestCaseSource(typeof(MainViewModelData), "GetReportDatas")]
        public void GetReport(MainViewModelData.GetReportData data)
        {            
            vm.ItemListLocation = data.ItemListLocation;
            vm.PayPalFileLocation = data.AmazonReportLocation;
            vm.GetReport();
            Assert.AreEqual(data.OrderItemsCount, vm.OrderItems.Count, data.Title);
            Assert.AreEqual(data.ItemListCount, vm.ItemList.Count, data.Title);
        }

        [TestCaseSource(typeof(MainViewModelData), "GetReportDatas")]
        public void GetItemListFromFile(MainViewModelData.GetReportData data)
        {
            vm.ItemListLocation = data.ItemListLocation;
            vm.GetItemListFromFile();
            Assert.AreEqual(data.ItemListCount, vm.ItemList.Count, data.Title);
        }

        [TestCaseSource(typeof(MainViewModelData), "ItemAndOrderDatas")]
        public void AutoCreateItems(MainViewModelData.ItemAndOrderData data)
        {
            vm.ItemList = data.ItemList;
            vm.OrderItems = data.OrderItemList;
            vm.AutoCreateItems();
            Assert.AreEqual(data.ItemListCount, vm.ItemList.Count, data.Title);
        }

        [TestCaseSource(typeof(MainViewModelData), "ItemAndOrderDatas")]
        public void DeletedItemsFromItemList(MainViewModelData.ItemAndOrderData data)
        {
            vm.ItemList = new List<Item>(data.ItemList?? new List<Item>());
            vm.DeletedItemsFromItemList(data.ItemList);
            if (vm.ItemList != null)
            {
                Assert.AreEqual(vm.ItemList.Count, 0, data.Title);
            }
        }

        [Test]
        public void EditItemListItem()
        {
            var item = new Item
            {
                SKU="0",
                Name = "item1",
                ItemCost = 1000,
                AmazonSKU="A1",
                EbaySKU="E1",
                QuantityBought=1,
                QuantitySold=1,
            };

            var ItemList = new List<Item>();
            ItemList.Add(item);
            vm.ItemList = ItemList;
            vm.EditItemListItem(item, "SKU", "1");
            vm.EditItemListItem(item, "Name", "item2");
            vm.EditItemListItem(item, "Amazon SKU", "A2");
            vm.EditItemListItem(item, "Item Cost", "$11.00");
            vm.EditItemListItem(item, "Ebay SKU", "E2");
            vm.EditItemListItem(item, "Quantity Bought", "2");
            vm.EditItemListItem(item, "Quantity Sold", "2");
            Assert.AreEqual("1", vm.ItemList[0].SKU);
            Assert.AreEqual("item2", vm.ItemList[0].Name);
            Assert.AreEqual("A2", vm.ItemList[0].AmazonSKU);
            Assert.AreEqual("E2", vm.ItemList[0].EbaySKU);
            Assert.AreEqual(2, vm.ItemList[0].QuantityBought);
            Assert.AreEqual(2, vm.ItemList[0].QuantitySold);
            Assert.AreEqual(1100, vm.ItemList[0].ItemCost);
        }
    }
}