using Microsoft.Data.SqlClient;
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
        public static void DatabaseSave(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<Dictionary<string, string>>> contentToSave)
        {
            Action<string, SqlConnection> saveToDatabase = (tableName, connection) =>
            {
                List<Dictionary<string, string>> content = contentToSave[tableName];

                for (int i = 0; i < content.Count; i++)
                {
                    string columns = string.Join(",", content[i].Keys.Select(k => $"[{k}]"));
                    string values = string.Join(",", content[i].Keys.Select(k => $"@{k}"));
                    string query = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values})";

                    logger.LogInformation($"query for entering into database: {query}");

                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            foreach (var kvp in content[i])
                            {
                                command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                            }

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Error sending command to database: {ex.Message}");
                    }
                }
            };

            foreach (var type in contentToSave.Keys)
            {
                DatabaseConnection.AccessDatabase(logger, type, builder, saveToDatabase);
            }
        }

        public static void DatabaseUpdate()
        {

        }
    }
}
