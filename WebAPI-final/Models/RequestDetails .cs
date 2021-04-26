using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("BookBorrowingRequestDetails")] 
    public class RequestDetails 
    {
        [Key]
        [Column("BookBorrowingRequestDetailsId")]
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book {get; set; }
        public int RequestOrderId { get; set; }
        public virtual RequestOrder RequestOrder {get; set;}

    }
}
