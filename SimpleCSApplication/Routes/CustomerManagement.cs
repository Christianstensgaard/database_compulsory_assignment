using SimpleCSApplication.Models;

namespace SimpleCSApplication.Routes;
public static class CustomerManagement{
  public static void Endpoints(this WebApplication app)
  {
    app.MapPost("/customer/create", (CustomerModel model) =>
    {
      if(CustomerModel.CreateCustomer(model))
        return Results.Ok("Customer Created");
      return Results.NotFound("Somethings went wrong!");
    })
    .WithName("CustomerCreate")
    .WithDescription("Create a new customer")
    .WithOpenApi();
  }
}