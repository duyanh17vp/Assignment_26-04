using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.Models;

namespace WebAPI.Repository
{
  public class RequestOrderRepository : RepositoryBase<RequestOrder>, IRequestOrderRepository
  {
    public RequestOrderRepository(LibraryDBContext context) 
      : base(context) 
    { 
    }

    public IEnumerable<RequestOrder> GetAllRequestOrders()
    {
      return FindAll()
        .OrderBy(ro => ro.Id)
        .ToList();
    }

    public RequestOrder GetRequestOrderById(int requestOrderId)
    {
      return FindByCondition(requestOrder => requestOrder.Id.Equals(requestOrderId))
        .FirstOrDefault();
    }

    public RequestOrder GetRequestOrderWithDetails(int requestOrderId)
    {
      return FindByCondition(requestOrder => requestOrder.Id.Equals(requestOrderId))
        .Include(b => b.Books)
        .FirstOrDefault();
    }
    
    public RequestOrder CreateRequest(int userId, List<int> booksId)
    {
      var request = new RequestOrder { Status = Status.waiting, NormalUserId = userId,
                                       DateRequest = DateTime.Now, DateReturn = DateTime.Now.AddMonths(6) };
      _context.RequestOrders.Add(request);
      _context.SaveChanges();
      return request;
    }

    public void AddListRequestDetails(int requestId,List<int> booksId)
    {
      booksId.ForEach(bookId => _context.RequestDetails.Add(new RequestDetails { BookId = bookId, RequestOrderId = requestId }));
      _context.SaveChanges();
    }

    public RequestOrder ChangeStatus(int requestOrderId, int superUserId, Status status)
    {
      var request = _context.RequestOrders.FirstOrDefault(ro => ro.Id.Equals(requestOrderId));
      if (request == null){
        return null;
      }
      request.Status = status;
      request.SuperUserId = superUserId;
        return request;
    }

  }
  public interface IRequestOrderRepository : IRepositoryBase<RequestOrder>
  {
    IEnumerable<RequestOrder> GetAllRequestOrders();
    RequestOrder GetRequestOrderById(int requestOrderId );
    RequestOrder GetRequestOrderWithDetails(int requestOrderId);
    RequestOrder CreateRequest(int normalUserId , List<int> booksId);
    void AddListRequestDetails(int requestId,List<int> booksId);
    RequestOrder ChangeStatus(int requestOrderId, int superUserId, Status status);
  }
}
