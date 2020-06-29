using System.Collections;
using System.Collections.Generic;

namespace EmJayLib
{
    public interface IDatabase
    {
        IDBResult OpenDatabase(string dbLocation);
        IDBResult Insert(string table, List<string> columns, List<object> items);
        IDBResult Select(string table, string column, Dictionary<string, object> items);
        bool Exist(string table, string column, int value);
        IDBResult Update(string table, int rowID, List<string> columns, List<object> values);
        IDBResult CreateDatabase(string dbLocation, List<string> tables);
    }
}