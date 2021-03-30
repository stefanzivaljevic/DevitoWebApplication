using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevitoWebsite.Data;
using DevitoWebsite.Models;
using DevitoWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MailKit.Net.Smtp;
using Vereyon.Web;

namespace DevitoWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IDevitoRepository _repository;
        private readonly ILogger<AccountController> _logger;

        public IFlashMessage FlashMessage { get; }

        public AccountController(SignInManager<StoreUser> signInManager, UserManager<StoreUser> userManager, IConfiguration config, IDevitoRepository repository, IFlashMessage flashMessage, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _repository = repository;
            FlashMessage = flashMessage;
            _logger = logger;
        }

        public IActionResult Login(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");

            }

            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {

                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                var countries = _repository.GetCountries().ToList();
                var viewModel = new RegisterViewModel()
                {

                    PostalNumber = null,
                    Country = countries
                };
                return View(viewModel);
            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            

            if(_userManager.FindByEmailAsync(model.Email) == null)
            {
                ModelState.AddModelError("", "Neuspešna prijava. ");
                return View();
            }
            else
            {
                var user = _userManager.FindByEmailAsync(model.Email);

                if (user.Result.EmailConfirmed)
                {
                    if (ModelState.IsValid)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                        if (result.Succeeded)
                        {
                            if (Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(Request.Query["ReturnUrl"].First());
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                    }
                    ModelState.AddModelError("", "Neuspešna prijava. ");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Nalog nije aktiviran. Na email vam je poslat link za aktivaciju naloga. ");
                    return View();
                }

            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                if(_repository.IsThereSameEmail(model.Email))
                {
                    model.Country = _repository.GetCountries();
                    ModelState.AddModelError("Email", "Korisnik sa istom email adresom je već registrovan. ");
                    ModelState.AddModelError(string.Empty, "Korisnik sa istom email adresom je već registrovan. ");
                    return View(model);
                }
                else
                {
                    var user = new StoreUser { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, UserName = model.Email, Address = model.Address, PostalNumber = model.PostalNumber,City = model.City , CountryId = model.CountryId, PhoneNumber = model.PhoneNumber };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmEmail","Account", new
                        {
                            userId = user.Id, token = token
                        }, Request.Scheme);


                        try
                        {
                            MimeMessage message = new MimeMessage();

                            MailboxAddress from = new MailboxAddress("DevitoAnonymo",
                            "stefan.zivaljevic@gmail.com");
                            message.From.Add(from);

                            MailboxAddress to = new MailboxAddress(model.FirstName +" "+ model.LastName,
                            "planicar@gmail.com");
                            //promeniti da stize na model.Email
                            message.To.Add(to);
                            
                            message.Subject = "Aktivacija naloga - DevitoAnonymo.com";

                            BodyBuilder bodyBuilder = new BodyBuilder();
                            bodyBuilder.HtmlBody = "<h1>Aktivacija naloga - DevitoAnonymo.com</h1><br/>" +
                                "<h3>Da biste aktivirali svoj nalog, kliknite na sledeći link:</h3>" +
                                confirmationLink +
                                "<br/><hr style='width:100%'/><p><strong>Ukoliko postoji problem sa pristupom nalogu, javite se na email: office@devitoanonymo.com</strong></p>";

                            message.Body = bodyBuilder.ToMessageBody();

                            SmtpClient client = new SmtpClient();
                            await client.ConnectAsync("Smtp.gmail.com", 465, true);
                            await client.AuthenticateAsync("stefan.zivaljevic@gmail.com", "zdmmgzjsmsiizygb");

                            await client.SendAsync(message);
                            await client.DisconnectAsync(true);
                            client.Dispose();
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }

                        FlashMessage.Info("Uspešno ste registrovani. Pre nego što se prijavite, molimo vas potvrdite svoj nalog putem aktivacionog linka koji vam je poslat na email. ");
                        return Redirect("/Account/Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }

                        return View(model);

                    }
                }


            }
            else
            {

                return View(model);
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateTokenAsync([FromBody] LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByNameAsync(model.Email);

        //        if (user != null)
        //        {
        //            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);



        //            if (result.Succeeded)
        //            {
        //                //Create the token
        //                var claims = new[]
        //                {
        //                new Claim(JwtRegisteredClaimNames.Sub, user.Email), 
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)  
        //            };

        //                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
        //                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //                var token = new JwtSecurityToken(
        //                        _config["Tokens:Issuer"], 
        //                        _config["Tokens:Audience"],
        //                        claims,
        //                        expires: DateTime.Now.AddMinutes(30),
        //                        signingCredentials: creds);

        //                var results = new
        //                {
        //                    token = new JwtSecurityTokenHandler().WriteToken(token), 
        //                    expiration = token.ValidTo
        //                };

        //                return Created("", results); 
        //            }

        //        }
        //    }

        //    return BadRequest();
        //}

        [Authorize]
        [Route("Account/Details")]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            var country = _repository.GetCountryById(user.CountryId);
            var wishList = _repository.GetWishListByUserId(user.Id);

            var viewModel = new UserDetailsViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PostalNumber = user.PostalNumber,
                City = user.City,
                Country = country,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                WishList = wishList
            };

            return View(viewModel);
        }

        //Update User

        [Authorize]
        [Route("Account/Edit")]
        public IActionResult EditUserContactInfo(string returnUrl = null)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var countries = _repository.GetCountries().ToList();
            var viewModel = new RegisterViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PostalNumber = user.PostalNumber,
                City = user.City,
                Country = countries,
                CountryId = user.CountryId,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                viewModel.ReturnUrl = returnUrl;
                return View(viewModel);
            }
            else
            {
                return View(viewModel);
            }          
                        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/EditContactInfoPost")]
        public async Task<IActionResult> EditUserContactInfoPost(RegisterViewModel model)
        {

            model.Country = _repository.GetCountries();
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address= model.Address; //custom property
                user.PostalNumber = model.PostalNumber;
                user.City = model.City;
                user.CountryId = model.CountryId;

                if (_userManager.CheckPasswordAsync(user,model.Password).Result == true)
                {
                    // Password is correct 
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        if (model.ReturnUrl != null)
                        {
                            FlashMessage.Info("Uspešno ste izmenili podatke. ");
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            FlashMessage.Info("Uspešno ste izmenili podatke. ");
                            return Redirect("/Account/Details");
                        }


                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }

                        return View("EditUserContactInfo", model);

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Pogrešna lozinka. ");
                    ModelState.AddModelError("Password","Pogrešna lozinka. ");
                    return View("EditUserContactInfo", model);
                }

                
            }
            else
            {
                return View("EditUserContactInfo", model);
            }
        }

        [Authorize]
        [Route("/Account/EditLogin")]
        public IActionResult EditUserLoginInfo()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var viewModel = new ChangePasswordViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PostalNumber = user.PostalNumber,
                City = user.City,
                CountryId = user.CountryId,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Account/EditUserLoginInfoAsync")]
        public async Task<IActionResult> EditUserLoginInfoAsync(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.PostalNumber = model.PostalNumber;
                user.City = model.City;
                user.CountryId = model.CountryId;

                if (_userManager.CheckPasswordAsync(user, model.OldPassword).Result == true)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

                    // Password is correct 
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {

                        FlashMessage.Info("Uspešno ste promenili lozinku. ");
                        return Redirect("/Account/Details");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }

                        return View("EditUserLoginInfo", model);

                    }
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Pogrešna lozinka. ");
                    return View("EditUserLoginInfo", model);
                }


            }
            else
            {
                return View("EditUserLoginInfo", model);
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            if(userId == null || token == null)
            {
                return RedirectToAction("Index","Home");
            }
            
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                FlashMessage.Warning("Neuspešna aktivacija. ");
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                FlashMessage.Info("Uspešno ste aktivirali nalog. ");
                return View("Login");
            }

            FlashMessage.Warning("Neuspešna aktivacija. ");
            return RedirectToAction("Index", "Home");
        }



        public IActionResult ForgotPassword()
        {
            var viewModel = new ForgotPasswordViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("/Account/ForgotPasswordPost")]
        public async Task<IActionResult> ForgotPasswordPostAsync(ForgotPasswordViewModel viewModel)
        {
            var email = viewModel.Email;

            if (_repository.IsThereSameEmail(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var confirmationLink = Url.Action("ForgotPasswordReset", "Account", new
                {
                    userId = user.Id,
                    token = token
                }, Request.Scheme);

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("DevitoAnonymo promena lozinke",
                _config["from"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("DevitoAnonymo korisnik",
                _config["to"]);
                message.To.Add(to);

                message.Subject = "DevitoAnonymo - Promena lozinke";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>DevitoAnonymo - Zahtev za promenu lozinke</h1><br/>" +
                    "<p>Poslali ste zahtev za promenu lozinke vašeg naloga. Kliknite na sledeći link kako biste promenili lozinku.</p><br/>" +
                    confirmationLink +
                    "<br/><p><strong>Ako niste Vi poslali zahtev za promenu lozinke, ignorišite ovu poruku.</strong></p>" +
                    "<br/><hr style='width:100%'/><p><strong>Ukoliko postoji problem sa pristupom nalogu, javite se na email: office@devitoanonymo.com</strong></p>";

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_config["mailHost"], 465, true);
                await client.AuthenticateAsync(_config["from"], _config["appPass"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();

                FlashMessage.Info("Na email vam je poslat zahtev za promenu lozinke.  ");
                return Redirect("/Account/Login");


            }
            else
            {
                ModelState.AddModelError("Email","Korisnik sa unetom email adresom ne postoji. ");
                return View();
            }
        }

        [AllowAnonymous]
        [Route("/Account/ForgotPasswordReset")]
        public async Task<IActionResult> ForgotPasswordReset(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                FlashMessage.Warning("Neuspešna promena lozinke. ");
                return RedirectToAction("Index", "Home");
            }


            var viewModel = new ForgotPasswordResetViewModel()
            {
                Email = user.Email,
                Token = token
            };

            return View(viewModel);
        }



        [Route("/Account/ForgotPasswordResetPost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordResetPostAsync(ForgotPasswordResetViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);

                //var newPassword = _userManager.PasswordHasher.HashPassword(user,viewModel.Password);

                var result = await _userManager.ResetPasswordAsync(user,viewModel.Token,viewModel.Password);

                if (result.Succeeded)
                {
                    FlashMessage.Info("Uspešno ste promenili lozinku. ");
                    return View("Login");
                }
                else
                {
                    FlashMessage.Warning("Neuspešna promena lozinke. ");
                    return View("Login");
                }
            }
            else
            {
                return View("ForgotPasswordReset", viewModel);
            }
                        
        }

    }
}