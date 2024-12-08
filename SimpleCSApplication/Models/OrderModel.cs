namespace SimpleCSApplication.Models;
public class OrderModel
{
  public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public int BookID { get; set; }
    public int Quantity { get; set; } = 1;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public CustomerModel Customer { get; set; }
    public BookModel Book { get; set; }



  // Processing new orders (writing to the relational database).
}