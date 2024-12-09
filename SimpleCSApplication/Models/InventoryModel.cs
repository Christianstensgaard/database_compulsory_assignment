using MySql.Data.MySqlClient;
using SimpleCSApplication.Controllers;

namespace SimpleCSApplication.Models;
public class InventoryModel{
  public int ItemID { get; set; }
  public int ProductID { get; set; }
  public int Quantity { get; set; } = 0;

  public static bool UpdateInventoryQuantity(int ProductID, int addAmount){
    try{
      var connection = Sql.I.GetConnection();
      if (connection == null) return false;

      const string query = "UPDATE Inventory SET Quantity = Quantity + @AddAmount WHERE ProductID = @ProductID;";
      using var command = new MySqlCommand(query, connection);

      command.Parameters.AddWithValue("@ProductID", ProductID);
      command.Parameters.AddWithValue("@AddAmount", addAmount);

      var rowsAffected = command.ExecuteNonQuery();
      if (rowsAffected > 0){
        var cache = Redis.I.database;
        cache.KeyDelete($"book:{ProductID}:quantity");
        return true;
      }
    }
    catch (Exception ex){
      Console.WriteLine($"Error updating inventory: {ex.Message}");
    }
    return false;
  }

  public static List<InventoryModel>? GetInventory(){
    try{
      var connection = Sql.I.GetConnection();
      if (connection == null) return null;

      const string query = "SELECT * FROM Inventory;";
      using var command = new MySqlCommand(query, connection);
      using var reader = command.ExecuteReader();

      var inventory = new List<InventoryModel>();
      while (reader.Read()){
        var inventoryItem = new InventoryModel{
            ProductID = Convert.ToInt32(reader["ProductID"]),
            Quantity = Convert.ToInt32(reader["Quantity"]),
        };
        inventory.Add(inventoryItem);
      }
      return inventory;
    }
    catch (Exception ex){
        Console.WriteLine($"Error getting inventory: {ex.Message}");
    }
    return null;
  }

  public static List<InventoryModel>? GetAllBooksByLowQuantity(int quantity)
  {
      try
      {
          var connection = Sql.I.GetConnection();
          if (connection == null) return null;

          const string query = "SELECT * FROM Inventory WHERE Quantity < @Quantity;";
          using var command = new MySqlCommand(query, connection);
          command.Parameters.AddWithValue("@Quantity", quantity);
          using var reader = command.ExecuteReader();

          var lowStockBooks = new List<InventoryModel>();
          while (reader.Read())
          {
              var inventoryItem = new InventoryModel
              {
                  ProductID = Convert.ToInt32(reader["ProductID"]),
                  Quantity = Convert.ToInt32(reader["Quantity"]),
              };
              lowStockBooks.Add(inventoryItem);
          }

          return lowStockBooks;
      }
      catch (Exception ex)
      {
          Console.WriteLine($"Error getting low stock books: {ex.Message}");
      }

      return null;
  }

  public static int GetQuantityForBook(int BookID){
    var cache = Redis.I.database;
    try{
      string? cachedQuantity = cache.StringGet($"book:{BookID}:quantity");
      if (!string.IsNullOrEmpty(cachedQuantity)){
        return Convert.ToInt32(cachedQuantity);
      }

      var connection = Sql.I.GetConnection();
      if (connection == null) return -2;

      const string query = "SELECT Quantity FROM Inventory WHERE ProductID = @BookID LIMIT 1;";
      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@BookID", BookID);

      using var reader = command.ExecuteReader();
      if (reader.Read()){
        int quantity = Convert.ToInt32(reader["Quantity"]);
        cache.StringSet($"book:{BookID}:quantity", quantity, TimeSpan.FromMinutes(60));
        return quantity;
      }
    }
    catch (Exception ex){
      Console.WriteLine($"Error retrieving quantity for book: {ex.Message}");
    }
    return -1;
  }
}
