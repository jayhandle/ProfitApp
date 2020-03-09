using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace ProfitLibrary
{
    public class SQLiteDatabase : IDatabase
    {
        private SQLiteConnection connection;
        public SQLiteDatabase()
        {
        
        }

        public IDBResult OpenDatabase(string DBLocation)
        {
            var result = new SQLiteResult();
            if (string.IsNullOrWhiteSpace(DBLocation))
            {
                result.Message = "Unable to locate database.";
                result.Result = "FILE DOES NOT EXIT";
            }
            
            try
            {
                if (File.Exists(DBLocation))
                {
                    connection = new SQLiteConnection($"Data Source = {DBLocation}");
                    result.Message = "Successfully connected to database.";
                    result.Result = "SUCCESS";
                }
                else
                {
                    result.Message = "Unable to locate database.";
                    result.Result = "FILE DOES NOT EXIT";
                }
            }
            catch(Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;

            }
            
            return result;
        }

        public IDBResult Insert(string table, List<string>columns, List<object> items)
        {
            //var itemStrings = itemList;
            var result = new SQLiteResult();
            var rows = 0;
            try
            {
                var query = $"INSERT INTO {table} {convertItemsToQueryString(columns)}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                OpenConnection();
                for (var i = 0; i < items.Count; i++)
                {
                    command.Parameters.AddWithValue($"@{columns[i]}", items[i]);
                }
                    
                rows += command.ExecuteNonQuery();
                
                result.Message = $"Successfully inserted items into the table. rows : {rows}.";
                result.Result = "SUCCESS";
            }
            catch(Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;
            }

            CloseConnection();
            return result;
        }

        private List<object> convertToList(IEnumerable items)
        {
            var list = new List<object>();
            var enumerator = items.GetEnumerator();
            //list.Add(enumerator.Current);
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }

        public IDBResult Select(string table, string column, Dictionary<string,object> items)
        {
            var result = new SQLiteResult();
            try
            {
                var query = $"SELECT {column} FROM {table}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                OpenConnection();
                var reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    var data = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        var keys = new List<string>(items.Keys);
                        var tempItem = new Dictionary<string, object>();
                        foreach (var key in keys)
                        {
                            tempItem.Add(key, reader[key]);
                        }
                        data.Add(tempItem);
                    }
                    result.Result = data;
                    result.Message = "SUCCESS";
                }
                else
                {
                    result.Message = "Table is empty";
                }
            }
            catch (Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;
            }

            CloseConnection();
            return result;
        }
        private void OpenConnection()
        {
            if(connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }
        private void CloseConnection()
        {
            if(connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }
        private string convertItemsToQueryString(List<string> col)
        {
           var query = "(";
            foreach (var item in col)
            {
                query += $"'{item}', ";
            }

            query = query.Remove(query.LastIndexOf(","));
            query += ") VALUES (";
            foreach (var item in col)
            {
                query += $"@{item}, ";
            }

            query = query.Remove(query.LastIndexOf(","));
            query += ")";
            return query;
        }

        public bool Exist(string table, string column, int value)
        {
            var result = new SQLiteResult();
            try
            {
                var query = $"SELECT * FROM {table} WHERE {column} = {value}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                OpenConnection();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;
            }

            CloseConnection();
            return false;
        }

        public IDBResult Update(string table, int rowID, List<string> columns, List<object> values)
        {
            var result = new SQLiteResult();
            try
            {
                var query = $"UPDATE {table} SET {convertToUpdateQuery(columns, values)} WHERE id = {rowID}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                OpenConnection();
                var rows = command.ExecuteNonQuery();
            
                result.Message = $"Successfully updated items in the table. rows : {rows}.";
                result.Result = "SUCCESS";

        }
            catch (Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;
            }

            CloseConnection();
            return result;
        }

        private string convertToUpdateQuery(List<string> columns, List<object> values)
        {
            var q = string.Empty;
            for(int i = 0; i<columns.Count;i++)
            {
                q += $"{columns[i]} = '{values[i]}',";
            }

            q = q.Remove(q.LastIndexOf(','));

            return q;
        }

        public IDBResult CreateDatabase(string dbLocation, List<string> tables)
        {
            connection = new SQLiteConnection($"Data Source= {dbLocation}");
            //var itemStrings = itemList;
            var result = new SQLiteResult();
            var rows = 0;
            try
            {
                foreach(var table in tables)
                {
                    var query = table;
                    var command = new SQLiteCommand(query, connection);
                    OpenConnection();
                    rows += command.ExecuteNonQuery();
                }
                result.Message = $"Successfully added table(s). rows : {rows}.";
                result.Result = "SUCCESS";
            }
            catch (Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;
            }

            CloseConnection();
            return result;
        }
    }
}