using MongoDB.Bson;
using MongoDB.Driver;
using MySql.Data.MySqlClient;
using SimpleCSApplication.Controllers;
using SimpleCSApplication.Repo;

namespace SimpleCSApplication.Models
{
  public class AuthorModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    private string BioId {get; set;}


  // Displaying books and authors
    public static AuthorModel? GetAuthor(string name)
{
    var connection = Sql.I.GetConnection();
    if (connection == null) return null;

    try
    {
        // Step 1: Get Author from MySQL
        const string query = "SELECT * FROM Authors WHERE Name = @Name LIMIT 1;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", name);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var authorModel = new AuthorModel
            {
                Id = Convert.ToInt32(reader["AuthorID"]),
                Name = reader["Name"].ToString(),
                BioId = reader["BioID"].ToString() // Assume this is the MongoDB ObjectId stored as string
            };

            if (!string.IsNullOrEmpty(authorModel.BioId))
            {
                var bioCollection = MongoDb.I.GetCollection("Bios"); // Assume Bios collection for Bio documents

                var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(authorModel.BioId)); // MongoDB ObjectId
                var bioDocument = bioCollection.Find(filter).FirstOrDefault();

                if (bioDocument != null)
                {
                    authorModel.Bio = bioDocument["Bio"].ToString(); // Get Bio text from MongoDB
                }
            }
            return authorModel;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving author: {ex.Message}");
    }

    return null;
}
    public static AuthorModel? GetAuthor(int id)
    {
      var connection = Sql.I.GetConnection();
      if (connection == null) return null;

      try
      {
        const string query = "SELECT * FROM Authors WHERE AuthorID = @id LIMIT 1;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
          return new AuthorModel
          {
            Id = Convert.ToInt32(reader["AuthorID"]),
            Name = reader["Name"].ToString(),
            Bio = reader["Bio"].ToString()
          };
        }
      }
      catch (Exception ex)
      {
          Console.WriteLine($"Error retrieving author: {ex.Message}");
      }

      return null;
    }
    public static List<AuthorModel>? GetAllAuthors()
    {
        var connection = Sql.I.GetConnection();
        if (connection == null) return null;

        var authors = new List<AuthorModel>();

        try
        {
            const string query = "SELECT * FROM Authors;";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var bioId = reader["BioID"].ToString();
                var bio = MDb.GetBioFromMongo(bioId);
                authors.Add(new AuthorModel
                {
                    Id = Convert.ToInt32(reader["AuthorID"]),
                    Name = reader["Name"].ToString(),
                    Bio = bio // Setting Bio from MongoDB
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving authors: {ex.Message}");
        }
        return authors;
    }

    public static bool CreateAuthor(AuthorModel model)
    {
      var connection = Sql.I.GetConnection();
      if (connection == null) return false;

      try
      {
          var bioCollection = MongoDb.I.GetCollection("Bios");
          
          var bioDocument = new BsonDocument
          {
              { "Bio", model.Bio }
          };
          
          bioCollection.InsertOne(bioDocument);
          var bioId = bioDocument["_id"].ToString();
          const string query = "INSERT INTO Authors (Name, BioID) VALUES (@Name, @BioID);";
          using var command = new MySqlCommand(query, connection);
          command.Parameters.AddWithValue("@Name", model.Name);
          command.Parameters.AddWithValue("@BioID", bioId); // Store the BioID (MongoDB ObjectId)
          int rowsAffected = command.ExecuteNonQuery();
          Console.WriteLine($"{rowsAffected} author(s) added.");
          return true;
      }
      catch (Exception ex)
      {
          Console.WriteLine($"Error creating author: {ex.Message}");
          return false;
      }
    }
  }
}
