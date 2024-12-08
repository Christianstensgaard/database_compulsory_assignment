namespace SimpleCSApplication.Models;
public class Inventory{
  public int ItemID { get; set; }
  public int ProductID { get; set; }
  public int Quantity { get; set; } = 0;

  public BookModel Book { get; set; }
}
