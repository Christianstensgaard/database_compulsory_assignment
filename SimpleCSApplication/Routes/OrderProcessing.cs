namespace SimpleCSApplication.Routes;
public static class OrderProcessing{
  public static void Endpoints(this WebApplication app)
  {
    // Get all books by a specific author
    app.MapGet("/Order/new/{author}", (string author) =>
    {


    })
    .WithName("OrderNew")
    .WithOpenApi();
  }
}