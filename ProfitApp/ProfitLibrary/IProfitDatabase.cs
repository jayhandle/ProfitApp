using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProfitLibrary
{
    public interface IProfitDatabase
    {
        IList<OrderItem> GetOrderItems();
        IList<Item> GetItemList();
        void CreateDatabase(string profitFileLocation, string itemListLocation);
    }
}