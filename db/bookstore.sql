CREATE DATABASE Bookstore;
USE Bookstore;
-- tables
CREATE TABLE Books (
  BookID INT PRIMARY KEY AUTO_INCREMENT,
  Title NVARCHAR(255) NOT NULL,
  AuthorID INT,
  ISBN NVARCHAR(20) UNIQUE
);

CREATE TABLE Authors (
  AuthorID INT PRIMARY KEY AUTO_INCREMENT,
  Name NVARCHAR(100) UNIQUE,
  BioID CHAR(24) -- < text moved to MongoDb
);

CREATE TABLE Customers (
  CustomerID INT PRIMARY KEY AUTO_INCREMENT,
  Name NVARCHAR(100) NOT NULL,
  Email NVARCHAR(100) UNIQUE NOT NULL,
  PasswordHash NVARCHAR(255) NOT NULL,
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- thinking maybe if long time has gone by, ask if information i correct
  UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE Orders (
  OrderID INT PRIMARY KEY AUTO_INCREMENT,
  CustomerID INT NOT NULL,
  BookID INT NOT NULL,
  Quantity INT NOT NULL DEFAULT 1,
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
  FOREIGN KEY (BookID) REFERENCES Books(BookID)
);

CREATE TABLE Inventory (
  ItemID INT PRIMARY KEY AUTO_INCREMENT,
  ProductID INT NOT NULL,
  Quantity INT NOT NULL DEFAULT 0,
  FOREIGN KEY (ProductID) REFERENCES Books(BookID)
);

-- Adding indexing
CREATE INDEX idx_Books_Isbn ON Books (ISBN);                        -- ISBN is the most useful for indexing the books, since it used a lot.
CREATE UNIQUE INDEX idx_Customers_Email ON Customers (Email);       -- Looking up customer is mail used in most scenarios
CREATE INDEX idx_Orders_CustomerID ON Orders (CustomerID);          -- Customer ID, Would i say is most useful here
CREATE INDEX idx_Inventory_ProductID ON Inventory (ProductID);      -- 
