using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookstore.Core.Models
{
    public class BaseClass
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public BaseClass()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }
    }
}
