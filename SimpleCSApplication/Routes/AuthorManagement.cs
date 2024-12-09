using SimpleCSApplication.Models;

namespace SimpleCSApplication.Routes;
public static class AuthorManagement{
  public static void Endpoints(this WebApplication app)
  {

    app.MapPost("/author/create", (AuthorModel model)=>{
      if(AuthorModel.CreateAuthor(model)){
        return Results.Ok();
      } else return Results.Problem();
    })
    .WithDescription("Create author")
    .WithName("CreateAuthor")
    .WithOpenApi();

    app.MapGet("/author/all", ()=>{
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