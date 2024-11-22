# Documentation
>[!NOTE]
> Assignment Documentation and information



# 1. Getting Started
|Technology Stack Recommendation
|Component	                  Database Type	      Example Database	                Rationale
|-----------------------------------------------------------------------------------------------------------------------------------------
|Books and Authors	          |Relational	        |PostgreSQL	                      |Structured schema, advanced querying.
|Customers and Orders	        |Relational	        |MySQL	                            |ACID compliance for order integrity.
|Inventory Management	        |NoSQL	              |Redis	                            |Low-latency, high write performance.



|Component	                  |Cache Type	        |What to Cache	                    |Invalidation  Strategy
|-----------------------------------------------------------------------------------------------------------------------------------------
|Books and Authors	          |Redis/Memcached	    |Book metadata,                    |search results	On update, or time-based expiration
|Customers and Orders	        |Redis	              |Sessions, recent orders	          |TTL for sessions, update on new order
|Inventory Management	        |Redis	              |Stock levels for reads	          |Immediate invalidation on update
|Recommendations	            |Redis	              |Recommendations, search results	  |Periodic refresh or TTL
|Homepage Content	            |Redis/CDN	          |Static data, banners	            |On content change or periodic refresh



## Overview
You will work to design and implement an online bookstore application using a relational database and / or a NoSQL database.
Your application should handle data related to books, authors, customers, and orders efficiently. You will make informed design
choices about which database technologies to use for different parts of the system and how to handle data caching when appropriate.

# Scenario
You are building an online bookstore that includes:

- Books and Authors: Data about available books and their authors.
- Customers and Orders: Customer information and order processing.
- Inventory Management: Updates to stock levels when orders are placed.
- Different parts of your application may have varying requirements for reading and writing data.

For example, book details might be read frequently but updated rarely, whereas inventory levels and orders may need rapid updates. You will decide which database types (relational or NoSQL) are best suited for these different aspects.


# Tasks
## Database Design:

1. Relational Database: 
- Create a normalized relational database schema for core transactional data (e.g., orders, customers).
2. NoSQL Database:
- Decide which parts of the data (such as frequently accessed book details or user session data) could be stored in a NoSQL key-value store (e.g., Redis).
3. Caching Strategy:
- Identify if and where caching can improve performance in data retrieval (for example, caching book details that don’t change often).

# (Minimal) Implementation in C#:
Develop a C# application that:
Connects to and interacts with both your relational database and NoSQL data store (e.g. MySQL, PostgreSQL, Redis or MongoDB).
Handles essential operations such as:
Displaying books and authors.
Processing new orders (writing to the relational database).
Updating inventory levels.
Retrieving cached data when appropriate.
Project Setup and Delivery:

# Documentation
**Use a GitHub repository for your project**.
Ensure each team member's contributions are visible through commit history.

**Include a README file that:**
- Briefly explains your design choices for using the relational database, NoSQL store, and caching.
- Provides instructions on how to set up and run the application.