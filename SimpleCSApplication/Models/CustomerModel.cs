using MySql.Data.MySqlClient;
using SimpleCSApplication.Controllers;
namespace SimpleCSApplication.Models;
public class CustomerModel
{
  public int CustomerID { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
  public string PasswordHash { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public static bool CreateCustomer(CustomerModel model){
    var connection = Sql.I.GetConnection();
    if (connection == null) return false;

    try{
      const string query = @"
        INSERT INTO Customers (Name, Email, PasswordHash)
        VALUES (@Name, @Email, @PasswordHash);";

      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@Name", model.Name);
      command.Parameters.AddWithValue("@Email", model.Email);
      command.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
      return command.ExecuteNonQuery() > 0;
    }
    catch (Exception ex){
      Console.WriteLine($"Error creating customer: {ex.Message}");
    }
    return false;
  }
  public static CustomerModel? GetCustomerById(int id){
    var connection = Sql.I.GetConnection();
    if (connection == null) return null;

    try{
      const string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID LIMIT 1;";
      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@CustomerID", id);

      using var reader = command.ExecuteReader();
      if (reader.Read()){
        return new CustomerModel{
          CustomerID = Convert.ToInt32(reader["CustomerID"]),
          Name = reader["Name"].ToString(),
          Email = reader["Email"].ToString(),
          PasswordHash = reader["PasswordHash"].ToString(),
          CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
          UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
        };
      }
    }
    catch (Exception ex){
      Console.WriteLine($"Error retrieving customer by ID: {ex.Message}");
    }
    return null;
  }
  public static CustomerModel? GetCustomerByEmail(string mail){
    var connection = Sql.I.GetConnection();
    if (connection == null) return null;

    try{
      const string query = "SELECT * FROM Customers WHERE Email = @Email LIMIT 1;";
      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@Email", mail);

      using var reader = command.ExecuteReader();
      if (reader.Read())
      {
        return new CustomerModel
        {
          CustomerID = Convert.ToInt32(reader["CustomerID"]),
          Name = reader["Name"].ToString(),
          Email = reader["Email"].ToString(),
          PasswordHash = reader["PasswordHash"].ToString(),
          CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
          UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
        };
      }
    }
    catch (Exception ex){
        Console.WriteLine($"Error retrieving customer by email: {ex.Message}");
    }
    return null;
  }
  public static bool ValidateLogin(string email, string passwordHash){
    var connection = Sql.I.GetConnection();
    if (connection == null) return false;

    try{
      const string query = "SELECT PasswordHash FROM Customers WHERE Email = @Email LIMIT 1;";
      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@Email", email);

      using var reader = command.ExecuteReader();
      if (reader.Read())
      {
        var storedPasswordHash = reader["PasswordHash"].ToString();
        return storedPasswordHash == passwordHash; // Compare hashes
      }
    }
    catch (Exception ex){
      Console.WriteLine($"Error validating login: {ex.Message}");
    }
    return false;
  }
}
