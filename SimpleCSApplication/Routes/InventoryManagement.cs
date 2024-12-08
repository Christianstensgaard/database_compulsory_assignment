namespace SimpleCSApplication.Routes;
public static class InventoryManagement{
  public static void Endpoints(this WebApplication app)
  {
    // Get all books by a specific author
    app.MapGet("/inventory/update/{author}", (string author) =>
    {
    })
    .WithName("InventoryUpdate")
    .WithOpenApi();

  }
}