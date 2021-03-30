using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevitoWebsite.Data;
using DevitoWebsite.Models;
using DevitoWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace DevitoWebsite.Controllers
{
    [Authorize]
    [Route("WishList")]
    public class WishListController : Controller
    {
        private readonly IDevitoRepository _repository;
        private readonly UserManager<StoreUser> _userManager;

        public IFlashMessage FlashMessage { get; }
        public WishListController(IDevitoRepository repository, UserManager<StoreUser> userManager, IFlashMessage flashMessage)
        {
            _repository = repository;
            _userManager = userManager;
            FlashMessage = flashMessage;
        }


        public IActionResult Index()
        {
            return Redirect("/Account/Details?wishlist");
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToWishList(Product viewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                var item = _repository.GetProductById(viewModel.Id);
                var user = _userManager.GetUserAsync(User).Result;
                string userId = user.Id;

                var wishList = _repository.GetWishListByUserId(userId);
                if (ModelState.IsValid)
                {


                    if (wishList == null)
                    {


                        var wishListModel = new WishList();
                        wishListModel.User = user;
                        wishListModel.WishListItems = new List<WishListItem>()
                {
                    new WishListItem()
                    {
                        Product = item,
                        WishListId = wishListModel.Id
                    }
                };


                        _repository.AddEntity(wishListModel);


                        if (_repository.SaveAll())
                        {
                            //var result = new { Success = "true", message = "Uspešno ste dodali artikal u korpu. " };
                            //return Json(result);
                            FlashMessage.Info("Uspešno ste dodali artikal u listu želja. ");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            throw new Exception("Nije uspelo");
                        }


                    }
                    else
                    {
                        item = _repository.GetProductById(viewModel.Id);
                        viewModel = item;

                        if (_repository.IsThereSameProductInTheWishList(wishList.Id, item.Id) != false)
                        {
                            //var result = new { Success = "true", message = "Već ste dodali ovaj artikal u korpu. " };
                            //return Json(result);


                            ModelState.AddModelError("Product", "Već ste dodali ovaj artikal u listu želja. ");
                            return View("../Home/Details", viewModel);
                        }
                        else
                        {



                            wishList.WishListItems.Append(
                            new WishListItem()
                            {
                                Product = item,
                                WishListId = wishList.Id
                            });


                            _repository.AddEntity(new WishListItem()
                            {
                                Product = item,
                                WishListId = wishList.Id
                            });

                            _repository.UpdateEntity(wishList);

                            if (_repository.SaveAll())
                            {
                                //return Json(new { Success = true });
                                FlashMessage.Info("Uspešno ste dodali artikal u listu želja. ");
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
                    viewModel = item;
                    return View("../Home/Details", viewModel);
                }
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            


        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult RemoveFromWishList(string wishListItemId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            string userId = user.Id;
            var wishList = _repository.GetWishListByUserId(userId);



            WishListItem wishListItem = _repository.GetWishListItemById(int.Parse(wishListItemId));

            _repository.RemoveWishListItem(wishListItem);





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
    }
}