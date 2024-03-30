using Microsoft.Data.SqlClient;

namespace DatabaseFunctions.Models
{
    public class AzureAccountInfo
    {
        // database information
        static SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
        static string DataSource = "tcp:databasefunctionsdbserver.database.windows.net";
        static string UserID = "GeorgeAdmin";
        static string Password = "Sponge2002!";
        static string InitialCatalog = "DatabaseFunctions_db";

        public static SqlConnectionStringBuilder builder
        {
            get
            {
                _builder.DataSource = DataSource;
                _builder.UserID = UserID;
                _builder.Password = Password;
                _builder.InitialCatalog = InitialCatalog;

                return _builder;
            }
        }
    }
}
