using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.Models;
using WebAPI.Extensions;

namespace WebAPI.Repository
{
  public class UserRepository : RepositoryBase<User>, IUserRepository
  {
    public UserRepository(LibraryDBContext context) 
      : base(context) 
    { 
    }

    public IEnumerable<User> GetAllUsers()
    {
      List<User> _users = FindAll()
        .OrderBy(u => u.Id)
        .ToList();
      return _users.WithoutPasswords();
    }

    public User GetUserById(int userId)
    {
      User _user = FindByCondition(user => user.Id.Equals(userId))
        .FirstOrDefault();
      return _user.WithoutPassword();
    }

    public User GetUserWithDetails(int userId)
    {
      User _user = FindByCondition(user => user.Id.Equals(userId))
        .Include(ro => ro.RequestOrders)
        .FirstOrDefault();
        return _user.WithoutPassword();
    }
  }
  public interface IUserRepository : IRepositoryBase<User>
  {
    IEnumerable<User> GetAllUsers();
    User GetUserById(int userId );
    User GetUserWithDetails(int userId);

  }
}
