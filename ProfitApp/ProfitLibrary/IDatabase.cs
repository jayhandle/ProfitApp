using System.Collections;
using System.Collections.Generic;

namespace ProfitLibrary
{
    public interface IDatabase
    {
        IDBResult OpenDatabase(string dbLocation);
        IDBResult Insert(string table, List<string> columns, IEnumerable items);
        IDBResult Select(string table);
    }
}