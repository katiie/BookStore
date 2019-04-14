using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookstore.Core.Models
{
    public class Book: BaseClass
    {
        [Required]
        public string Name { get; set; }

        public string LocationTag { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }
        [Required]

        public string Description { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
