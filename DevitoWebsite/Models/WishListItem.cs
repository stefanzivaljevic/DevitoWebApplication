using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class WishListItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public WishList WishList { get; set; }
        public int WishListId { get; set; }
    }
}
