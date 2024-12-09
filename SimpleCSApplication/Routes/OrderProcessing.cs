using SimpleCSApplication.Models;

namespace SimpleCSApplication.Routes;
public static class OrderProcessing{
  public static void Endpoints(this WebApplication app)
  {
    app.MapPost("/Order/new", (OrderModel model) =>
    {
      if(OrderModel.CreateOrder(model))
        return Results.Ok("Order created!");
      return Results.NotFound("Failed to create order");
    })
    .WithName("CreateNewOrder")
    .WithDescription("Create a new order")
    .WithOpenApi();


    app.MapGet("/order/{customerId:int}", (int customerId)=>{
      List<OrderModel> items = OrderModel.GetOrders(customerId);
      if(items == null)
        return Results.NotFound("No Orders found!");
      return Results.Ok(items);
    })
    .WithName("GetOrderByCostumerId")
    .WithDescription("Getting all orders for the customer")
    .WithOpenApi();
  }
}