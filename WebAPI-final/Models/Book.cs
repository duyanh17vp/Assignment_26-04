using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Book
    {
        [Key]
        [Column("BookId")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        
        public virtual Category Category { get; set;}
        public virtual ICollection<RequestOrder> RequestOrders { get; set;}
        public virtual ICollection<RequestDetails > RequestDetails {get; set;}

    }
}
