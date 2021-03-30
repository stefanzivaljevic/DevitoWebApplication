using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DevitoWebsite.Data;
using DevitoWebsite.Models;
using DevitoWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Vereyon.Web;

namespace DevitoWebsite.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly IDevitoRepository _repository;
        private readonly UserManager<StoreUser> _userManager;

        public IFlashMessage FlashMessage { get; }

        public CartController(IDevitoRepository repository, UserManager<StoreUser> userManager, IFlashMessage flashMessage)
        {
            _repository = repository;
            _userManager = userManager;
            FlashMessage = flashMessage;
        }
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result.Id;
            var cart = _repository.GetCartByUserId(user);
            if (cart == null)
            {
                return View(null);
            }
            else
            {
                var cartItem = _repository.GetCartItemByCartId(cart.Id);
                cart.CartItem = cartItem;
                var viewModel = new CartViewModel
                {
                    Cart = cart
                };
                return View(viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(ProductDetailsViewModel viewModel)
        {
            var item = _repository.GetProductById(viewModel.ProductId);
            var user = _userManager.GetUserAsync(User).Result;
            string userId = user.Id;

            var cart = _repository.GetCartByUserId(userId);
            if (ModelState.IsValid)
            {


                if (cart == null)
                {


                    var cartModel = new Cart();
                    cartModel.User = user;
                    cartModel.CartItem = new List<CartItem>()
                {
                    new CartItem()
                    {
                        Product = item,
                        Quantity = viewModel.Quantity,
                        Size = viewModel.Size,
                        CartId = cartModel.Id
                    }
                };
                    cartModel.TotalPrice += item.Price * viewModel.Quantity;


                    _repository.AddEntity(cartModel);


                    if (_repository.SaveAll())
                    {
                        //var result = new { Success = "true", message = "Uspešno ste dodali artikal u korpu. " };
                        //return Json(result);
                        FlashMessage.Info("Uspešno ste dodali artikal u korpu. ");
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        throw new Exception("Nije uspelo");
                    }


                }
                else
                {
                    item = _repository.GetProductById(viewModel.ProductId);
                    viewModel.Product = item;

                    if (_repository.IsThereSameProductInTheCart(cart.Id, item.Id) != false)
                    {
                        //var result = new { Success = "true", message = "Već ste dodali ovaj artikal u korpu. " };
                        //return Json(result);

                        
                        ModelState.AddModelError("Product", "Već ste dodali ovaj artikal u korpu. ");
                        return View("../Home/Details", viewModel);
                    }
                    else
                    {



                        cart.CartItem.Append(
                        new CartItem()
                        {
                            Product = item,
                            Quantity = viewModel.Quantity,
                            Size = viewModel.Size,
                            CartId = cart.Id
                        }
                    );



                        cart.TotalPrice += item.Price * viewModel.Quantity;



                        _repository.AddEntity(new CartItem()
                        {
                            Product = item,
                            Quantity = viewModel.Quantity,
                            Size = viewModel.Size,
                            CartId = cart.Id
                        });

                        _repository.UpdateCartWithNewCartItem(cart);

                        if (_repository.SaveAll())
                        {
                            //return Json(new { Success = true });
                            FlashMessage.Info("Uspešno ste dodali artikal u korpu. ");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            //var result = new { Success = "true", message = "Uspešno ste dodali artikal u korpu. " };
                            //return Json(result);

                            return RedirectToAction("Index", "Home");
                        }

                    }
                }
            }
            else
            {
                viewModel.Sizes = _repository.GetSizesById(item.Id);
                viewModel.Product = item;
                return View("../Home/Details", viewModel);
            }
            

        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult RemoveFromCart(string cartItemId, int quantity)
        {
            var user = _userManager.GetUserAsync(User).Result;
            string userId = user.Id;
            var cart = _repository.GetCartByUserId(userId);            

            CartItem cartItem = _repository.GetCartItemById(int.Parse(cartItemId));

            cart.TotalPrice -=cartItem.Product.Price*quantity;

            _repository.RemoveCartItem(cartItem);

            if (_repository.SaveAll())
            {
                var result = new { Success = "true", message = "Uspešno ste uklonili artikal iz korpe. " };
                return Json(result);
            }
            else
            {
                var result = new { Success = "true", message = "Uspešno ste uklonili artikal iz korpe. " };
                return Json(result);
            }
        }


        [HttpPut]
        public async Task<JsonResult> UpdateCartItemAsync(string cartItemId, int quantity, string size, int quantityBeforeChange)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = _repository.GetCartByUserId(user.Id);
            var cartItem = _repository.GetCartItemByCartId(cart.Id);
            var cItemId = int.Parse(cartItemId);

            foreach(var item in cartItem)
            {
 
                if(item.Id == int.Parse(cartItemId))
                {
                    var cartItemQuantity = cartItem.SingleOrDefault(id => id.Id == cItemId).Quantity = quantity;
                    cartItem.SingleOrDefault(id => id.Id == cItemId).Size = size;

                    if (cartItemQuantity == quantityBeforeChange)
                    {
                        continue;
                    }
                    else
                    {
                        if (cartItemQuantity > quantityBeforeChange)
                            cart.TotalPrice += item.Product.Price;

                        else if (cartItemQuantity < quantityBeforeChange)
                            cart.TotalPrice -= item.Product.Price;
                    }
                }
                

            }

            cart.CartItem = cartItem;

            _repository.UpdateEntity(cart);

            if (_repository.SaveAll())
            {
                var result = new { Success = "true", message = cart.Id};
                return Json(result);
            }
            else
            {
                var result = new { Success = "true", message = cart.Id };
                return Json(result);
            }
            
        }

        [Route("GetPrice")]
        public async Task<JsonResult> GetPriceAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = _repository.GetCartByUserId(user.Id);

            if (cart == null)
            {
                var result = new { Success = "false", error = "error" };
                return Json(result);
            }
            else
            {
                var totalPrice = cart.TotalPrice;
                var result = new { Success = "true", message = "<p class='font-weight-bold d-inline float-right'>"+ totalPrice + " RSD </p>", message2 = "<p class='font-weight-bold d-inline float-right'>" + (totalPrice+250) + " RSD </p>" };
                return Json(result);
            }
            


        }


    }
    
}