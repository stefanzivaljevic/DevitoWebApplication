using DevitoWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.ViewModels
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Cart = new Cart();
            Product = new Product();
        }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
