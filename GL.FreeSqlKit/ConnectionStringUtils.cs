namespace GL.FreeSqlKit
{
    public class ConnectionStringUtils
    {
        public static string SqlServerConnectionString(string server, string username, string password, string database)
            => $"Server={server};User ID={username};Password={password};Database={database}";
    }
}
