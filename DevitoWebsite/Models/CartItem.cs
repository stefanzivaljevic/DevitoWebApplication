using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Size { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }

    }
}
