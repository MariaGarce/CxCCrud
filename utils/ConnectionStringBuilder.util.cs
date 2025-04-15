namespace CRUDCxC.Utils
{
    public static class ConnectionStringBuilder
    {
        public static string BuildFromEnvironment()
        {
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            bool isAzureSql = dbServer?.Contains(".database.windows.net") == true;

            var userId = isAzureSql ? $"{dbUser}@{dbServer?.Split('.')[0]}" : dbUser;
            var encrypt = isAzureSql ? "True" : "False";
            var trustCert = isAzureSql ? "False" : "True";
            var connectionString = $"Server={dbServer},{dbPort};Database={dbName};User Id={userId};Password={dbPassword};Encrypt={encrypt};TrustServerCertificate={trustCert};Connection Timeout=30;";

            return connectionString;
        }
    }
}
