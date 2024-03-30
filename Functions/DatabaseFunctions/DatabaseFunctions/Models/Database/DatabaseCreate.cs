using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.Models.Database
{
    public class DatabaseCreate
    {
        public static bool CreateTables(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<string>> tablesAndFields)
        {
            bool success = true;

            foreach (var table in tablesAndFields)
            {
                success = CreateNewTable(logger, builder, table.Key, table.Value);

                if (!success)
                {
                    success = false;
                    break;
                }
            }

            return success;
        }

        static bool CreateNewTable(ILogger logger, SqlConnectionStringBuilder builder, string tableName, List<string> columns)
        {
            bool success = true;

            // Modify the columns list to include column names and types

            List<string> columnsWithTypes = new List<string>()
            {
                "id INT PRIMARY KEY IDENTITY(1,1)"
            };

            // Add the remaining columns
            for (int i = 0; i < columns.Count; i++)
            {
                columnsWithTypes.Add($"{columns[i]} VARCHAR(100)");
            }

            Action<string, SqlConnection> createNewTable = (tablename, connection) =>
            {
                try
                {
                    string columnString = String.Join(", ", columnsWithTypes);
                    string query = $"CREATE TABLE {tableName} ({columnString})";

                    logger.LogInformation($"column string: {columnString}");
                    logger.LogInformation($"query string: {query}");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    logger.LogError($"Error creating new table: {ex.Message}");
                }
            };

            DatabaseConnection.AccessDatabase(logger, tableName, builder, createNewTable);

            return success;
        }
    }
}
