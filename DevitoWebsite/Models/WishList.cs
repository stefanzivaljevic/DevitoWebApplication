using DevitoWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public virtual StoreUser User { get; set; }
        public IList<WishListItem> WishListItems { get; set; }
    }
}
