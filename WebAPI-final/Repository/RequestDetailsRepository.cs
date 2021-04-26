using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.Models;

namespace WebAPI.Repository
{
  public class RequestDetailsRepository : RepositoryBase<RequestDetails>, IRequestDetailsRepository
  {
    public RequestDetailsRepository(LibraryDBContext context) 
      : base(context) 
    { 
    }

    public IEnumerable<RequestDetails> GetAllRequestDetails()
    {
      return FindAll()
        .OrderBy(rd => rd.Id)
        .ToList();
    }

    public RequestDetails GetRequestDetailsById(int requestDetailsId)
    {
      return FindByCondition(rd => rd.Id.Equals(requestDetailsId))
        .FirstOrDefault();
    }
  }
  public interface IRequestDetailsRepository : IRepositoryBase<RequestDetails>
  {
    IEnumerable<RequestDetails> GetAllRequestDetails();
    RequestDetails GetRequestDetailsById(int requestDetailsId );

  }
}
