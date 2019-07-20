using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfitLibrary
{
    public class OrderItem
    {
        public bool Assigned { get; set; }
        public string SKU { get; set; }
        public string OrderID { get; set; }
        public string ItemName { get; set; }
        public int QuantitySold { get; set; }
        public string DateSold { get; set; }
        public long SoldFor { get; set; }
        public string BoughtFrom { get; set; }
        public long ItemCost { get; set; }
        public long ShippingCost { get; set; }
        public long SellingFees { get; set; }      
        public long Profit { get; set; }
    }
}
