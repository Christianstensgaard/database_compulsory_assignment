# **Project Overview**

The goal of this project is to design and implement an online bookstore application that handles data related to books, authors, customers, and orders efficiently. The application will utilize a relational database (SQL Server) and a NoSQL database (MongoDB) to meet the varying requirements of different parts of the system.

## **Database Design**

The database design will consist of two main components:

1. **Relational Database**: SQL Server will be used as the relational database, with tables for books, authors, customers, orders, and inventory management.
2. **NoSQL Database**: MongoDB will be used as the NoSQL database, with collections for book details, customer information, and order processing.

**Database Schema**

Here are the database schema designs for each component:

**Relational Database**

Books and Authors will be stored inside a relational database, this i chose because the data rarely changed and
the data can be structured easily for query, Also here, we can easy say, that it would make sense to index the ISBN
that, when searching the ISBN is used in many scenarios.

Authors would also be stored in this type of database, since the data would rarely changes. in this table, i'm a bit unsure, if
any indexing of the data should be, so i will leave  it for new, a more complex analysis of the query executions could ensure if a
indexing of this should be added.
```sql
  CREATE TABLE Books (
    BookID INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(255) NOT NULL,
    AuthorID INT,
    ISBN NVARCHAR(20) UNIQUE,
  );

  CREATE INDEX idx_Books_Isbn ON Books (ISBN);

  CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(100) NOT NULL,
    BioID CHAR(24) -- mongoDb object_id
  );
```

moving on to the customer and orders, i chose to add **Customers**, **Orders** to the relational database as well. the reasoning is
the same as the books and authors, the data structure and the data is rarely changed.
thinking on what could be indexed, the first thing i would reason are the email, that it is common used in this type of scenarios.

moving on to orders, the most common scenarios would be usage of the Costumer id for searching orders. so indexing can be created for that one as well.

```sql
  CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- thinking maybe if long time has gone by, ask if information i correct
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
  );

  CREATE UNIQUE INDEX idx_Customers_Email ON Customers (Email);

  CREATE TABLE Orders (
  OrderID INT PRIMARY KEY AUTO_INCREMENT,
  CustomerID INT NOT NULL,
  BookID INT NOT NULL,
  Quantity INT NOT NULL DEFAULT 1,
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
  FOREIGN KEY (BookID) REFERENCES Books(BookID)

  CREATE INDEX idx_Orders_CustomerID ON Orders (CustomerID);
);
```

The last thing i would add to the relational database is Inventory.

````sql
  CREATE TABLE Inventory (
    ItemID INT PRIMARY KEY AUTO_INCREMENT,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    FOREIGN KEY (ProductID) REFERENCES Books(BookID)
  );

  CREATE INDEX idx_Inventory_ProductID ON Inventory (ProductID);
````

i have chosen to make all these inside the Relational database, for the relations between the tables, and the rarely changing data 
context of the elements.

(note) - Think i'm moving the order to Mongo for the processing could change multiple times.





---
**NoSQL (MongoDB)**

```json

// Customer Information Collection
{
  "_id": ObjectId,
  "Name": String,
  "Email": String,
  "PasswordHash": String
}

// Order Processing Collection
{
  "_id": ObjectId,
  "OrderID": Integer,
  "CustomerID": ObjectId,
  "BookID": ObjectId,
  "Quantity": Integer,
  "Status": String
}
```

## **Caching Strategies**

To implement caching strategies, we will use Redis as the in-memory data store. We will cache frequently accessed book details and customer information to improve application performance.

## **C# Application Implementation**

The C# application will be built using ASP.NET Core Web API framework. The API endpoints will include:

* Book management (e.g., get books by author, get book details)
* Customer management (e.g., register customers, login customers)
* Order processing (e.g., place orders, cancel orders)
* Inventory management (e.g., update inventory levels)

## **Docker Containerization**
# **Conclusion**
