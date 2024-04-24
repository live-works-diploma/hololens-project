using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DatabaseFunctions.Models.Information
{
    public class ModelDBAccountInfo
    {
        // database information
        static SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();

        //static string DataSource = "tcp:hololenssqlserver1.database.windows.net";
        //static string UserID = "sqladmin";
        //static string Password = "Bullet66!";
        //static string InitialCatalog = "hololensSQLdb";
        
        static string DataSource = "tcp:databasefunctionsdbserver.database.windows.net,1433";
        static string UserID = "GeorgeAdmin";
        static string Password = "Sponge2002!";
        static string InitialCatalog = "DatabaseFunctions_db";

        // Server=tcp:databasefunctionsdbserver.database.windows.net,1433;
        // Initial Catalog = DatabaseFunctions_db; Encrypt=True;
        // TrustServerCertificate=False;
        // Connection Timeout = 30;
        // Authentication="Active Directory Default";

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
