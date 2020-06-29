using System.Collections.Generic;

namespace EmJayLib
{
    public interface IDBResult
    {
        object Result { get; set; }
        string Message { get; set; }
    }
}