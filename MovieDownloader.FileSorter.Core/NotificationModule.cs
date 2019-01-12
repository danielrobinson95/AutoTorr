using System.Net;
using System.Net.Mail;
using MovieDownloader.Models;

namespace MovieDownloader.FileSorter.Core
{
    public static class NotificationModule
    {
        

        public static void SendEmail(string body)
        {
            var settings = Settings.AppSettings;
            var exeName = System.AppDomain.CurrentDomain.FriendlyName;

            var fromAddress = new MailAddress(settings.FromEmailAddress, exeName);
            var toAddress = new MailAddress(settings.ToEmailAddress);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, settings.EmailPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = $"{exeName} Notification",
                Body = body
            })
            {
                smtp.Send(message);
            }

        }
    }
}
