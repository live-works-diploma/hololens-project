using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DatabaseFunctions.Models.Database
{
    public class DatabaseConnection
    {
        static void OpenConnection(SqlConnection connection, ILogger logger)
        {
            try
            {
                connection.Open();
                logger.LogInformation("Connected to SQL Server.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error opening connection: {ex.Message}");
                throw;
            }
        }

        static void CloseConnection(SqlConnection connection, ILogger logger)
        {
            try
            {
                connection.Close();
                logger.LogInformation("Connection closed.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error closing connection: {ex.Message}");
                throw;
            }
        }

        static void LogConnectionError(ILogger logger, string message)
        {
            logger.LogError($"Error connecting to database: {message}");
        }

        public static List<Dictionary<string, string>>? AccessDatabase(ILogger logger, string tableName, SqlConnectionStringBuilder builder, Func<string, SqlConnection, List<Dictionary<string, string>>> whatToDoWithDatabaseConnection)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                OpenConnection(connection, logger);
                try
                {
                    List<Dictionary<string, string>> instancesFound = whatToDoWithDatabaseConnection(tableName, connection);
                    return instancesFound;
                }
                catch (Exception ex)
                {
                    LogConnectionError(logger, ex.Message);
                    return null;
                }
                finally
                {
                    CloseConnection(connection, logger);
                }                
            }
        }

        public static void AccessDatabase(ILogger logger, string tableName, SqlConnectionStringBuilder builder, Action<string, SqlConnection> whatToDoWithDatabase)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                OpenConnection(connection, logger);
                try
                {
                    whatToDoWithDatabase(tableName, connection);
                }
                catch (Exception ex)
                {
                    LogConnectionError(logger, ex.Message);
                }
                finally
                {
                    CloseConnection(connection, logger);
                }
            }
        }
    }
}
