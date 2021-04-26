using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Category
    {
        [Key]
        [Column("CategoryId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Book> Books {get; set; }
    }
}
