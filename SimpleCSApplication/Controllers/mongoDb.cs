using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleCSApplication.Controllers;
public class MongoDb{
  public static MongoDb I {get; private set;} = new MongoDb();

  public IMongoCollection<BsonDocument> GetCollection(string name){
    return database.GetCollection<BsonDocument>(name);
  }

  public bool Connected(){
    try
    {
        // Try to list databases to check connection
        var databases = database.Client.ListDatabases().ToList();
        return true; // Connection is fine
    }
    catch (Exception ex)
    {
        // Handle exceptions, like network issues or incorrect credentials
        return false; // Connection failed
    }
  }

  MongoDb(){
    var client = new MongoClient(ConnectionString);
    database = client.GetDatabase(Database_name);
  }
  readonly IMongoDatabase database;
  const string Database_name = "Bookstore";
  const string ConnectionString = "mongodb://root:rootpassword@localhost:27017";
}