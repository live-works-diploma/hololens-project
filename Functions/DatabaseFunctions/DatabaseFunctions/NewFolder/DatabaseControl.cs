using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.NewFolder
{
    public class DatabaseControl
    {
        public async Task GetFromDatabase(HttpResponseData response, string connectionString, List<string> tableNames)
        {
            Dictionary<string, List<Dictionary<string, string>>> foundData = new Dictionary<string, List<Dictionary<string, string>>>();

            Action<string, SqlConnection> retrieveFromDatabase = (tableName, connection) =>
            {
                string query = $"SELECT * FROM {tableName}";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
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
            };

            for (int i = 0; i < tableNames.Count; i++)
            {
                await AccessDatabase(tableNames[i], connectionString, retrieveFromDatabase);
            }

            try
            {
                string jsonData = JsonConvert.SerializeObject(foundData);
                await response.WriteAsJsonAsync(jsonData);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SaveToDatabase(string connectionString, Dictionary<string, List<Dictionary<string, string>>> contentToSave)
        {
            Action<string, SqlConnection> saveToDatabase = (tableName, connection) =>
            {
                List<Dictionary<string, string>> content = contentToSave[tableName];

                for (int i = 0; i < content.Count; i++)
                {
                    string columns = string.Join(",", content[i].Keys);
                    string values = string.Join(",", content[i].Values.Select(v => $"@{v}"));
                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

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
                    catch (Exception e)
                    {

                    }
                }
            };

            foreach (var type in contentToSave.Keys)
            {
                await AccessDatabase(type, connectionString, saveToDatabase);
            }
        }

        public static async Task AccessDatabase(string tableName, string connectionString, Action<string, SqlConnection> whatToDoWithDatabaseConnection)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        whatToDoWithDatabaseConnection(tableName, connection);

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error connecting to Azure SQL Database: " + ex.Message);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
