using Azure;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.NewFolder
{
    public class DatabaseControl
    {
        ILogger logger;       

        public DatabaseControl(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<bool> GetFromDatabase(SqlConnectionStringBuilder builder, List<string> tableNames)
        {
            Dictionary<string, List<Dictionary<string, string>>> foundData = new Dictionary<string, List<Dictionary<string, string>>>();

            bool success = true;

            Func<string, SqlConnection, bool> retrieveFromDatabase = (tableName, connection) =>
            {
                string query = $"SELECT * FROM {tableName}";

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool success = true;

                        logger.LogInformation("Connected through connection.");

                        List<Dictionary<string, string>> allInstances = new List<Dictionary<string, string>>();

                        while (reader.Read())
                        {
                            Dictionary<string, string> instance = new Dictionary<string, string>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                string columnValue = reader.GetValue(i).ToString() ?? "not set";
                                instance[columnName] = columnValue;
                            }

                            allInstances.Add(instance);
                        }

                        foundData[tableName] = allInstances;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error Sending Command to Database: {ex.Message}");
                    success = false;
                }

                return success;
            };

            for (int i = 0; i < tableNames.Count; i++)
            {
                success = await AccessDatabase(logger, tableNames[i], builder, retrieveFromDatabase);

                if (!success)
                {
                    break;
                }
            }

            string jsonData = JsonConvert.SerializeObject(foundData);

            return success;
        }


        public async Task<bool> SaveToDatabase(SqlConnectionStringBuilder builder, Dictionary<string, List<Dictionary<string, string>>> contentToSave)
        {
            bool success = true;

            Func<string, SqlConnection, bool> saveToDatabase = (tableName, connection) =>
            {
                bool success = true;

                List<Dictionary<string, string>> content = contentToSave[tableName];

                for (int i = 0; i < content.Count; i++)
                {
                    string columns = string.Join(",", content[i].Keys.Select(k => $"@{k}"));
                    string values = string.Join(",", content[i].Keys.Select(k => $"@{k}"));
                    string query = $"INSERT INTO @{tableName} ({columns}) VALUES ({values})";

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
                        success = false;
                        logger.LogError($"Error sending command to database: {ex.Message}");
                    }
                }

                return success;
            };

            foreach (var type in contentToSave.Keys)
            {
                success = await AccessDatabase(logger, type, builder, saveToDatabase);

                if (!success)
                {
                    break;
                }
            }

            return success;
        }

        public static async Task<bool> AccessDatabase(ILogger logger, string tableName, SqlConnectionStringBuilder builder, Func<string, SqlConnection, bool> whatToDoWithDatabaseConnection)
        {
            bool success = true;

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    logger.LogInformation("Connected to sqlconnection");

                    connection.Open();

                    success = whatToDoWithDatabaseConnection(tableName, connection);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                success = false;
                logger.LogError($"Error Accessing database: {ex.Message}");
            }

            return success;
        }
    }   
}
