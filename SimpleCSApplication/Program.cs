using MongoDB.Bson;
using MongoDB.Driver;
using SimpleCSApplication.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

BookManagement.Endpoints(app);
CustomerManagement.Endpoints(app);
InventoryManagement.Endpoints(app);
OrderProcessing.Endpoints(app);

app.Run();

IMongoDatabase database;
IMongoCollection<BsonDocument> collection;


// Connect to MongoDB (local instance or cloud)
var client = new MongoClient("mongodb://root:rootpassword@localhost:27017"); // Change the connection string accordingly
database = client.GetDatabase("Bookstore"); // Replace with your database name
collection = database.GetCollection<BsonDocument>("Books"); // Replace with your collection name

// Create a new document to insert
var document = new BsonDocument
{
    { "name", "John Doe" },
    { "age", 30 },
    { "email", "johndoe@example.com" }
};

// Insert the document into the collection
collection.InsertOne(document);
Console.WriteLine("Document inserted!");