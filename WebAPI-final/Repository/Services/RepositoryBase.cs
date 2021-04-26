using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebAPI.Models;

namespace WebAPI.Repository
{
  public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
  {
    protected LibraryDBContext _context { get; set; } 
    public RepositoryBase(LibraryDBContext context) 
    {
        _context = context; 
    } 
    public IQueryable<T> FindAll()
    {
      return _context.Set<T>().AsNoTracking(); 
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
      return _context.Set<T>().Where(expression).AsNoTracking();
    }
    public async Task<EntityEntry<T>> Create(T entity)
    {
      return await _context.Set<T>().AddAsync(entity); 
    }
     public void Update(T entity)
    {
      _context.Set<T>().Update(entity);
      _context.SaveChanges();
    }
    public EntityEntry<T> Delete(T entity)
    {
      return _context.Set<T>().Remove(entity);
    }
  }
  public interface IRepositoryBase<T> where T : class
  {
    IQueryable<T> FindAll(); 
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression); 
    Task<EntityEntry<T>> Create(T entity);
    void Update(T entity);
    EntityEntry<T> Delete(T entity);
  }
}