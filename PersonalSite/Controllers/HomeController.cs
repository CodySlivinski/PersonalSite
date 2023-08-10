using Microsoft.AspNetCore.Mvc;
using PersonalSite.Models;
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace PersonalSite.Controllers
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PortfolioDetails()
        {
            return View();
        }

        public IActionResult ToDo()
        {
            return View();
        }
        public IActionResult StoreFront()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult FinalProject()
        {
            return View();
        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            //When a class has validation attributes, that validation should be checked BEFORE attempting to process any of the data provided.
            if (!ModelState.IsValid)
            {
                //Send them back to the form. We can pass the object to the view so the form will contain the original information they provided.
                return View(cvm);
            }

            string message = $"You have receiced a new email from your site's contact form!<br/>" +
                $"Sender: {cvm.Name}<br/>Email: {cvm.Email}<br/>Subject: {cvm.Subject}<br/> Message: {cvm.Message}";

            var mm = new MimeMessage();

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            mm.Subject = cvm.Subject;

            mm.Body = new TextPart("HTML") { Text = message };

            mm.Priority = MessagePriority.Urgent;

            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            using (var client = new SmtpClient())
            {
                    client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                    client.Authenticate
                   (
                   //Username
                   _config.GetValue<string>("Credentials:Email:User"),
                   //Password
                   _config.GetValue<string>("Credentials:Email:Password")
                   );

                try
                {
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"There was an error processing your request. Pleas try again later." +
                        $"<br/>Error Message: {ex}";

                    return View(cvm);
                }
            } 
            return View("EmailConfirmation", cvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }//end homeController
}