# **Project Overview**

## Fast introduction:
For the database design, I have chosen to use MySQL as my relational database. The reasoning behind this choice is that I can create an `init.sql` file that can easily be executed when Docker starts up for the first time, making it easier to deploy in this type of project.

For the NoSQL database, I have picked MongoDB. The reasoning is that it’s what I know best, and the database is used for demonstration in the class.

For caching, I’ve chosen Redis due to its fast memory and ease of use for caching data.

## Data Storage

Most of the elements of the bookstore are stored in MySQL. This is because most of the items have a relationship to each other in one way or another.

### Index Planning

For index planning, I have chosen to index:

- **Books**: ISBN
- **Customers**: Email
- **Orders**: CustomerID
- **Inventory**: ProductID

**See** [Source](/db/bookstore.sql) line 47

## Data Selection

Based on my knowledge of what makes sense, this is what I would pick. I chose to store the bio of the author and book descriptions inside MongoDB. I made this choice because they are non-structured data that come in varying lengths and sizes, making MongoDB the perfect place for this type of data.

## Caching

When it comes to caching the data, I selected different get functions to use this feature. 

1. **Books by ISBN**: I am caching books by ISBN, as I know that educational books are often searched by ISBN, making it a good idea to cache.
2. **Books by Author**: When I read something, I most likely look for other books by the author, so this might be a good idea to cache. Although this is debatable, it’s included for now and can always be changed.
3. **Quantity of Books**: Another thing I wanted to cache is the quantity of a book. This makes a lot of sense for searching books in a bookstore, as the quantity is referenced often during the search. I have also set up TTD for this one.

# SourceFiles

## Mongo Db
* [MDB](/SimpleCSApplication/repo/MDb.cs)
* [BookModel](/SimpleCSApplication/Models/BookModel.cs)
* [AuthorModel](/SimpleCSApplication/Models/AuthorsModel.cs)

I used Mongo db to write and read text based information, in this case it's the book description.
````Csharp
while (reader.Read())
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
    books.Add(bookModel);
  }
````

````csharp
  var bioDocument = new BsonDocument
  {
      { "Bio", bio }
  };

  var document = new BsonDocument
  {
      { "Description", description }
  };
````
Right as I have been working on it, I had a hard time seeing the point in making the other in NoSQL. In my mind, I think the Customer, order are related to each other, they are rarely changed. 
On the logistics we have the inventory, orders, authors and books all are related to each other in one way or another. 
The only part, that I found that would make sense is the biography and descriptions. 

## Relational Database (MYSQL)
* [Sql](/db/bookstore.sql)
* [BookModel](/SimpleCSApplication/Models/BookModel.cs)
* [AuthorModel](/SimpleCSApplication/Models/AuthorsModel.cs)
* [CustomerModel](/SimpleCSApplication/Models/CustomerModel.cs)
* [InventoryModel](/SimpleCSApplication/Models/InventoryModel.cs)
* [OrderModel](/SimpleCSApplication/Models/OrderModel.cs)

Not much to comment on this, the links show where SQL is used, including the script.


## Caching
* [Redis](/SimpleCSApplication/Controllers/redis.cs)
* [BookModel](/SimpleCSApplication/Models/BookModel.cs)
* [InventoryModel](/SimpleCSApplication/Models/InventoryModel.cs)
* [OrderModel](/SimpleCSApplication/Models/OrderModel.cs)



Inside the model classes you will find me use the Redis for caching, and I would use it like this
````csharp
//- deserialize the books if found
string cacheKey = $"author:{model.Id}:books";
string? cachedBooks = cache.StringGet(cacheKey);
if (!string.IsNullOrEmpty(cachedBooks))
{
    return JsonSerializer.Deserialize<List<BookModel>>(cachedBooks);
}

//- Serialize the books
if (books.Count > 0)
{
    string booksJson = JsonSerializer.Serialize(books);
    cache.StringSet(cacheKey, booksJson, TimeSpan.FromMinutes(45));
}
````

While developing the system, I tried to think where caching would make the most sense to implement.
First, I wanted to cache the books and the ISBN for that Is used in many scenarios when searching for a book on the same lane I chose to cache Books related to the author. 

But in all cases, I would monitor the user behavior and see what part of the data that is used the most, and cache it. I have been thinking that creating something more dynamic could be useful, for user behaviors could change, and if the cache-system automatically could analyze the behavior, and cache accordantly, I would make the system more robust. 

And the other is caching quantity on a book, for this would definitely be one that are used, every time the book is looked at. 
The last thing I picked at this time is the order. It will make sense if you have a live processing system the user can follow the order, but right now the bookstore doesn’t have those features, so I don’t see the point in it. But there you go!
