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
        public async Task<bool> CreateTables(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<string>> tablesAndFields)
        {
            bool success = true;

            foreach (var table in tablesAndFields)
            {
                success = await CreateNewTable(logger, builder, table.Key, table.Value);

                if (!success)
                {
                    success = false;
                    break;
                }
            }

            return success;
        }

        async Task<bool> CreateNewTable(ILogger logger, SqlConnectionStringBuilder builder, string tableName, List<string> columns)
        {
            bool success = true;

            // Modify the columns list to include column names and types

            List<string> columnsWithTypes =
            [
                "Id INT IDENTITY(1,1) PRIMARY KEY", .. columns.Select(column => $"{column} VARCHAR(100)")
            ];

            string columnString = String.Join(", ", columnsWithTypes);

            logger.LogError($"column string: {columnString}");

            string query = $"CREATE TABLE {tableName} ({columnString})";

            logger.LogError($"query string: {query}");

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
                    logger.LogError($"Error creating new table: {ex.Message}");
                }

                return success;
            };

            success = await DatabaseControl.AccessDatabase(logger, tableName, builder, createNewTable);

            return success;
        }
    }
}
