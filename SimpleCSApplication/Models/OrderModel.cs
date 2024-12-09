using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using MySql.Data.MySqlClient;
using SimpleCSApplication.Controllers;

namespace SimpleCSApplication.Models;
public class OrderModel
{
  [JsonIgnore]
  public int OrderID { get; set; }
  public int CustomerID { get; set; }
  public int BookID { get; set; }
  public int Quantity { get; set; } = 1;
  public bool Paid { get; set; }  = false;
  [JsonIgnore]
  public DateTime CreatedAt { get; set; }
  [JsonIgnore]
  public DateTime UpdatedAt { get; set; }

  [JsonIgnore]
  public CustomerModel Customer { get; set; }
  [JsonIgnore]
  public BookModel Book { get; set; }

  public static bool CreateOrder(OrderModel model)
  {
    try{
      var connection = Sql.I.GetConnection();
      if (connection == null) return false;

      const string query = @"
          INSERT INTO Orders (CustomerID, BookID, Quantity, Paid)
          VALUES (@CustomerID, @BookID, @Quantity, @Paid);
      ";
      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@CustomerID", model.CustomerID);
      command.Parameters.AddWithValue("@BookID", model.BookID);
      command.Parameters.AddWithValue("@Quantity", model.Quantity);
      command.Parameters.AddWithValue("@Paid", false);

      int rowsAffected = command.ExecuteNonQuery();
      return rowsAffected > 0;
    }
    catch (Exception ex){
      Console.WriteLine($"Error creating order: {ex.Message}");
      return false;
    }
  }

public static List<OrderModel>? GetOrders(int customerId)
{
  var cache = Redis.I.database;
  try{
    string cacheKey = $"orders:customer:{customerId}";

    string? cachedOrders = cache.StringGet(cacheKey);
    if (!string.IsNullOrEmpty(cachedOrders)){
      return JsonSerializer.Deserialize<List<OrderModel>>(cachedOrders);
    }

    var connection = Sql.I.GetConnection();
    if (connection == null) return null;

    const string procedureName = "GetOrdersByCustomerId";
    using var command = new MySqlCommand(procedureName, connection){
        CommandType = CommandType.StoredProcedure
    };
    command.Parameters.AddWithValue("@customerId", customerId);

    using var reader = command.ExecuteReader();
    var orders = new List<OrderModel>();
    while (reader.Read()){
      var order = new OrderModel{
        OrderID = Convert.ToInt32(reader["OrderID"]),
        CustomerID = Convert.ToInt32(reader["CustomerID"]),
        BookID = Convert.ToInt32(reader["BookID"]),
        Quantity = Convert.ToInt32(reader["Quantity"]),
        Paid = Convert.ToBoolean(reader["Paid"]),
        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),

        Customer = new CustomerModel{
            CustomerID = Convert.ToInt32(reader["CustomerID"]),
            Name = reader["CustomerName"].ToString(),
            Email = reader["CustomerEmail"].ToString()
        },
        Book = new BookModel{
            Id = Convert.ToInt32(reader["BookID"]),
            Title = reader["BookTitle"].ToString(),
            AuthorId = Convert.ToInt32(reader["BookAuthorID"])
        }
      };
      orders.Add(order);
    }

    string serializedOrders = JsonSerializer.Serialize(orders);
    cache.StringSet(cacheKey, serializedOrders, TimeSpan.FromMinutes(120));
    return orders;
  }
  catch (Exception ex){
    Console.WriteLine($"Error retrieving orders: {ex.Message}");
    return null;
  }
}



}