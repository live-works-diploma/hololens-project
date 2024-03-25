using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.NewFolder
{
    public class DatabaseControlAdv
    {
        public async Task CreateTables(string connectionString, Dictionary<string, List<string>> tablesAndFields)
        {
            foreach (var table in tablesAndFields)
            {
                await CreateNewTable(connectionString, table.Key, table.Value);
            }
        }

        async Task CreateNewTable(string connectionString, string tableName, List<string> columns)
        {
            string columnString = String.Join(", ", columns);
            string query = $"CREATE TABLE {tableName} ({columnString})";

            Action<string, SqlConnection> createNewTable = (tableName, connection) =>
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {

                }
            };

            await DatabaseControl.AccessDatabase(tableName, connectionString, createNewTable);
        }
    }
}
