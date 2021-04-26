using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.Models;

namespace WebAPI.Repository
{
  public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
  {
    public CategoryRepository(LibraryDBContext context) 
      : base(context) 
    { 
    }

    public IEnumerable<Category> GetAllCategories()
    {
      return FindAll()
        .OrderBy(c => c.Id)
        .ToList();
    }

    public Category GetCategoryById(int categoryId)
    {
      return FindByCondition(category => category.Id.Equals(categoryId))
        .FirstOrDefault();
    }

    public Category GetCategoryWithDetails(int categoryId)
    {
      return FindByCondition(category => category.Id.Equals(categoryId))
        .Include(b => b.Books)
        .FirstOrDefault();
    }
  }
  public interface ICategoryRepository : IRepositoryBase<Category>
  {
    IEnumerable<Category> GetAllCategories();
    Category GetCategoryById(int categoryId );
    Category GetCategoryWithDetails(int categoryId);
  }
}
