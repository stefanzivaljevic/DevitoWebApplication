using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevitoWebsite.Data;
using DevitoWebsite.Models;
using DevitoWebsite.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PayPal;
using Vereyon.Web;

namespace DevitoWebsite.Controllers
{
    [Route("Checkout")]
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IDevitoRepository _repository;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IFlashMessage _flashMessage;
        private readonly IConfiguration _configuration;

        public CheckoutController(IDevitoRepository repository, UserManager<StoreUser> userManager, IFlashMessage flashMessage, IConfiguration configuration)
        {
            _repository = repository;
            _userManager = userManager;
            _flashMessage = flashMessage;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            var countries = _repository.GetCountries();
            var cart = _repository.GetCartByUserId(user.Id);
            var cartItem = _repository.GetCartItemByCartId(cart.Id);

            cart.CartItem = cartItem;

            if(cart == null || cart.CartItem.Count <= 0)
            {
                return RedirectToAction("Index","Cart");
            }

            foreach(var c in cartItem)
            {
                if(c.Size == "Odaberite veličinu")
                    return RedirectToAction("Index", "Cart");
            }
            
            

            var viewModel = new CheckoutViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PostalNumber = user.PostalNumber,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                CountryId = user.CountryId,
                Country = countries,
                Cart = cart
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrderAsync(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var cart = _repository.GetCartByUserId(user.Id);
                var cartItem = _repository.GetCartItemByCartId(cart.Id);
                var countries = _repository.GetCountries();
                model.Country = countries;
                cart.CartItem = cartItem;

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("DevitoAnonymo porudžbina",
                _configuration["from"]);
                message.From.Add(from);

                InternetAddressList list = new InternetAddressList();
                list.Add(new MailboxAddress(user.FirstName+" "+user.LastName, user.Email));//user email
                list.Add(new MailboxAddress("MKOClothing","mkoclothingemail@asdf.com"));//MKOClothing email
                list.Add(new MailboxAddress("DevitoAnonytmo office", "planicar@gmail.com")); //davidov email
                message.To.AddRange(list);
                //--------------------------------------------
                message.Subject = "Nova porudžbina";

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                

                var order = new Order
                {
                    Orderdate = DateTime.Now,
                    User = user,
                    PaymentMethod = model.PaymentMethod,
                    OrderNumber = cart.Id+user.PhoneNumber+cart.CartItem.FirstOrDefault().Id+Math.Round((DateTime.Now.ToUniversalTime() - epoch).TotalSeconds)

                };

                    var mailHtmlBody = "<h1>Porudžbina - DevitoAnonymo.com</h1><br/><hr/><table><tbody>" +
                    "<tr><th>Broj porudžbine</th><td style='width: 200px'>" + order.OrderNumber + "</td></tr>" +
                    "<tr><th>Ime</th><td style='width: 200px'>" + model.FirstName + "</td></tr>" +
                    "<tr><th>Prezime</th><td style='width: 200px'>" + model.LastName + "</td></tr>" +
                    "<tr><th>Adresa</th><td style='width: 200px'>" + model.Address + "</td></tr>" +
                    "<tr><th>Poštanski broj</th><td style='width: 200px'>" + model.PostalNumber + "</td></tr>" +
                    "<tr><th>Grad</th><td style='width: 200px'>" + model.City + "</td></tr>" +
                    "<tr><th>Država</th><td style='width: 200px'>" + model.Country.SingleOrDefault(c => c.Id == model.CountryId).Title + "</td></tr>" +
                    "<tr><th>Broj telefona</th><td style='width: 200px'>" + model.PhoneNumber + "</td></tr>";
                    

                if(model.PaymentMethod == PaymentMethod.Pouzećem)
                {
                    mailHtmlBody += "<tr><th>Ukupna cena za naplatu: </th><td style='width: 200px'>" + cart.TotalPrice + " RSD</td></tr>";
                }
                else
                {
                    if(model.PaymentMethod == PaymentMethod.Paypal)
                        mailHtmlBody += "<tr><th>Ukupna cena za naplatu: </th><td style='width: 400px;color:green'>"+cart.TotalPrice+" RSD<strong> - PLAĆENO PUTEM PAYPAL-A</strong></td></tr>";
                }

                var orderItems = new List<OrderItem>();

                mailHtmlBody +=
                    "</tbody></table><hr/>"+
                    "<h3>Naručeni artikli: </h3><hr/>";
                    foreach(var c in cart.CartItem)
                {
                    mailHtmlBody += "<table><tbody>" +
                        "<tr><th>Broj artikla: </th><td style='width: 200px'>" + c.Product.ItemNumber+"</td></tr>" +
                        "<tr><th>Naziv: </th><td style='width: 200px'>" + c.Product.Title+ "</td></tr>" +
                        "<tr><th>Veličina: </th><td style='width: 200px'>" + c.Size + "</td></tr>" +
                        "<tr><th>Količina: </th><td style='width: 200px'>" + c.Quantity + "</td></tr>" +
                        "</tbody></table><hr/>";

                    

                    var orderItem = new OrderItem
                    {
                        Product = c.Product,
                        Quantity = c.Quantity,
                        Size = c.Size,
                        UnitPrice = c.Product.Price
                    };
                    
                    orderItems.Add(orderItem);

                }

                mailHtmlBody +="<br/><p><strong>Ukoliko postoji problem sa porudžbinom, javite se na email: porudzbine@devitoanonymo.com</strong></p>";

                order.OrderItems = orderItems;


                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = mailHtmlBody;

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_configuration["mailHost"], 465, true);
                await client.AuthenticateAsync(_configuration["from"], _configuration["appPass"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();

                _repository.AddEntity(order);

                _repository.RemoveEntity(cart);

                if (_repository.SaveAll())
                {
                    _flashMessage.Confirmation("Uspešno ste poručili artikle. Na email vam je poslata priznanica sa detaljima porudžbine. ");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _flashMessage.Warning("Greška pri obradi vašeg zahteva. ");
                    return RedirectToAction("Index", "Home");
                }

                
            }
            else
            {
                return View(model);
            }
            
        }


       

    }
}