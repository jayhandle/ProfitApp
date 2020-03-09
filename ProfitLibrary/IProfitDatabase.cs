using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProfitLibrary
{
    public interface IProfitDatabase
    {
        IList<OrderItem> GetOrderItems();
        IList<Item> GetItemList();
        IDBResult CreateDatabase(string dblocation);
        void SaveItemList(ObservableCollection<Item> itemList);
        void SaveOrderList(ObservableCollection<OrderItem> orderItems);
        void Update(string Table, int row, string column, string value);
    }
}