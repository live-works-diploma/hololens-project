using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DatabaseFunctions.Models.Database.Items
{
    public class ModelDBItemRead
    {
        public static Dictionary<string, List<Dictionary<string, string>>>? DatabaseGet(ILogger logger, SqlConnectionStringBuilder builder, string[] tableNames, string conditions)
        {
            Func<string, SqlConnection, List<Dictionary<string, string>>> function = (tableName, connection) =>
            {
                string query = conditions == "" ? $"SELECT * FROM {tableName}" : $"SELECT * FROM {tableName} WHERE {conditions}";

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
                            string? columnValue = reader.GetValue(i).ToString();

                            if (columnValue == null)
                            {
                                logger.LogError($"Error getting data for column name: {columnName}");
                                continue;
                            }

                            instance[columnName] = columnValue;
                        }

                        allInstances.Add(instance);
                    }

                    return allInstances;
                }
            };

            Dictionary<string, List<Dictionary<string, string>>> allData = new();

            for (int i = 0; i < tableNames.Length; i++)
            {
                var instancesFound = ModelDBConnect.AccessDatabase(logger, tableNames[i], builder, function);

                if (instancesFound == null)
                {
                    return null;
                }

                string strippedKey = Regex.Match(tableNames[i], @"\[dbo\]\.\[(.*?)\]").Groups[1].Value;
                allData[strippedKey] = instancesFound;
            }

            return allData;
        }
    }
}
