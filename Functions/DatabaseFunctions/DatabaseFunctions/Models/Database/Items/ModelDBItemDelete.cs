using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.Models.Database.Items
{
    public class ModelDBItemDelete
    {
        public static void DatabaseDeleteAll(ILogger logger, SqlConnectionStringBuilder builder, Dictionary<string, List<Dictionary<string, string>>> contentToDelete)
        {
            foreach (var tableName in contentToDelete.Keys)
            {
                DatabaseDeleteItems(logger, builder, tableName, contentToDelete[tableName]);
            }
        }

        public static void DatabaseDeleteItems(ILogger logger, SqlConnectionStringBuilder builder, string tableName, List<Dictionary<string, string>> conditions)
        {
            string baseQuery = $"DELETE FROM {tableName}";

            Action<string, SqlConnection> deleteFromDatabase = (query, connection) =>
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    string fullQuery = query; // Start with the base query for each condition set
                    List<string> whereClauses = new List<string>();
                    foreach (var kvp in conditions[i])
                    {
                        whereClauses.Add($"{kvp.Key} = @{kvp.Key}");
                    }
                    if (whereClauses.Count > 0)
                    {
                        fullQuery += $" WHERE {string.Join(" AND ", whereClauses)}";
                    }

                    using (SqlCommand command = new SqlCommand(fullQuery, connection))
                    {
                        foreach (var kvp in conditions[i])
                        {
                            command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            };

            ModelDBConnect.AccessDatabase(logger, baseQuery, builder, deleteFromDatabase);
        }
    }
}
