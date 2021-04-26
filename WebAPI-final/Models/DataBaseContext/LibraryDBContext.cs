using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
  public class LibraryDBContext : DbContext
  {
    private readonly Action<LibraryDBContext, ModelBuilder> _customizeModel;

        #region Constructors
        public LibraryDBContext()
        {
        }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) 
          : base(options) 
        {
        }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options, Action<LibraryDBContext, ModelBuilder> customizeModel)
            : base(options)
        {
            _customizeModel = customizeModel;
        }
        #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //Category
      modelBuilder.Entity<Category>()
        .HasData(
          new Category
          {
            Id = 1,
            Name = "Category1"
          },
          new Category
          {
            Id = 2,
            Name = "Category2"
          },
          new Category
          {
            Id = 3,
            Name = "Category3"
          },
          new Category
          {
            Id = 4,
            Name = "Category4"
          }
        );
      //Book
      modelBuilder.Entity<Book>()
        .HasOne<Category>(s => s.Category)
        .WithMany(g => g.Books)
        .HasForeignKey(s => s.CategoryId)
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<Book>()
        .HasData(
          new Book
          {
            Id = 1,
            Name = "Book1",
            Title = "title",
            CategoryId = 1
          },
          new Book
          {
            Id = 2,
            Name = "Book2",
            Title = "title",
            CategoryId = 1
          }
        );
      //RequestOrder
      modelBuilder.Entity<RequestOrder>()
        .Property(u => u.Status)
        .HasDefaultValue(Status.waiting);
      modelBuilder.Entity<RequestOrder>()
        .HasOne<User>(s => s.NormalUser)
        .WithMany(g => g.RequestOrders)
        .HasForeignKey(s => s.NormalUserId)
        .OnDelete(DeleteBehavior.Cascade);
      //
      modelBuilder.Entity<RequestDetails>()
        .HasOne<RequestOrder>(s => s.RequestOrder)
        .WithMany(g => g.RequestDetails)
        .HasForeignKey(s => s.RequestOrderId);
      modelBuilder.Entity<RequestDetails>()
        .HasOne<Book>(s => s.Book)
        .WithMany(g => g.RequestDetails)
        .HasForeignKey(s => s.BookId);
      //User
      modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasDefaultValue(Role.NormalUser);
      modelBuilder.Entity<User>()
        .HasData(
           new User { Id = 1, FullName = "Admin", UserName = "superuser", Password = "superuser", Role = Role.SuperUser },
            new User { Id = 2, FullName = "Normal", UserName = "normaluser", Password = "normaluser", Role = Role.NormalUser }
        );
    }
    //
    public DbSet<Book> Books {get; set; }
    public DbSet<Category> Categories {get; set; }
    public DbSet<User> Users {get; set; }
    public DbSet<RequestDetails > RequestDetails {get; set; }
    public DbSet<RequestOrder> RequestOrders {get; set; }
  }
}