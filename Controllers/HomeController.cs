using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        [HttpPost]
        public JsonResult SendEmail(EmailModel request)
        {
            if (request.Service != String.Empty && request.Cell != String.Empty)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
                email.To.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
                email.Subject = request.Service;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Message + " " +"My Cell: " + request.Cell };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                var message = "email sent";

                return Json(message);
            }

            var failed_message = "error occurd please provide more information";
            return Json(failed_message);
          


        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
