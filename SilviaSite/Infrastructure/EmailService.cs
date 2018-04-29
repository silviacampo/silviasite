using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using SilviaSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilviaSite.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public bool Send(EmailMessage emailMessage)
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

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    emailClient.Disconnect(true);
                }
            }
        }
    }
}
