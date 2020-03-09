using System.Collections.Generic;

namespace ProfitLibrary
{
    public interface IDBResult
    {
        object Result { get; set; }
        string Message { get; set; }
    }
}