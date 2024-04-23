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

namespace DatabaseFunctions.Models.Database.Items
{
    public class ModelDBItemRead
    {
        public Dictionary<string, List<Dictionary<string, string>>>? GetData(ILogger logger, string[] dataTypes, string conditions, bool blobStorage = true)
        {
            if (blobStorage)
            {
                logger.LogInformation("Retrieving data from blob storage");
                return BlobStorageGet(logger, dataTypes, conditions);
            }
            else
            {
                logger.LogInformation("Retrieving data from database");
                return DatabaseGet(logger, dataTypes, conditions);
            }

            throw new NotImplementedException();
        }

        Dictionary<string, List<Dictionary<string, string>>>? DatabaseGet(ILogger logger, string[] tableNames, string conditions)
        {
            logger.LogInformation($"conditions: {conditions}");

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
                var instancesFound = ModelDBConnect.AccessDatabase(logger, tableNames[i], ModelDBAccountInfo.builder, function);

                if (instancesFound == null)
                {
                    return null;
                }

                string strippedKey = Regex.Match(tableNames[i], @"\[dbo\]\.\[(.*?)\]").Groups[1].Value;
                string key = strippedKey == "" ? tableNames[i] : strippedKey;

                logger.LogInformation($"key: {key}, stripped key: {strippedKey}");
                allData[key] = instancesFound;
            }

            return allData;
        }

        Dictionary<string, List<Dictionary<string, string>>>? BlobStorageGet(ILogger logger, string[] tableNames, string conditions)
        {
            throw new NotImplementedException();
        }
    }
}
