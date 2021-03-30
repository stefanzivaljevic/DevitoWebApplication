using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string SizeLetter { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
