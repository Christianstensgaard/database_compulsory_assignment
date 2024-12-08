using MySql.Data.MySqlClient;

namespace SimpleCSApplication.Controllers{
  public class Sql{
    public static Sql I { get; private set; } = new();
    private Sql() { }

    private MySqlConnection? _connection;
    private const string DefaultConnectionString = "Server=localhost;Port=3306;Database=Bookstore;Uid=root;Pwd=rootpassword;";

    public void Connect(string connectionString = DefaultConnectionString)
    {
      try
    {
        if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
        {
            Console.WriteLine("Already connected to the database.");
            return;
        }

        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        Console.WriteLine("Database connection established.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error connecting to database: {ex.Message}");
        _connection = null;
      }
    }

    public void Disconnect()
    {
      if (_connection != null)
      {
        _connection.Close();
        _connection = null;
        Console.WriteLine("Database connection closed.");
      }
    }

    public bool IsConnected()
    {
      return _connection != null && _connection.State == System.Data.ConnectionState.Open;
    }

    public MySqlConnection? GetConnection()
    {
      if (!IsConnected())
      {
        Connect();

        if(!IsConnected())
          return null;
      }
      return _connection;
    }
  }
}
