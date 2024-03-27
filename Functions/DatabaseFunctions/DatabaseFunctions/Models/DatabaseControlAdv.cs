using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.NewFolder
{
    public class DatabaseControlAdv
    {
        public async Task<(bool success, string failMessage)> CreateTables(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<string>> tablesAndFields)
        {
            bool success = true;
            string failMessage = "";

            foreach (var table in tablesAndFields)
            {
                (success, failMessage) = await CreateNewTable(logger, builder, table.Key, table.Value);

                if (!success)
                {
                    success = false;
                    break;
                }
            }

            return (success, failMessage);
        }

        async Task<(bool success, string failMessage)> CreateNewTable(ILogger logger, SqlConnectionStringBuilder builder, string tableName, List<string> columns)
        {
            bool success = true;
            string failMessage = "";

            // Modify the columns list to include column names and types

            List<string> columnsWithTypes = new List<string>()
            {
                $"{columns[0]} VARCHAR(100) PRIMARY KEY"
            };

            // Add the remaining columns
            for (int i = 1; i < columns.Count; i++)
            {
                columnsWithTypes.Add($"{columns[i]} VARCHAR(100)");
            }

            string columnString = String.Join(", ", columnsWithTypes);            
            string query = $"CREATE TABLE {tableName} ({columnString})";

            logger.LogInformation($"column string: {columnString}");
            logger.LogInformation($"query string: {query}");

            Func<string, SqlConnection, bool> createNewTable = (tableName, connection) =>
            {
                bool success = true;

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    failMessage = "Error creating new table: {ex.Message}";
                    logger.LogError(failMessage);
                }

                return success;
            };

            (success, failMessage) = await DatabaseControl.AccessDatabase(logger, tableName, builder, createNewTable);

            return (success, failMessage);
        }
    }
}
