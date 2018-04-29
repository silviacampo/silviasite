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
        private readonly IEmailService _emailService;

        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IEmailService emailService, IStringLocalizer<HomeController> localizer)
        {
            _emailService = emailService;
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
                if (_emailService.Send(emailMessage)) {
                    return Json(_localizer["MessageSent"]);
                }
                return Json(_localizer["MessageFailSent"]);
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

