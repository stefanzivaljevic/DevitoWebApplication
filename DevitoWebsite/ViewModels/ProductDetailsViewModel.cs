using DevitoWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.ViewModels
{
    public class ProductDetailsViewModel
    {

        public Product Product { get; set; }
        public int ProductId { get; set; }
        public IEnumerable<Size> Sizes { get; set; }

        [Range(1, 50,ErrorMessage ="Možete poručiti najmanje 1, a najviše 50 artikala. ")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Odaberite veličinu. ")]
        public string Size { get; set; }

    }
}
