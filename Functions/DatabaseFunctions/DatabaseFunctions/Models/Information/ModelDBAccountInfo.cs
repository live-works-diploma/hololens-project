using Microsoft.Data.SqlClient;

namespace DatabaseFunctions.Models.Information
{
    public class ModelDBAccountInfo
    {
        // database information
        static SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();

        static string DataSource = "tcp:hololenssqlserver1.database.windows.net";
        static string UserID = "sqladmin";
        static string Password = "Bullet66!";
        static string InitialCatalog = "hololensSQLdb";

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
