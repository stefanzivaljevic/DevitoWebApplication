using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevitoWebsite.Models;
using DevitoWebsite.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Vereyon.Web;

namespace DevitoWebsite.Controllers
{
    [Route("/Kontakt")]
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFlashMessage _flashMessage;

        public ContactController(IConfiguration configuration, IFlashMessage flashMessage)
        {
            _configuration = configuration;
            _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            var viewModel = new ContactMailViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> SendMailAsync(ContactMailViewModel viewModel)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("DevitoAnonymo Kontakt",
                _configuration["from"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("DevitoAnonymo Office",
                _configuration["to"]);
                message.To.Add(to);

                message.Subject = "Nova Kontakt Poruka";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Kontakt poruka</h1><br/>" +
                    "<table>" +
                    "<tbody>" +
                    "<tr><th>Ime</th><td style='width: 200px'>" + viewModel.Name + "</td></tr>" +
                    "<tr><th>Prezime</th><td style='width: 200px'>" + viewModel.LastName + "</td></tr>" +
                    "<tr><th>Email</th><td style='width: 200px'>" + viewModel.Email + "</td></tr></tbody></table>" +
                    "<h3>Poruka:</h3><p style='word-break: break-all;width: 300px;'>" + viewModel.Message + "</p>";

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_configuration["mailHost"], 465, true);
                await client.AuthenticateAsync(_configuration["from"], _configuration["appPass"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }



            _flashMessage.Info("Poruka uspešno poslata. ");
            return RedirectToAction("Index","Home");
        }


        [HttpPost]
        [Route("SendErrorMail")]
        public async Task<IActionResult> SendErrorMailAsync(ErrorViewModel viewModel)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("DevitoAnonymo Kontakt Za Probleme Na Sajtu",
                _configuration["from"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("Planicar",
                _configuration["to"]);
                message.To.Add(to);

                message.Subject = "Nova kontakt poruka o problemu na sajtu";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Kontakt poruka o problemu na sajtu</h1><br/>" +
                    "<table>" +
                    "<tbody>" +
                    "<tr><th>Ime</th><td style='width: 200px'>" + viewModel.Name + "</td></tr>" +
                    "<tr><th>Prezime</th><td style='width: 200px'>" + viewModel.LastName + "</td></tr>" +
                    "<tr><th>Email</th><td style='width: 200px'>" + viewModel.Email + "</td></tr></tbody></table>" +
                    "<h3>Poruka:</h3><p style='word-break: break-all;width: 300px;'>" + viewModel.Message + "</p>";
                //bodyBuilder.TextBody = "Hello World!";

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_configuration["mailHost"], 465, true);
                await client.AuthenticateAsync(_configuration["from"], _configuration["appPass"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            _flashMessage.Info("Poruka uspešno poslata. ");
            return RedirectToAction("Index", "Home");
        }
    }
}