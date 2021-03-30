using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevitoWebsite.Models;
using DevitoWebsite.Data;
using DevitoWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DevitoWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDevitoRepository _repository;
        private readonly UserManager<StoreUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IDevitoRepository repository, UserManager<StoreUser> userManager)
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var data = _repository.GetProducts();
            return View(data);
        }

        [Route("Products/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var product = _repository.GetProductById(id);
            var size = _repository.GetSizesById(id);
            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Sizes = size
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/UsloviKoriscenja")]
        public IActionResult TermsOfUse()
        {
            return View();
        }

        [Route("/Privatnost")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
