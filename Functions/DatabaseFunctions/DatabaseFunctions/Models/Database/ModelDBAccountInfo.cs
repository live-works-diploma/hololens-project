using Microsoft.Data.SqlClient;

namespace DatabaseFunctions.Models.Database
{
    public class ModelDBAccountInfo
    {
        // database information
        static SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();

        static string DataSource = "tcp:hololenssqlserver1.database.windows.net";
        static string UserID = "sqladmin";
        static string Password = "Bullet66!";
        static string InitialCatalog = "hololensSQLdb";

        //static string DataSource = "tcp:databasefunctionsdbserver.database.windows.net";
        //static string UserID = "GeorgeAdmin";
        //static string Password = "";
        //static string InitialCatalog = "DatabaseFunctions_db";

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
