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
            connection = new SQLiteConnection("Data Source= datebase.sqlite3");
        }

        public IDBResult OpenDatabase(string DBLocation)
        {
            var result = new SQLiteResult(); 
            try
            {
                if (File.Exists(DBLocation))
                {
                    connection = new SQLiteConnection("Data Source= datebase.sqlite3");
                    result.Message = "Successfully connected to database.";
                    result.Result = "SUCCESS";
                }
            }
            catch(Exception ex)
            {
                result.Result = "ERROR";
                result.Message = ex.Message;

            }
            result.Message = "Unable to locate database.";
            result.Result = "FILE DOES NOT EXIT";
            return result;
        }

        public IDBResult Insert(string table, List<string>columns, IEnumerable items)
        {
            var itemStrings = new List<string> { Collection items}
            var result = new SQLiteResult();
            try
            {
                
                if (columns.Count != items.Count)
                {
                    result.Result = "ERROR";
                    result.Message = "Number of columns and number of items do not match.";
                    return result;
                }
                var query = $"INSERT INTO {table} {convertItemsToQueryString(columns)}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                OpenConnection();
                for (var i = 0; i < items.Count; i++)
                {
                    command.Parameters.AddWithValue($"@{columns[i]}", items[i]);
                }
                var rows = command.ExecuteNonQuery();

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
        public IDBResult Select(string table)
        {
            var result = new SQLiteResult();
            try
            {
                var query = $"SELECT * FROM {table}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                OpenConnection();
                var reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    result.Result = reader;
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

            query.Remove(query.LastIndexOf(","));
            query += ") VALUES (";
            foreach (var item in col)
            {
                query += $"@{item}, ";
            }

            query.Remove(query.LastIndexOf(","));
            query += ")";
            return query;
        }
    }
}