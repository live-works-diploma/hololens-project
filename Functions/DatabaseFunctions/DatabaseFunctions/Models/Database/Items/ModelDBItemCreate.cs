using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.Models.Database.Items
{
    public class ModelDBItemCreate
    {
        public static void DatabaseInsertAll(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<Dictionary<string, string>>> contentToSave)
        {
            foreach (var tableName in contentToSave.Keys)
            {
                List<Dictionary<string, string>> content = contentToSave[tableName];

                for (int i = 0; i < content.Count; i++)
                {
                    InsertRecord(logger, builder, tableName, content[i]);
                }
            }
        }

        public static void InsertRecord(ILogger logger, SqlConnectionStringBuilder builder, string tableName, Dictionary<string, string> record)
        {
            string columns = string.Join(",", record.Keys.Select(k => $"[{k}]"));
            string values = string.Join(",", record.Keys.Select(k => $"@{k}"));
            string insertQuery = $"INSERT INTO [dbo].[{tableName}] ({columns}) VALUES ({values})";

            logger.LogInformation($"Insert Query: {insertQuery}");

            Action<string, SqlConnection> whatToDoWithDatabase = (query, connection) =>
            {
                logger.LogInformation($"Insert Query: {insertQuery}");
                logger.LogInformation($"");

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    foreach (var kvp in record)
                    {
                        command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                    }

                    command.ExecuteNonQuery();
                }
            };

            try
            {
                ModelDBConnect.AccessDatabase(logger, insertQuery, builder, whatToDoWithDatabase);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inserting record into database: {ex.Message}");
            }
        }
    }
}
