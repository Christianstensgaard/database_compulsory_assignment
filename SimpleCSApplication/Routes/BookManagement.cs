using SimpleCSApplication.Models;

namespace SimpleCSApplication.Routes;
public static class BookManagement{
  public static void Endpoints(this WebApplication app)
  {
    //- Create new book
    app.MapPost("/books/create/{quantity:int}", (BookModel model, int quantity) =>{
      if(!BookModel.CreateBook(model, quantity))
        return Results.NotFound("Failed to create book");
      return Results.Ok("Book created");
    })
    .WithDescription("Creates a new book, and store the information in Mysql, and Description in MongoDb")
    .WithName("CreateBook")
    .WithOpenApi();

    app.MapGet("/books/all", ()=>{

      var books = BookModel.GetAllBooks();
      if(books.Count > 0)
        return Results.Ok(books);
      else return Results.NotFound("Could not find any books!");
    })
    .WithName("GetAllBooks")
    .WithDescription("Getting all books")
    .WithOpenApi();


    app.MapGet("/books/isbn/{ISBN}", (string ISBN)=>{
      return Results.Ok(BookModel.GetBookByISBN(ISBN));
    })
    .WithDescription("Get book with ISBN")
    .WithName("GetBookWithISBN")
    .WithOpenApi();


    app.MapGet("/books/author/{author}", (string author)=>{
      AuthorModel model = AuthorModel.GetAuthor(author);
      if(model == null)
        return Results.NotFound("Failed to find author");
      return Results.Ok(BookModel.GetBooksByAuthor(model));
    })
    .WithDescription("Getting all Books by a given author")
    .WithName("GetBooksByAuthors")
    .WithOpenApi();
  }
}