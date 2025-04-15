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

            var connectionString = $"Server={dbServer},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt=False;";

            return connectionString;
        }
    }
}
