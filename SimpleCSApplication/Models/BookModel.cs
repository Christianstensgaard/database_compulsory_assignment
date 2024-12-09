using System.Text.Json;
using MySql.Data.MySqlClient;
using SimpleCSApplication.Controllers;
using SimpleCSApplication.Repo;

namespace SimpleCSApplication.Models;
public class BookModel{
  public int Id { get; set; }
  public string Title { get; set; }
  public int AuthorId { get; set; }
  public string Description { get; set; }
  private string DescriptionId { get; set; }
  public string ISBN { get; set; }


  public static BookModel? GetBookByISBN(string ISBN){
    var cache = Redis.I.database;
    try
    {
      string? cachedBook = cache.StringGet($"book:{ISBN}");
      if (!string.IsNullOrEmpty(cachedBook))
      {
        return JsonSerializer.Deserialize<BookModel>(cachedBook);
      }

      var connection = Sql.I.GetConnection();
      if (connection == null) return null;

      const string query = "SELECT * FROM Books WHERE ISBN = @ISBN LIMIT 1;";
      using var command = new MySqlCommand(query, connection);
      command.Parameters.AddWithValue("@ISBN", ISBN);

      using var reader = command.ExecuteReader();
      if (reader.Read())
      {
        var bookModel = new BookModel
        {
          Id = Convert.ToInt32(reader["BookID"]),
          Title = reader["Title"].ToString(),
          AuthorId = Convert.ToInt32(reader["AuthorID"]),
          DescriptionId = reader["DescriptionId"].ToString(),
          ISBN = reader["ISBN"].ToString()
        };

        bookModel.Description = MDb.GetDescriptionFromMongo(bookModel.DescriptionId);

        string bookJson = JsonSerializer.Serialize(bookModel);
        cache.StringSet($"book:{ISBN}", bookJson, TimeSpan.FromMinutes(30));

        return bookModel;
      }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving book: {ex.Message}");
    }

    return null;
  }
  public static List<BookModel>? GetBooksByAuthor(AuthorModel model){
    if (model.Id != -1 && model.Id > 0){
      var cache = Redis.I.database;
      try{
        string cacheKey = $"author:{model.Id}:books";

        string? cachedBooks = cache.StringGet(cacheKey);
        if (!string.IsNullOrEmpty(cachedBooks)){
            return JsonSerializer.Deserialize<List<BookModel>>(cachedBooks);
        }

        var connection = Sql.I.GetConnection();
        if (connection == null) return null;

        const string query = "SELECT * FROM Books WHERE AuthorID = @AuthorID;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@AuthorID", model.Id);

        var books = new List<BookModel>();
        using var reader = command.ExecuteReader();
        while (reader.Read()){
          var bookModel = new BookModel
          {
              Id = Convert.ToInt32(reader["BookID"]),
              Title = reader["Title"].ToString(),
              AuthorId = Convert.ToInt32(reader["AuthorID"]),
              DescriptionId = reader["DescriptionId"].ToString(),
              ISBN = reader["ISBN"].ToString()
          };

          bookModel.Description = MDb.GetDescriptionFromMongo(bookModel.DescriptionId);

          books.Add(bookModel);
        }

        if (books.Count > 0){
          string booksJson = JsonSerializer.Serialize(books);
          cache.StringSet(cacheKey, booksJson, TimeSpan.FromMinutes(45));
        }

        return books;
      }
      catch (Exception ex){
        Console.WriteLine($"Error retrieving books by author: {ex.Message}");
      }
    }

    return null;
  }
  public static List<BookModel> GetAllBooks(){
    var connection = Sql.I.GetConnection();
    if (connection == null) return new List<BookModel>();

    var books = new List<BookModel>();

    try{
      const string query = "SELECT * FROM Books;";
      using var command = new MySqlCommand(query, connection);

      using var reader = command.ExecuteReader();
      while (reader.Read()){
        var bookModel = new BookModel
        {
          Id = Convert.ToInt32(reader["BookID"]),
          Title = reader["Title"].ToString(),
          AuthorId = Convert.ToInt32(reader["AuthorID"]),
          DescriptionId = reader["DescriptionId"].ToString(),
          ISBN = reader["ISBN"].ToString()
        };

        bookModel.Description = MDb.GetDescriptionFromMongo(bookModel.DescriptionId);
        books.Add(bookModel);
      }
    }
    catch (Exception ex){
        Console.WriteLine($"Error retrieving books: {ex.Message}");
    }

    return books;
  }
  public static bool CreateBook(BookModel model, int quantity){
    var connection = Sql.I.GetConnection();
    if (connection == null) return false;

    try{
      var description_id = MDb.InsertDescription(model.Description);

      const string queryBook = "INSERT INTO Books (Title, AuthorID, DescriptionId, ISBN) VALUES (@Title, @AuthorID, @DescriptionId, @ISBN);";
      using var commandBook = new MySqlCommand(queryBook, connection);
      commandBook.Parameters.AddWithValue("@Title", model.Title);
      commandBook.Parameters.AddWithValue("@AuthorID", model.AuthorId);
      commandBook.Parameters.AddWithValue("@DescriptionId", description_id);
      commandBook.Parameters.AddWithValue("@ISBN", model.ISBN);
      int rowsAffected = commandBook.ExecuteNonQuery();

      if (rowsAffected > 0){
        const string queryInventory = "INSERT INTO Inventory (ProductID, Quantity) VALUES (LAST_INSERT_ID(), @Quantity);";
        using var commandInventory = new MySqlCommand(queryInventory, connection);
        commandInventory.Parameters.AddWithValue("@Quantity", quantity);
        commandInventory.ExecuteNonQuery();
      }

      return true;
    }
    catch (Exception ex){
      Console.WriteLine($"Error creating book: {ex.Message}");
      return false;
    }
  }
}