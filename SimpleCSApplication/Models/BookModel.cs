namespace SimpleCSApplication.Models;
public class BookModel{
  public int Id { get; set; }
  public string Title { get; set; }
  public int AuthorId { get; set; }
  public string ISBN { get; set; }

  // Displaying books and authors

  public static BookModel? GetBookByISBN(string ISBN){
    

    return null;
  }

  public static List<BookModel>? GetBooksByAuthor(AuthorModel model){
    

    return null;
  }

  public static void CreateBook(BookModel model){

  }

}