using SimpleCSApplication.Models;

namespace SimpleCSApplication.Routes;
public static class InventoryManagement{
  public static void Endpoints(this WebApplication app)
  {
    // Get all books by a specific author
    app.MapPost("/inventory/update/{bookId:int}/{addAmount:int}", (int bookId, int addAmount) =>
    {
      if(InventoryModel.UpdateInventoryQuantity(bookId, addAmount))
        return Results.Ok("Stock updated");
      return Results.NotFound("Inventory could not be found");
    })
    .WithName("InventoryUpdate")
    .WithOpenApi();


    app.MapGet("/inventory/all", ()=>{

      var items = InventoryModel.GetInventory();
      return Results.Ok(items);
    })
    .WithName("GetAllInInventory")
    .WithOpenApi();

    app.MapGet("/inventory/quantity/{bookId:int}", (int bookId)=>{
      var quantity = InventoryModel.GetQuantityForBook(bookId);
      return Results.Ok(quantity);
    })
    .WithName("GetQuantityForBook")
    .WithOpenApi();

  }
}