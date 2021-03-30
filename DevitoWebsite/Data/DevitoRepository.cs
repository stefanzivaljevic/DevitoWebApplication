using DevitoWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevitoWebsite.Data
{
    public class DevitoRepository : IDevitoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<StoreUser> _userManager;

        public bool IsThereSameEmail(string email)
        {
            if (_userManager.FindByEmailAsync(email).Result == null)
                return false;
            else
                return true;
        }

        public DevitoRepository(ApplicationDbContext context, UserManager<StoreUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products
                .OrderBy(t => t.Id)
                .ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.SingleOrDefault(p => p.Id == id);
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }
        
        public void RemoveEntity(object model)
        {
            _context.Remove(model);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Country> GetCountries()
        {
            return _context.Countries
                .OrderBy(c=>c.Title)
                .ToList();
        }

        public Country GetCountryById(int countryId)
        {
            return _context.Countries.SingleOrDefault(c=>c.Id == countryId);
        }

        public IEnumerable<Size> GetSizesById(int id)
        {
            return _context.Size
                .Where(s=>s.ProductId == id)
                .ToList();
            
        }

        public Cart GetCartByUserId(string id)
        {
            if(_context.Carts.SingleOrDefault(u => u.User.Id == id) == null)
            {
                return null;
            }
            else
            {

                var cart = _context.Carts.SingleOrDefault(u => u.User.Id == id);
                //var cart = _userManager.FindByIdAsync(id).Result.Cart;
                var cartItem = _context.CartItems.Where(c => c.CartId == cart.Id).ToList();
                //var cartItem = _userManager.FindByIdAsync(id).Result.Cart.CartItem;
                cart.CartItem = cartItem;
                return cart;
            }
            
        }

        public void UpdateEntity(object model)
        {
            _context.Update(model);
        }

        public void UpdateCartWithNewCartItem(Cart model)
        {
            
            _context.Update(model);
        }

        public bool IsThereSameProductInTheCart(int id, int productId)
        {
            if (_context.CartItems.Where(c => c.CartId == id).Any())
            {

                if(_context.CartItems.Where(c => c.CartId == id).Where(p => p.Product.Id == productId).Any())
                {
                    if(_context.CartItems.Where(c => c.CartId == id).Where(p => p.Product.Id == productId).SingleOrDefault(o => o.Product.Id == productId).Product.Id == productId)
                        return true;

                    else 
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public IList<CartItem> GetCartItemByCartId(int id)
        {

            //var cartItem = _context.CartItems.Where(c => c.CartId == id).Include(p=>p.Product).Include(s=>s.Size).ToList();

            var cartItem = _context.CartItems.Where(c => c.CartId == id)
                .Include(p => p.Product).ThenInclude(s => s.Sizes)  
                .ToList();


            return cartItem;
        }

        public void RemoveCartItems(IList<CartItem> cartItem)
        {
            _context.RemoveRange(cartItem);
        }

        public void RemoveCart(Cart cart)
        {
            _context.Carts.Remove(cart);
        }

        public CartItem GetCartItemByProductId(int productId)
        {
            return _context.CartItems.SingleOrDefault(p=>p.Product.Id == productId);
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);


        }

        public CartItem GetCartItemById(int cartItemId)
        {
            return _context.CartItems
                .Include(p=>p.Product)
                .SingleOrDefault(ca=>ca.Id == cartItemId);
        }

        public Product GetProductByCartItemId(int cartItemId)
        {
            return _context.CartItems
                .Include(p=>p.Product)
                .SingleOrDefault(c=>c.Id == cartItemId).Product;
        }

        public Cart GetCartById(int iD)
        {
            return _context.Carts.SingleOrDefault(c=>c.Id == iD);
        }

        public WishList GetWishListByUserId(string id)
        {
            if (_context.WishList.SingleOrDefault(u => u.User.Id == id) == null)
            {
                return null;
            }
            else
            {

                var wishList = _context.WishList.SingleOrDefault(u => u.User.Id == id);
                var wishListItems = _context.WishListItems.Where(w=>w.WishListId == wishList.Id).Include(p=>p.Product).ToList();
                wishList.WishListItems = wishListItems;
                

                return wishList;
            }
        }

        public bool IsThereSameProductInTheWishList(int id, int productId)
        {
            if (_context.WishListItems.Where(w => w.WishListId == id).Any())
            {

                if (_context.WishListItems.Where(c => c.WishListId == id).Where(p => p.Product.Id == productId).Any())
                {
                    if (_context.WishListItems.Where(c => c.WishListId == id).Where(p => p.Product.Id == productId).SingleOrDefault(o => o.Product.Id == productId).Product.Id == productId)
                        return true;

                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public WishListItem GetWishListItemById(int id)
        {
            return _context.WishListItems
                .Include(p => p.Product)
                .SingleOrDefault(w=>w.Id == id);
        }

        public void RemoveWishListItem(WishListItem wishListItem)
        {
            _context.WishListItems.Remove(wishListItem);
        }
    }
}
