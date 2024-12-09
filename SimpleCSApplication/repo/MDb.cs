using MongoDB.Bson;
using MongoDB.Driver;
using SimpleCSApplication.Controllers;

namespace SimpleCSApplication.Repo;
public class MDb{
  public static string? InsertBio(string bio){
    var bioCollection = MongoDb.I.GetCollection("Bios");
    var bioDocument = new BsonDocument
    {
        { "Bio", bio }
    };

    bioCollection.InsertOne(bioDocument);
    return bioDocument["_id"].ToString();
  }
  public static string? InsertDescription(string description){
    var collection = MongoDb.I.GetCollection("Descriptions");
    var document = new BsonDocument
    {
        { "Description", description }
    };
    collection.InsertOne(document);
    return document["_id"].ToString();
  }
  public static string? GetBioFromMongo(string bioId)
  {
      var mongoDb = MongoDb.I;
      var collection = mongoDb.GetCollection("Bios");
      var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(bioId));
      var document = collection.Find(filter).FirstOrDefault();
      return document?["Bio"]?.ToString();
  }
  public static string? GetDescriptionFromMongo(string bioId)
  {
      var mongoDb = MongoDb.I;
      var collection = mongoDb.GetCollection("Descriptions");
      var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(bioId));
      var document = collection.Find(filter).FirstOrDefault();
      return document?["Description"]?.ToString();
  }
}