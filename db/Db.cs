using System.Configuration;
using System.Data.SqlClient;

namespace BankTimeNET.db
{
    public static class Db
    {
        public static SqlConnection connect()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["BankTimeNETConnection"].ConnectionString;
            sqlConnection.Open();

            return sqlConnection;
        } 
    }
}
