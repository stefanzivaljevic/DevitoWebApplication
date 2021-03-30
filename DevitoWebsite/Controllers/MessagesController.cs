using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace DevitoWebsite.Controllers
{
    [Route("Messages")]
    public class MessagesController : Controller
    {
        public MessagesController(IFlashMessage flashMessage)
        {
            FlashMessage = flashMessage;
        }

        public IFlashMessage FlashMessage { get; }

        public IActionResult Index()
        {
            return RedirectToAction("Error","Home");
        }

        [HttpPost]
        [Route("Cart/Success")]
        public JsonResult AddToCartSuccess()
        {

            FlashMessage.Info("Uspešno ste dodali artikal u korpu. ");
            var result = new { Success = "true" };
            return Json(result);
        }

        [HttpPost]
        [Route("Cart/SameItemError")]
        public JsonResult AddToCartSameItem()
        {
            FlashMessage.Warning("Već ste dodali ovaj artikal u korpu. ");
            var result = new { Success = "true" };
            return Json(result);
        }

        
        [HttpPost]
        [Route("WishList/SameItemError")]
        public JsonResult AddToWishListSameItem()
        {

            FlashMessage.Warning("Već ste dodali ovaj artikal u listu želja. ");
            var result = new { Success = "true" };
            return Json(result);
        }

        [HttpPost]
        [Route("WishList/Success")]
        public JsonResult AddToWishListSuccess()
        {

            FlashMessage.Info("Uspešno ste dodali artikal u listu želja. ");
            var result = new { Success = "true" };
            return Json(result);
        }




    }
}