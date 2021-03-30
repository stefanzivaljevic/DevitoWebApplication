using System;
using System.Collections.Generic;
using System.Text;
using DevitoWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevitoWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Country> Countries {get; set;}
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Country>()
                .Property(a=>a.Id);

            builder.Entity<Cart>()
                .HasKey(c=> new{ c.Id});

            builder.Entity<CartItem>()
                .HasKey(c => new { c.Id });

            builder.Entity<WishList>()
                .HasKey(w => new { w.Id });

            builder.Entity<WishListItem>()
                .HasKey(w => new { w.Id });

            builder.Entity<Order>()
                .HasKey(o => new { o.Id });

            builder.Entity<OrderItem>()
                .HasKey(o => new { o.Id });
        }
    }
}
