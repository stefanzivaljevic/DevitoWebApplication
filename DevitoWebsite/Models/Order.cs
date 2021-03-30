using DevitoWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Orderdate { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public StoreUser User { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
}
