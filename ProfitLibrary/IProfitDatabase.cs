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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns>returns the number of rows updated. If an error occured it will return -1, if no rows were updated, it will return 0.</returns>
        int Update(string Table, int row, string column, string value);
        string InsertItem(Item item);
        string InsertOrder(OrderItem order);
    }
}