using SilviaSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilviaSite.Infrastructure
{
    public interface IEmailService
    {
        bool Send(EmailMessage emailMessage);
    }
}
