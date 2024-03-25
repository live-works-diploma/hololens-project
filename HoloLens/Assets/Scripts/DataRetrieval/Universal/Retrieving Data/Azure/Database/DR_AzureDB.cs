using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;
public class DR_AzureDB<T> : IDataRetrieval<T>, IAzure where T : class
{
    public string connectionString;

    public void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData)
    {
        Action<SqlDataReader> dataReader = reader =>
        {
            while (reader.Read())
            {
                throw new Exception();
            }
        };

        foreach (var type in expectedTypes.Keys)
        {
            AccessDatabase(type, dataReader);
        }
    }

    Dictionary<string, Type> expectedTypes;
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }

    void AccessDatabase(string tableName, Action<SqlDataReader> whatToDoWithDatabaseConnection)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Debug.Log("Connected to Azure SQL Database!");

                string query = $"SELECT * FROM {tableName}";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    whatToDoWithDatabaseConnection(reader);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error connecting to Azure SQL Database: " + ex.Message);
            }
        }
    }
}
