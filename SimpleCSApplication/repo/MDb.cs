using MongoDB.Bson;
using MongoDB.Driver;
using SimpleCSApplication.Controllers;

namespace SimpleCSApplication.Repo;
public class MDb{
  public static string? GetBioFromMongo(string bioId)
  {
      var mongoDb = MongoDb.I;
      var collection = mongoDb.GetCollection("Bios"); // Assuming you store Bios in a 'Bios' collection
      var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(bioId));
      var document = collection.Find(filter).FirstOrDefault();
      return document?["Bio"]?.ToString(); // Return the Bio if found, else null
  }
}