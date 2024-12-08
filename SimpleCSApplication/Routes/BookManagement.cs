using SimpleCSApplication.Models;

namespace SimpleCSApplication.Routes;
public static class BookManagement{
  public static void Endpoints(this WebApplication app)
  {
    // Get all books by a specific author
    app.MapGet("/books/author/{author}", (string author) =>
    {
      return Results.Ok("Hello!");
    })
    .WithName("GetBooksByAuthor")
    .WithDescription("Getting all books by a specific author")
    .WithOpenApi();

    // Get details of a book by ID
    app.MapGet("/books/{id:int}", (int id) =>
    {
      return Results.Ok("Hello!");
    })
    .WithName("GetBookDetails")
    .WithDescription("Get book details by book id")
    .WithOpenApi();

    app.MapPost("/books/create/author", (AuthorModel model)=>{
      if(AuthorModel.CreateAuthor(model)){
        return Results.Ok();
      } else return Results.Problem();
    })
    .WithDescription("Create author")
    .WithName("CreateAuthor")
    .WithOpenApi();


    app.MapGet("/books/get/all/authors", ()=>{
      List<AuthorModel>? models = AuthorModel.GetAllAuthors();
      if (models == null || models.Count == 0)
      {
          return Results.NotFound("No authors found.");
      }
      return Results.Ok(models);
    })
    .WithName("GetAllAuthors")
    .WithDescription("Returning all authors in the database")
    .WithOpenApi();

  }
}