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
      var databases = database.Client.ListDatabases().ToList();
      return true;
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  MongoDb(){
    var client = new MongoClient(ConnectionString);
    database = client.GetDatabase(Database_name);
  }
  readonly IMongoDatabase database;
  const string Database_name = "Bookstore";
  const string ConnectionString = "mongodb://root:rootpassword@mongo:27017";
}