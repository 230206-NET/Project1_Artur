namespace DataAccess;

internal class Secrets {
    private const string _connection = "Server=tcp:project1db-artur.database.windows.net,1433;Initial Catalog=Project1;Persist Security Info=False;User ID=artur;Password=Revature!999;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    public static string getConnectionString() => _connection;

}
