using DevitoWebsite.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevitoWebsite.Data
{
    public interface IDevitoRepository
    {

        
        void AddEntity(object model);
        Product GetProductById(int id);
        IEnumerable<Product> GetProducts();
        bool SaveAll();
        IEnumerable<Country> GetCountries();
        Country GetCountryById(int countryId);
        IEnumerable<Size> GetSizesById(int id);
        Cart GetCartByUserId(string id);
        void UpdateEntity(object model);
        void UpdateCartWithNewCartItem(Cart viewModel);
        bool IsThereSameProductInTheCart(int id, int productId);
        IList<CartItem> GetCartItemByCartId(int id);
        void RemoveCartItems(IList<CartItem> cartItem);
        void RemoveCart(Cart cart);
        CartItem GetCartItemByProductId(int productId);
        void RemoveCartItem(CartItem cartItem);
        CartItem GetCartItemById(int cartItemId);
        bool IsThereSameEmail(string email);
        Cart GetCartById(int iD);
        WishList GetWishListByUserId(string id);
        bool IsThereSameProductInTheWishList(int id, int productId);
        WishListItem GetWishListItemById(int id);
        void RemoveWishListItem(WishListItem wishListItem);
        void RemoveEntity(object model);
    }
}