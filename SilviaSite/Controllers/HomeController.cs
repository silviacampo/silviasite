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
using SilviaSite.Models;
using SilviaSite.Infrastructure;

namespace SilviaSite.Controllers
{
    public class HomeController : Controller
    {
        private IEmailConfiguration _emailConfiguration;

        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IEmailConfiguration emailConfiguration, IStringLocalizer<HomeController> localizer)
        {
            _emailConfiguration = emailConfiguration;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
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

                using (var emailClient = new SmtpClient())
                {
                    try
                    {
                        emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);

                        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                        emailClient.Send(message);

                        return Json(_localizer["MessageSent"]);
                    }
                    catch (Exception e)
                    {
                        return Json(_localizer["MessageFailSent"]);
                    }
                    finally
                    {
                        emailClient.Disconnect(true);
                    }
                }
            }
            else
            {
                return Json(_localizer["MissingFields"]);
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

