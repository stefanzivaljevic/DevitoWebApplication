using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Size { get; set; }
        public double UnitPrice { get; set; }
        public Order Order { get; set; }
    }
}
