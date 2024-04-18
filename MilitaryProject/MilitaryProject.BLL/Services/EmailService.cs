using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Smtp;
using MilitaryProject.BLL.Interfaces;
using static QRCoder.PayloadGenerator;

namespace MilitaryProject.BLL.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message, string smtpServer)
        {
            var smtpClient = new SmtpClient(smtpServer, 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("smtp.gmail.com", "password123"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("smtp.gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
