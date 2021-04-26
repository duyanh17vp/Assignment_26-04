using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.Models;

namespace WebAPI.Repository
{
  public class BookRepository : RepositoryBase<Book>, IBookRepository
  {
    public BookRepository(LibraryDBContext context) 
      : base(context) 
    { 
    }

    public IEnumerable<Book> GetAllBooks()
    {
      return FindAll()
        .OrderBy(b => b.Id)
        .ToList();
    }

    public Book GetBookById(int bookId)
    {
      return FindByCondition(book => book.Id.Equals(bookId))
        .FirstOrDefault();
    }

    public Book GetBookWithDetails(int bookId)
    {
      return FindByCondition(book => book.Id.Equals(bookId))
        .Include(ro => ro.RequestOrders)
        .FirstOrDefault();
    }

  }
  public interface IBookRepository : IRepositoryBase<Book>
  {
    IEnumerable<Book> GetAllBooks();
    Book GetBookById(int bookId );
    Book GetBookWithDetails(int bookId);

  }
}
