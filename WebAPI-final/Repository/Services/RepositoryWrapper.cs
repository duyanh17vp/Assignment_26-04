

using WebAPI.Models;

namespace WebAPI.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper 
    { 
        private LibraryDBContext _context; 
        private IBookRepository _book;
        private ICategoryRepository _category;
        private IUserRepository _user;
        private IRequestOrderRepository _requestOrder;
        private IRequestDetailsRepository _requestDetails;
        public IBookRepository Book 
        { 
            get 
            { 
                if (_book == null) 
                { 
                    _book = new BookRepository(_context); 
                } 
                return _book; 
            } 
        } 
        
        public ICategoryRepository Category 
        { 
            get 
            { 
                if (_category == null) 
                { 
                    _category = new CategoryRepository(_context); 
                } 
                return _category; 
            } 
        }
        public IUserRepository User 
        { 
            get 
            { 
                if (_user == null) 
                { 
                    _user = new UserRepository(_context); 
                } 
                return _user; 
            } 
        }
        public IRequestOrderRepository RequestOrder 
        { 
            get 
            { 
                if (_requestOrder == null) 
                { 
                    _requestOrder = new RequestOrderRepository(_context); 
                } 
                return _requestOrder; 
            } 
        }
        public IRequestDetailsRepository RequestDetails 
        { 
            get 
            { 
                if (_requestDetails == null) 
                { 
                    _requestDetails = new RequestDetailsRepository(_context); 
                } 
                return _requestDetails; 
            } 
        } 
        
        public RepositoryWrapper(LibraryDBContext context) 
        { 
            _context = context; 
        } 
        
        public void Save() 
        {
            _context.SaveChanges();
        } 
    }
    public interface IRepositoryWrapper 
    { 
        IBookRepository Book { get; } 
        ICategoryRepository Category { get; } 
        IUserRepository User { get; }
        IRequestOrderRepository RequestOrder { get; }
        IRequestDetailsRepository RequestDetails { get; }
        void Save(); 
    }
}
