using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookstore.Core.Models
{
    public class Stock:BaseClass
    {

        [Required]
        public int Quantity { get; set; }

        public int Dispatched { get; set; }

        public int Leftover { get; set; }

        public bool IsAvailable { get; set; }

        public string Status { get; set; }


        public double Distance { get; set; }

        [Required]
        public Guid StoreId { get; set; }

        [Required]
        public Guid BookId { get; set; }
        public virtual Store Store{ get;  set; }

        public virtual Book Book { get; set; }

        //public void UpdateStock(int _dispatch)
        //{
        //   // Dispatched += _dispatch ;
        //    Leftover = Quantity != 0 && Dispatched != 0? Quantity - Dispatched:0;

        //}
      
    }
}
