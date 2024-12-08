namespace SimpleCSApplication.Routes;
public static class CustomerManagement{
  public static void Endpoints(this WebApplication app)
  {
    app.MapGet("/Customer/create/{username}/{passHash}", (string username, string passHash) =>
    {

    })
    .WithName("CustomerCreate")
    .WithDescription("Create a new customer")
    .WithOpenApi();

  }
}