using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("BookBorrowingRequest")] 
    public class RequestOrder
    {
        [Key]
        [Column("BookBorrowingRequestId")]
        public int Id { get; set; }
        [Required]
        public Status Status { get; set; }
        public DateTime DateRequest { get; set; }
        public DateTime DateReturn { get; set; }
        [Required]
        public int NormalUserId { get; set; }
        public User NormalUser { get; set;}
        public int? SuperUserId { get; set; }
        public User SuperUser { get; set;}
        public virtual ICollection<Book> Books { get; set;}
        public virtual ICollection<RequestDetails > RequestDetails {get; set;}
    }
}
