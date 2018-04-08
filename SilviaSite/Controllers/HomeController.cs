using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MimeKit;
using MimeKit.Text;
using System.ComponentModel.DataAnnotations;

namespace SilviaSite.Controllers
{
    public class HomeController : Controller
    {
		private IEmailConfiguration _emailConfiguration;

		private readonly IStringLocalizer<HomeController> _localizer;

		public HomeController(IEmailConfiguration emailConfiguration, IStringLocalizer<HomeController> localizer) {
			_emailConfiguration = emailConfiguration;
			_localizer = localizer;
		}

        public IActionResult Index()
        {
			string test = _localizer["TEST"];

			return View();
        }

        [AcceptVerbs("Post")]
        public IActionResult Send([FromForm] EmailMessage emailMessage)
		{
            if (ModelState.IsValid)
            {
                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpUsername));
                message.From.Add(new MailboxAddress(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpUsername));

                message.Subject = emailMessage.Subject;
                message.Body = new TextPart(TextFormat.Text)
                {
                    Text = "from: " + emailMessage.From + Environment.NewLine + "responseRequired: " + emailMessage.Required + Environment.NewLine + emailMessage.Content
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    try {
                        //The last parameter here is to use SSL (Which you should!)
                        emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, _emailConfiguration.SSL);

                        //Remove any OAuth functionality as we won't be using it. 
                        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                        emailClient.Send(message);

                        return Json("MessageSent");
                    }
                    catch (Exception e) {
                        return Json("MessageFailSent");
                    }
                    finally {
                        emailClient.Disconnect(true);
                    }
                                     
                                       
                }
            }
            else
            {
                return Json("MissingFields");
            }
		}

		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }

	public class EmailMessage
	{
		public EmailMessage()
		{
		}
        [Required]
        public string From { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public bool Required { get; set; }
    }

	public class EmailAddress
	{
		public string Name { get; set; }
		public string Address { get; set; }
	}

	public interface IEmailConfiguration
	{
		string SmtpServer { get; }
		int SmtpPort { get; }
		string SmtpUsername { get; set; }
		string SmtpPassword { get; set; }
        bool SSL { get; set; }

        string PopServer { get; }
		int PopPort { get; }
		string PopUsername { get; }
		string PopPassword { get; }
	}

	public class EmailConfiguration : IEmailConfiguration
	{
		public string SmtpServer { get; set; }
		public int SmtpPort { get; set; }
		public string SmtpUsername { get; set; }
		public string SmtpPassword { get; set; }
        public bool SSL { get; set; }

        public string PopServer { get; set; }
		public int PopPort { get; set; }
		public string PopUsername { get; set; }
		public string PopPassword { get; set; }
	}

	public interface IEmailService
	{
		void Send(EmailMessage emailMessage);
		List<EmailMessage> ReceiveEmail(int maxCount = 10);
	}

	public class EmailService : IEmailService
	{
		private readonly IEmailConfiguration _emailConfiguration;

		public EmailService(IEmailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}

		public List<EmailMessage> ReceiveEmail(int maxCount = 10)
		{
			throw new NotImplementedException();
		}

		public void Send(EmailMessage emailMessage)
		{
			throw new NotImplementedException();
		}
	}
}
