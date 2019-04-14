using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookstore.Core.Models
{
    public class Store:BaseClass
    {
        public string Name { get; set; }
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

        public double  Distance { get; set; }

        public Store()
        {
            Country = "Nigeria";
        }

       
    }


    
}
