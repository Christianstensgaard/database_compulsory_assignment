CREATE DATABASE Bookstore;
USE Bookstore;
-- tables
CREATE TABLE Books (
  BookID INT PRIMARY KEY AUTO_INCREMENT,
  Title NVARCHAR(255) NOT NULL,
  AuthorID INT,
  DescriptionId  CHAR(24), -- Mongo object_id (Book description)
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
  Paid TINYINT NOT NULL DEFAULT 0,
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


-- Adding Stored Procedure for Getting orders by id
-- Selecting order information, customer information and book information. (book could maybe be make less?)
-- From orders, Joining Customers on ID and Book on id
-- Where input id (customerId) = CustomerId
-- o: Order, c: Customer, b:Book
DELIMITER //
CREATE PROCEDURE GetOrdersByCustomerId (
    IN customerId INT
)
BEGIN
    SELECT
        o.OrderID,
        o.CustomerID,
        o.BookID,
        o.Quantity,
        o.Paid,
        o.CreatedAt,
        o.UpdatedAt,
        c.Name AS CustomerName,
        c.Email AS CustomerEmail,
        b.Title AS BookTitle,
        b.AuthorID AS BookAuthorID
    FROM Orders o
    JOIN Customers c ON o.CustomerID = c.CustomerID
    JOIN Books b ON o.BookID = b.BookID
    WHERE o.CustomerID = customerId;
END //
DELIMITER ;


INSERT INTO Authors (Name, BioID)
VALUES
('J.K. Rowling', '507f1f77bcf86cd799439011'),   -- Random MongoDB
('George R.R. Martin', '507f1f77bcf86cd799439012'),
('J.R.R. Tolkien', '507f1f77bcf86cd799439013');

INSERT INTO Books (Title, AuthorID, DescriptionId, ISBN)
VALUES
('Harry Potter and the Philosophers Stone', 1, '507f1f77bcf86cd799439011', '9780747532699'),
('A Game of Thrones', 2, '507f1f77bcf86cd799439012', '9780553103540'),
('The Hobbit', 3, '507f1f77bcf86cd799439013', '9780618968633');

INSERT INTO Customers (Name, Email, PasswordHash)
VALUES
('Janni', 'Janni@Janni.com', 'passwordhash1'),
('Bob', 'bob@Bob.com', 'passwordhash2'),
('Jens', 'Jens@Jens.com', 'passwordhash3');

INSERT INTO Orders (CustomerID, BookID, Quantity, Paid)
VALUES
(1, 1, 1, 1),
(1, 2, 2, 0),
(2, 1, 1, 1),
(3, 3, 1, 0);

-- Insert inventory
INSERT INTO Inventory (ProductID, Quantity)
VALUES
(1, 50),  -- 50 copies of "Harry Potter and the Philosopher's Stone" available
(2, 30),  -- 30 copies of "A Game of Thrones" available
(3, 20);  -- 20 copies of "The Hobbit" available