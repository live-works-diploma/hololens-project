using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.Models.Database
{
    public class DatabaseSend
    {
        public static void DatabaseSaveAll(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<Dictionary<string, string>>> contentToSave)
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
            string insertQuery = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values})";

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        foreach (var kvp in record)
                        {
                            command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inserting record into database: {ex.Message}");
            }
        }

        public static void UpdateAllRecords(ILogger logger, SqlConnectionStringBuilder builder, string tableName, List<Dictionary<string, string>> records, string conditionColumn)
        {
            foreach (var record in records)
            {
                if (record.TryGetValue(conditionColumn, out var conditionValue))
                {
                    string condition = $"[{conditionColumn}] = '{conditionValue}'";
                    logger.LogInformation($"Condition: {condition}");
                    UpdateRecord(logger, builder, tableName, record, condition);
                }
                else
                {
                    logger.LogError($"Condition column '{conditionColumn}' not found in record.");
                }
            }
        }

        public static void UpdateRecord(ILogger logger, SqlConnectionStringBuilder builder, string tableName, Dictionary<string, string> record, string condition)
        {
            string setValues = string.Join(",", record.Keys.Where(k => k != "id").Select(k => $"[{k}] = @{k}"));
            string updateQuery = $"UPDATE [{tableName}] SET {setValues} WHERE {condition}";

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        foreach (var kvp in record)
                        {
                            command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error updating record in database: {ex.Message}");
            }
        }

    }
}
