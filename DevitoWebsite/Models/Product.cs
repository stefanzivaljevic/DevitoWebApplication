using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ItemNumber { get; set; }
        public List<Size> Sizes { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }


    }
}
