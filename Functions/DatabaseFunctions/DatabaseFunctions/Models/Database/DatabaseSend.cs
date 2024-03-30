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
        public static void DatabaseSave(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<Dictionary<string, string>>> contentToSave, bool allowUpdate, string condition)
        {
            foreach (var tableName in contentToSave.Keys)
            {
                List<Dictionary<string, string>> content = contentToSave[tableName];

                foreach (var record in content)
                {
                    string columns = string.Join(",", record.Keys.Select(k => $"[{k}]"));
                    string values = string.Join(",", record.Keys.Select(k => $"@{k}"));
                    string setValues = string.Join(",", record.Keys.Select(k => $"[{k}] = @{k}"));

                    string insertQuery = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values})";
                    string updateQuery = $"UPDATE [{tableName}] SET {setValues} WHERE {condition}";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                        {
                            connection.Open();

                            string commandText = insertQuery;
                            if (allowUpdate && !string.IsNullOrEmpty(condition))
                            {
                                string selectQuery = $"SELECT COUNT(*) FROM [{tableName}] WHERE {condition}";
                                using (SqlCommand checkCommand = new SqlCommand(selectQuery, connection))
                                {
                                    int count = (int)checkCommand.ExecuteScalar();
                                    if (count > 0)
                                    {
                                        commandText = updateQuery;
                                    }
                                }
                            }

                            using (SqlCommand command = new SqlCommand(commandText, connection))
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
                        logger.LogError($"Error sending command to database: {ex.Message}");
                    }
                }
            }
        }

        public static void DatabaseUpdate()
        {

        }
    }
}
